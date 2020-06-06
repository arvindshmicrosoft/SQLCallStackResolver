﻿//------------------------------------------------------------------------------
//    The MIT License (MIT)
//    
//    Copyright (c) Arvind Shyamsundar
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
//    This sample code is not supported under any Microsoft standard support program or service. 
//    The entire risk arising out of the use or performance of the sample scripts and documentation remains with you. 
//    In no event shall Microsoft, its authors, or anyone else involved in the creation, production, or delivery of the scripts
//    be liable for any damages whatsoever (including, without limitation, damages for loss of business profits,
//    business interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability
//    to use the sample scripts or documentation, even if Microsoft has been advised of the possibility of such damages.
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    using Dia;
    using Microsoft.Diagnostics.Runtime.Utilities;
    using Microsoft.SqlServer.XEvent.XELite;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;

    public class StackResolver : IDisposable
    {
        /// <summary>
        /// This is used to store module name and start / end virtual address ranges
        /// Only populated if the user provides a tab-separated string corresponding to the output of the following SQL query:
        /// select name, base_address from sys.dm_os_loaded_modules where name not like '%.rll'
        /// </summary>
        public List<ModuleInfo> LoadedModules = new List<ModuleInfo>();

        /// <summary>
        /// This holds the mapping of the various DLL exports for a module and the address (offset) for each such export
        /// Only populated if the user provides the 'image path' to the DLLs
        /// </summary>
        Dictionary<string, Dictionary<int, ExportedSymbol>> _DLLOrdinalMap;

        /// <summary>
        /// A cache of already resolved addresses
        /// </summary>
        Dictionary<string, string> cachedSymbols = new Dictionary<string, string>();

        /// <summary>
        /// R/W lock to protect the above cached symbols dictionary
        /// </summary>
        ReaderWriterLockSlim rwLockCachedSymbols = new ReaderWriterLockSlim();

        /// <summary>
        /// Status message - populated during associated long-running operations
        /// </summary>
        public string StatusMessage;

        private int globalCounter = 0;

        private bool cancelRequested = false;

        private static object _syncRoot = new object();

        /// <summary>
        /// Percent completed - populated during associated long-running operations
        /// </summary>
        public int PercentComplete;

        /// <summary>
        /// This function loads DLLs from a specified path, so that we can then build the DLL export's ordinal / address map
        /// </summary>
        /// <param name="callstack"></param>
        /// <param name="recurse"></param>
        /// <param name="dllPaths"></param>
        /// <returns></returns>
        private string LoadDllsIfApplicable(string callstack, bool recurse, List<string> dllPaths)
        {
            if (dllPaths == null)
            {
                return callstack;
            }

            _DLLOrdinalMap = new Dictionary<string, Dictionary<int, ExportedSymbol>>();

            // first we seek out distinct module names in this call stack
            // note that such frames will only be seen in the call stack when trace flag 3656 is enabled, but there were no PDBs in the BINN folder
            // sample frames are given below
            // sqldk.dll!Ordinal947+0x25f
            // sqldk.dll!Ordinal699 + 0x5f
            // sqlmin.dll!Ordinal1634 + 0x76c

            // More recent patterns which we choose not to support, because in these cases the module+offset is cleanly represented and it does symbolize nicely
            // 00007FF818405E70 Module(sqlmin+0000000001555E70) (Ordinal1877 + 00000000000004B0)                                  
            // 00007FF81840226A Module(sqlmin+000000000155226A) (Ordinal1261 + 00000000000071EA)                                  
            // 00007FF81555A663 Module(sqllang+0000000000C6A663) (Ordinal1203 + 0000000000005E33)

            // define a regex to identify such ordinal based frames
            var rgxOrdinalNotation = new Regex(@"(?<module>\w+)(\.dll)*!Ordinal(?<ordinal>[0-9]+)\s*\+\s*(0[xX])*");
            var matchednotations = rgxOrdinalNotation.Matches(callstack);

            var moduleNames = new List<string>();
            if (matchednotations.Count > 0)
            {
                foreach (Match match in matchednotations)
                {
                    var currmodule = match.Groups["module"].Value;
                    if (!moduleNames.Contains(currmodule))
                    {
                        moduleNames.Add(currmodule);
                    }
                }
            }

            // then we see if there is a matched DLL in any of the paths we have
            foreach (var currmodule in moduleNames)
            {
                foreach (var currPath in dllPaths)
                {
                    var foundFiles = Directory.EnumerateFiles(currPath, currmodule + ".dll", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                    if (foundFiles.Any())
                    {
                        _DLLOrdinalMap.Add(currmodule, StackResolver.GetExports(foundFiles.First()));

                        break;
                    }
                }
            }

            // finally do a pattern based replace
            // the replace method calls a delegate (ReplaceOrdinalWithRealOffset) which figures out the start address of the ordinal and 
            // then computes the actual offset
            var fullpattern = new Regex(@"(?<module>\w+)(\.dll)*!Ordinal(?<ordinal>[0-9]+)\s*\+\s*(0[xX])*(?<offset>[0-9a-fA-F]+)\s*");
            return fullpattern.Replace(callstack, ReplaceOrdinalWithRealOffset);
        }

        /// <summary>
        /// Read a XEL file, consume all callstacks, optionally bucketize them, and in all cases,
        /// return the information as equivalent XML
        /// </summary>
        /// <param name="xelFiles">List of paths to XEL files to read</param>
        /// <returns>A tuple with the count of events and XML equivalent of the histogram corresponding to these events</returns>
        public Tuple<int, string> ExtractFromXEL(string[] xelFiles, bool bucketize)
        {
            if (xelFiles == null)
            {
                return new Tuple<int, string>(0, string.Empty);
            }

            this.cancelRequested = false;

            var callstackSlots = new Dictionary<string, long>();
            var callstackRaw = new Dictionary<string, string>();
            var xmlEquivalent = new StringBuilder();

            // the below feels quite hacky. Unfortunately till such time that we have strong typing in XELite I believe this is unavoidable
            var relevantKeyNames = new string[] { "callstack", "call_stack", "stack_frames" };

            foreach (var xelFileName in xelFiles)
            {
                if (File.Exists(xelFileName))
                {
                    this.StatusMessage = $@"Reading {xelFileName}...";

                    var xeStream = new XEFileEventStreamer(xelFileName);

                    xeStream.ReadEventStream(
                        () =>
                        {
                            return Task.CompletedTask;
                        },
                        evt =>
                        {
                            var allStacks = (from actTmp in evt.Actions
                                             where relevantKeyNames.Contains(actTmp.Key.ToLower(CultureInfo.CurrentCulture))
                                             select actTmp.Value as string)
                                                .Union(
                                                from fldTmp in evt.Fields
                                                where relevantKeyNames.Contains(fldTmp.Key.ToLower(CultureInfo.CurrentCulture))
                                                select fldTmp.Value as string);

                            foreach (var callStackString in allStacks)
                            {
                                if (string.IsNullOrEmpty(callStackString))
                                {
                                    continue;
                                }

                                if (bucketize)
                                {
                                    lock (callstackSlots)
                                    {
                                        if (!callstackSlots.ContainsKey(callStackString))
                                        {
                                            callstackSlots.Add(callStackString, 1);
                                        }
                                        else
                                        {
                                            callstackSlots[callStackString]++;
                                        }
                                    }
                                }
                                else
                                {
                                    var evtId = string.Format(CultureInfo.CurrentCulture,
                                        "File: {0}, Timestamp: {1}, UUID: {2}:",
                                        xelFileName,
                                        evt.Timestamp.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.CurrentCulture),
                                        evt.UUID);

                                    lock (callstackRaw)
                                    {
                                        if (!callstackRaw.ContainsKey(evtId))
                                        {
                                            callstackRaw.Add(evtId, callStackString);
                                        }
                                        else
                                        {
                                            callstackRaw[evtId] += $"{Environment.NewLine}{callStackString}";
                                        }
                                    }
                                }
                            }

                            return Task.CompletedTask;
                        },
                        CancellationToken.None).Wait();
                }
            }

            this.StatusMessage = "Finished reading file(s), finalizing output...";

            int finalEventCount;

            if (bucketize)
            {
                xmlEquivalent.AppendLine("<HistogramTarget>");
                this.globalCounter = 0;

                foreach (var item in callstackSlots.OrderByDescending(key => key.Value))
                {
                    xmlEquivalent.AppendFormat(CultureInfo.CurrentCulture,
                        "<Slot count=\"{0}\"><value>{1}</value></Slot>",
                        item.Value,
                        item.Key);

                    xmlEquivalent.AppendLine();

                    this.globalCounter++;
                    this.PercentComplete = (int) ((double)this.globalCounter / callstackSlots.Count * 100.0);
                }

                xmlEquivalent.AppendLine("</HistogramTarget>");

                finalEventCount = callstackSlots.Count;
            }
            else
            {
                xmlEquivalent.AppendLine("<Events>");
                this.globalCounter = 0;

                var hasOverflow = false;

                foreach (var item in callstackRaw.OrderBy(key => key.Key))
                {
                    if (xmlEquivalent.Length < int.MaxValue * 0.90)
                    {
                        xmlEquivalent.AppendFormat(CultureInfo.CurrentCulture,
                            "<event key=\"{0}\"><action name='callstack'><value>{1}</value></action></event>",
                            item.Key,
                            item.Value);

                        xmlEquivalent.AppendLine();
                    }
                    else
                    {
                        hasOverflow = true;
                    }

                    this.globalCounter++;
                    this.PercentComplete = (int)((double)this.globalCounter / callstackRaw.Count * 100.0);
                }

                if (hasOverflow) xmlEquivalent.AppendLine("<!-- WARNING: output was truncated due to size limits -->");

                xmlEquivalent.AppendLine("</Events>");

                finalEventCount = callstackRaw.Count;
            }

            this.StatusMessage = $@"Finished processing {xelFiles.Length} XEL files";

            return new Tuple<int, string>(finalEventCount, xmlEquivalent.ToString());
        }

        public void CancelRunningTasks()
        {
            this.cancelRequested = true;
        }

        /// <summary>
        /// This delegate is invoked by the Replace function and is used to compute the effective offset from module load address
        /// based on ordinal start address and original offset
        /// </summary>
        /// <param name="mtch"></param>
        /// <returns></returns>
        internal string ReplaceOrdinalWithRealOffset(Match mtch)
        {
            var moduleName = mtch.Groups["module"].Value;

            if (!_DLLOrdinalMap.ContainsKey(moduleName))
            {
                return mtch.Value;
            }

            uint offsetSpecified = Convert.ToUInt32(mtch.Groups["offset"].Value, 16);

            return string.Format(CultureInfo.CurrentCulture,
                "{0}.dll+0x{1:X}{2}",
                moduleName,
                _DLLOrdinalMap[moduleName][int.Parse(mtch.Groups["ordinal"].Value, CultureInfo.CurrentCulture)].Address + offsetSpecified,
                Environment.NewLine);
        }

        /// <summary>
        /// Helper function to load a DLL and then lookup exported functions. For this we use CLRMD and specifically the PEHeader class
        /// </summary>
        /// <param name="DLLPath"></param>
        /// <returns></returns>
        public static Dictionary<int, ExportedSymbol> GetExports(string DLLPath)
        {
            // this is the placeholder for the final mapping of ordinal # to address map
            Dictionary<int, ExportedSymbol> exports = null;

            using (var dllStream = new FileStream(DLLPath, FileMode.Open, FileAccess.Read))
            {
                using (var dllImage = new PEImage(dllStream))
                {
                    var dir = dllImage.PEHeader.ExportTableDirectory;
                    var offset = dllImage.RvaToOffset(Convert.ToInt32(dir.RelativeVirtualAddress));

                    using (var mmf = MemoryMappedFile.CreateFromFile(dllStream, null, 0, MemoryMappedFileAccess.Read, null, HandleInheritability.None, false))
                    {
                        using (var mmfAccessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read))
                        {
                            mmfAccessor.Read(offset, out ImageExportDirectory exportDirectory);

                            var count = exportDirectory.NumberOfFunctions;
                            exports = new Dictionary<int, ExportedSymbol>(count);

                            var namesOffset = exportDirectory.AddressOfNames != 0 ? dllImage.RvaToOffset(exportDirectory.AddressOfNames) : 0;
                            var ordinalOffset = exportDirectory.AddressOfOrdinals != 0 ? dllImage.RvaToOffset(exportDirectory.AddressOfOrdinals) : 0;
                            var functionsOffset = dllImage.RvaToOffset(exportDirectory.AddressOfFunctions);

                            var ordinalBase = (int)exportDirectory.Base;

                            for (uint funcOrdinal = 0; funcOrdinal < count; funcOrdinal++)
                            {
                                // read function address
                                var address = mmfAccessor.ReadUInt32(functionsOffset + funcOrdinal * 4);

                                if (0 != address)
                                {
                                    exports.Add((int)(ordinalBase + funcOrdinal), new ExportedSymbol
                                    {
                                        Name = string.Format(CultureInfo.CurrentCulture,
                                            "Ordinal{0}",
                                            ordinalBase + funcOrdinal),
                                        Address = address
                                    });
                                }
                            }
                        }
                    }
                }

                return exports;
            }
        }

        /// <summary>
        /// Convert virtual-address only type frames to their module+offset format
        /// </summary>
        /// <param name="callStackLines"></param>
        /// <returns></returns>
        private string[] PreProcessVAs(string[] callStackLines)
        {
            var rgxVAOnly = new Regex(@"^\s*0[xX](?<vaddress>[0-9a-fA-F]+)\s*$");
            string[] retval = new string[callStackLines.Length];

            int frameNum = 0;
            foreach (var currentFrame in callStackLines)
            {
                // let's see if this is an VA-only address
                var matchVA = rgxVAOnly.Match(currentFrame);

                if (matchVA.Success)
                {
                    ulong virtAddress = Convert.ToUInt64(matchVA.Groups["vaddress"].Value, 16);

                    if (TryObtainModuleOffset(virtAddress, out string moduleName, out uint offset))
                    {
                        // finalCallstack.AppendLine(ProcessFrameModuleOffset(moduleName, offset.ToString()));
                        retval[frameNum] = string.Format(CultureInfo.CurrentCulture,
                            "{0}+0x{1:X}",
                            moduleName, offset);
                    }
                    else
                    {
                        retval[frameNum] = currentFrame.Trim();
                    }
                }
                else
                {
                    retval[frameNum] = currentFrame.Trim();
                }

                frameNum++;
            }

            return retval;
        }

        /// <summary>
        /// Find out distinct module names in a given stack. This is used to later load PDBs and optionally DLLs
        /// </summary>
        /// <param name="callStack"></param>
        /// <returns></returns>
        private static List<string> EnumModuleNames(string[] callStack)
        {
            List<string> uniqueModuleNames = new List<string>();
            var reconstructedCallstack = new StringBuilder();
            foreach (var frame in callStack)
            {
                reconstructedCallstack.AppendLine(frame);
            }

            // using the ".dll!0x" to locate the module names
            var rgxModuleName = new Regex(@"(?<module>\w+)((\.(dll|exe))*(!(?<symbolizedfunc>.+))*)*\s*\+(0[xX])*");
            var matchedModuleNames = rgxModuleName.Matches(reconstructedCallstack.ToString());

            foreach (Match moduleMatch in matchedModuleNames)
            {
                var actualModuleName = moduleMatch.Groups["module"].Value;

                if (!uniqueModuleNames.Contains(actualModuleName))
                {
                    uniqueModuleNames.Add(actualModuleName);
                }
            }

            return uniqueModuleNames;
        }

        /// <summary>
        /// Runs through each of the frames in a call stack and looks up symbols for each
        /// </summary>
        /// <param name="_diautils">The DIA helper instance</param>
        /// <param name="callStackLines">Call stack string</param>
        /// <param name="includeSourceInfo">Whether to include source / line info</param>
        /// <param name="relookupSource">Boolean used to control if we attempt to relookup source information</param>
        /// <returns></returns>
        private string ResolveSymbols(Dictionary<string, 
            DiaUtil> _diautils, 
            string[] callStackLines,
            bool includeSourceInfo,
            bool relookupSource,
            bool includeOffsets)
        {
            var finalCallstack = new StringBuilder();

            var rgxModuleName = new Regex(@"(?<module>\w+)(\.(dll|exe))*\s*\+\s*(0[xX])*(?<offset>[0-9a-fA-F]+)\s*");
            var rgxAlreadySymbolizedFrame = new Regex(@"(?<module>\w+)(\.(dll|exe))*!(?<symbolizedfunc>.+?)\s*\+\s*(0[xX])*(?<offset>[0-9a-fA-F]+)\s*");

            foreach (var iterFrame in callStackLines)
            {
                // hard-coded find-replace for XML markup - useful when importing from XML histograms
                var currentFrame = iterFrame.Replace("&lt;", "<").Replace("&gt;", ">");

                if (relookupSource && includeSourceInfo)
                {
                    // This is a rare case. Sometimes we get frames which are already resolved to their symbols but do not include source and line number information
                    // take for example     sqldk.dll!SpinlockBase::Sleep+0x2d0
                    // in these cases, we may want to 're-resolve' them to a symbol using DIA so that later
                    // we can embed source / line number information if that is available now (this is important for some
                    // Microsoft internal cases where customers send us stacks resolved with public PDBs but internally we
                    // have private PDBs so we want to now leverage the extra information provided in the private PDBs.)
                    var matchAlreadySymbolized = rgxAlreadySymbolizedFrame.Match(currentFrame);
                    if (matchAlreadySymbolized.Success && _diautils.ContainsKey(matchAlreadySymbolized.Groups["module"].Value))
                    {
                        var myDIAsession = _diautils[matchAlreadySymbolized.Groups["module"].Value]._IDiaSession;
                        myDIAsession.findChildrenEx(myDIAsession.globalScope,
                            SymTagEnum.SymTagNull,
                            matchAlreadySymbolized.Groups["symbolizedfunc"].Value,
                            0,
                            out IDiaEnumSymbols matchedSyms);

                        if (matchedSyms.count > 0)
                        {
                            for (uint tmpOrdinal = 0; tmpOrdinal < matchedSyms.count; tmpOrdinal++)
                            {
                                IDiaSymbol a = matchedSyms.Item(tmpOrdinal);
                                var rva = a.relativeVirtualAddress;

                                string offsetString = matchAlreadySymbolized.Groups["offset"].Value;
                                int numberBase = offsetString.ToUpperInvariant().StartsWith("0X", StringComparison.CurrentCulture) ? 16 : 10;

                                uint offset = Convert.ToUInt32(offsetString, numberBase);
                                rva += offset;

                                myDIAsession.findLinesByRVA(rva, 0, out IDiaEnumLineNumbers enumLineNums);

                                string tmpsourceInfo = string.Empty;

                                // only if we found line number information should we append to output
                                if (enumLineNums.count > 0)
                                {
                                    tmpsourceInfo = string.Format(CultureInfo.CurrentCulture,
                                        "({0}:{1})",
                                        enumLineNums.Item(0).sourceFile.fileName,
                                        enumLineNums.Item(0).lineNumber);
                                }

                                if (tmpOrdinal > 0)
                                {
                                    finalCallstack.Append(" OR ");
                                }

                                finalCallstack.AppendFormat(CultureInfo.CurrentCulture,
                                    "{0}!{1}{2}\t{3}",
                                    matchAlreadySymbolized.Groups["module"].Value,
                                    matchAlreadySymbolized.Groups["symbolizedfunc"].Value,
                                    includeOffsets ? "+" + offsetString : string.Empty,
                                    tmpsourceInfo);
                            }
                        }
                        else
                        {
                            // in the rare case that the symbol does not exist, return frame as-is
                            finalCallstack.Append(currentFrame);
                        }

                        finalCallstack.AppendLine();

                        continue;
                    }
                }

                var match = rgxModuleName.Match(currentFrame);

                if (match.Success)
                {
                    var matchedModuleName = match.Groups["module"].Value;
                    if (_diautils.ContainsKey(matchedModuleName))
                    {
                        string processedFrame = ProcessFrameModuleOffset(_diautils, 
                            matchedModuleName, 
                            match.Groups["offset"].Value,
                            includeSourceInfo,
                            includeOffsets
                            );

                        if (!string.IsNullOrEmpty(processedFrame))
                        {
                            // typically this is because we could not find the offset in any known function range
                            finalCallstack.AppendLine(processedFrame);
                        }
                        else
                        {
                            finalCallstack.AppendLine(currentFrame);
                        }
                    }
                    else
                    {
                        finalCallstack.AppendLine(currentFrame.Trim());
                    }
                }
                else
                {
                    finalCallstack.AppendLine(currentFrame.Trim());
                }
            }

            return finalCallstack.ToString();
        }

        /// <summary>
        /// This function will check if we have a module corresponding to the load address. Only used for pure virtual address format frames.
        /// </summary>
        /// <param name="virtAddress"></param>
        /// <param name="moduleName"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private bool TryObtainModuleOffset(ulong virtAddress, out string moduleName, out uint offset)
        {
            var matchedModule = from mod in LoadedModules
                                where (mod.BaseAddress <= virtAddress && virtAddress <= mod.EndAddress)
                                select mod;

            // we must have exactly one match (else either there's no matching module or we've got flawed load address data
            if (matchedModule.Count() != 1)
            {
                moduleName = null;
                offset = 0;

                return false;
            }

            moduleName = matchedModule.First().ModuleName;

            // compute the offset / RVA now
            offset = (uint)(virtAddress - matchedModule.First().BaseAddress);

            return true;
        }

        /// <summary>
        /// This is the most important function in this whole utility! It uses DIA to lookup the symbol based on RVA offset
        /// It also looks up line number information if available and then formats all of this information for returning to caller
        /// </summary>
        /// <param name="_diautils">The DIA helper instance</param>
        /// <param name="moduleName">Module name</param>
        /// <param name="offset">RVA offset within the module</param>
        /// <param name="includeSourceInfo">Whether to include source / line info</param>
        /// <param name="includeOffset">Whether to include func offset in output</param>
        /// <returns></returns>
        private string ProcessFrameModuleOffset(Dictionary<string, 
            DiaUtil> _diautils, 
            string moduleName, 
            string offset,
            bool includeSourceInfo,
            bool includeOffset)
        {
            bool useUndecorateLogic = false;

            // the offsets in the XE output are in hex, so we convert to base-10 accordingly
            var rva = Convert.ToUInt32(offset, 16);
            var symKey = moduleName + rva.ToString(CultureInfo.CurrentCulture);

            string result = null;
            this.rwLockCachedSymbols.EnterReadLock();
            if (this.cachedSymbols.ContainsKey(symKey))
            {
                result = this.cachedSymbols[symKey];
            }
            this.rwLockCachedSymbols.ExitReadLock();

            if (!string.IsNullOrEmpty(result))
            {
                // value was in cache
                return result;
            }

            // process the function name (symbol)
            // initially we look for 'block' symbols, which have a parent function
            // typically this is seen in kernelbase.dll 
            // (not very important for XE callstacks but important if you have an assert or non-yielding stack in SQLDUMPnnnn.txt files...)
            _diautils[moduleName]._IDiaSession.findSymbolByRVAEx(rva,
                SymTagEnum.SymTagBlock,
                out IDiaSymbol mysym,
                out int displacement);

            if (mysym != null)
            {
                uint blockAddress = mysym.addressOffset;

                // if we did find a block symbol then we look for its parent till we find either a function or public symbol
                // an addition check is on the name of the symbol being non-null and non-empty
                while (!(mysym.symTag == (uint) SymTagEnum.SymTagFunction || mysym.symTag == (uint)Dia.SymTagEnum.SymTagPublicSymbol) && string.IsNullOrEmpty(mysym.name))
                {
                    mysym = mysym.lexicalParent;
                }

                // Calculate offset into the function by assuming that the final lexical parent we found in the loop above
                // is the actual start of the function. Then the difference between (the original block start function start + displacement) 
                // and final lexical parent's start addresses is the final "displacement" / offset to be displayed
                displacement = (int) (blockAddress - mysym.addressOffset + displacement);
            }
            else
            {
                // we did not find a block symbol, so let's see if we get a Function symbol itself
                // generally this is going to return mysym as null for most users (because public PDBs do not tag the functions as Function
                // they instead are tagged as PublicSymbol)
                _diautils[moduleName]._IDiaSession.findSymbolByRVAEx(rva,
                    SymTagEnum.SymTagFunction,
                    out mysym,
                    out displacement);

                if (mysym == null)
                {
                    useUndecorateLogic = true;

                    // based on previous remarks, look for public symbol near the offset / RVA
                    _diautils[moduleName]._IDiaSession.findSymbolByRVAEx(rva,
                        SymTagEnum.SymTagPublicSymbol,
                        out mysym,
                        out displacement);
                }
            }

            if (mysym == null)
            {
                // if all attempts to locate a matching symbol have failed, return null
                return null;
            }

            // we are now just using the name property instead of calling the undecorated name function
            string funcname2;

            if (!useUndecorateLogic)
            {
                funcname2 = mysym.name;
            }
            else
            {
                // refer https://msdn.microsoft.com/en-us/library/kszfk0fs.aspx
                // UNDNAME_NAME_ONLY == 0x1000: Gets only the name for primary declaration; returns just [scope::]name. Expands template params. 
                mysym.get_undecoratedNameEx(0x1000, out funcname2);

                // catch-all / fallback
                if (string.IsNullOrEmpty(funcname2))
                {
                    funcname2 = mysym.name;
                }
            }

            string offsetStr = string.Empty;

            if (includeOffset)
            {
                offsetStr = string.Format(CultureInfo.CurrentCulture,
                    "+{0}",
                    displacement);
            }

            // try to find if we have source and line number info and include it based on the param
            string sourceInfo = string.Empty;

            if (includeSourceInfo)
            {
                _diautils[moduleName]._IDiaSession.findLinesByRVA(rva, 0, out IDiaEnumLineNumbers enumLineNums);

                // only if we found line number information should we append to output 
                if (enumLineNums.count > 0)
                {
                    for (uint tmpOrdinal = 0; tmpOrdinal < enumLineNums.count; tmpOrdinal++)
                    {
                        if (tmpOrdinal > 0)
                        {
                            sourceInfo += " -- WARN: multiple matches -- ";
                        }

                        sourceInfo += string.Format(CultureInfo.CurrentCulture,
                            "({0}:{1})",
                            enumLineNums.Item(0).sourceFile.fileName,
                            enumLineNums.Item(0).lineNumber);
                    }
                }
                else
                {
                    if (_diautils[moduleName].HasSourceInfo)
                    {
                        sourceInfo = "-- WARN: unable to find source info --";
                    }
                }
            }

            // make sure we cleanup COM allocations for the resolved sym
            Marshal.FinalReleaseComObject(mysym);

            result = string.Format(CultureInfo.CurrentCulture,
                "{0}!{1}{2}\t{3}",
                moduleName,
                funcname2,
                offsetStr,
                sourceInfo).Trim();

            this.rwLockCachedSymbols.EnterWriteLock();
            if (!this.cachedSymbols.ContainsKey(symKey))
            {
                this.cachedSymbols.Add(symKey, result);
            }
            this.rwLockCachedSymbols.ExitWriteLock();

            return result;
        }

        /// <summary>
        /// This helper function parses the output of the sys.dm_os_loaded_modules query 
        /// and constructs an internal map of each modules start and end virtual address
        /// </summary>
        /// <param name="baseAddressesString"></param>
        public bool ProcessBaseAddresses(string baseAddressesString)
        {
            bool retVal = true;

            if (string.IsNullOrEmpty(baseAddressesString))
            {
                // changed this to return true because this is not a true error condition
                return true;
            }

            LoadedModules.Clear();

            var rgxmoduleaddress = new Regex(
                @"^\s*(?<filepath>.+)(\t+| +)(?<baseaddress>(0x)*[0-9a-fA-F`]+)\s*$",
                RegexOptions.Multiline);

            var mcmodules = rgxmoduleaddress.Matches(baseAddressesString);

            if (mcmodules.Count == 0)
            {
                // it is likely that we have malformed input, cannot ignore this so return false.
                return false;
            }

            try
            {
                foreach (Match matchedmoduleinfo in mcmodules)
                {
                    var newModule = new ModuleInfo()
                    {
                        ModuleName = Path.GetFileNameWithoutExtension(matchedmoduleinfo.Groups["filepath"].Value),
                        BaseAddress = Convert.ToUInt64(matchedmoduleinfo.Groups["baseaddress"].Value.Replace("`", string.Empty), 16),
                        EndAddress = ulong.MaxValue // stub this with an 'infinite' end address; only the highest loaded module will end up with this value finally
                    };

                    LoadedModules.Add(newModule);
                }
            }
            catch(FormatException)
            {
                // typically errors with non-numeric info passed to  Convert.ToUInt64
                retVal = false;
            }
            catch (ArgumentException)
            {
                // typically these are malformed paths passed to Path.GetFileNameWithoutExtension
                retVal = false;
            }

            // sort them by base address
            LoadedModules = (from mod in LoadedModules orderby mod.BaseAddress select mod).ToList();

            // loop through the list, computing their end address
            for (int moduleIndex = 1; moduleIndex < LoadedModules.Count; moduleIndex++)
            {
                // the previous modules end address will be current module's end address - 1 byte
                LoadedModules[moduleIndex - 1].EndAddress = LoadedModules[moduleIndex].BaseAddress - 1;
            }

            return retVal;
        }

        /// <summary>
        /// This function builds up the PDB map, by searching for matched PDBs (based on name) and constructing the DIA session for each
        /// It is VERY important to specify the PDB search paths correctly, because there is no 'signature' information available 
        /// to match the PDB in any automatic way.
        /// </summary>
        /// <param name="_diautils"></param>
        /// <param name="rootPaths"></param>
        /// <param name="recurse"></param>
        /// <param name="moduleNames"></param>
        /// <param name="cachePDB">Cache a copy of PDBs into %TEMP%\SymCache</param>
        private static bool LocateandLoadPDBs(Dictionary<string, DiaUtil> _diautils, string rootPaths, bool recurse, List<string> moduleNames, bool cachePDB)
        {
            // loop through each module, trying to find matched PDB files
            var splitRootPaths = rootPaths.Split(';');
            foreach (string currentModule in moduleNames)
            {
                if (!_diautils.ContainsKey(currentModule))
                {
                    // check if the PDB is already cached locally
                    var cachedPDBFile = Path.Combine(Path.GetTempPath(), "SymCache", currentModule + ".pdb");
                    lock (_syncRoot)
                    {
                        if (!File.Exists(cachedPDBFile))
                        {
                            foreach (var currPath in splitRootPaths)
                            {
                                if (Directory.Exists(currPath))
                                {
                                    var foundFiles = Directory.EnumerateFiles(currPath, currentModule + ".pdb", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                                    if (foundFiles.Any())
                                    {
                                        if (cachePDB)
                                        {
                                            File.Copy(foundFiles.First(), cachedPDBFile);
                                        }
                                        else
                                        {
                                            cachedPDBFile = foundFiles.First();
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (File.Exists(cachedPDBFile))
                    {
                        try
                        {
                            _diautils.Add(currentModule, new DiaUtil(cachedPDBFile));
                        }
                        catch (COMException)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// This is what the caller will invoke to resolve symbols
        /// </summary>
        /// <param name="inputCallstackText">the input call stack text or XML</param>
        /// <param name="symPath">PDB search paths; separated by semi-colons. The first path containing a 'matching' PDB will be used.</param>
        /// <param name="searchPDBsRecursively">search for PDBs recursively in each path specified</param>
        /// <param name="dllPaths">DLL search paths. this is optional unless the call stack has frames of the form dll!OrdinalNNN+offset</param>
        /// <param name="searchDLLRecursively">Search for DLLs recursively in each path specified. The first path containing a 'matching' DLL will be used.</param>
        /// <param name="framesOnSingleLine">Mostly set this to false except when frames are on the same line and separated by spaces.</param>
        /// <param name="includeSourceInfo">This is used to control whether source information is included (in the case that private PDBs are available)</param>
        /// <param name="relookupSource">Boolean used to control if we attempt to relookup source information</param>
        /// <param name="includeOffsets">Whether to output func offsets or not as part of output</param>
        /// <returns></returns>
        public string ResolveCallstacks(string inputCallstackText, 
            string symPath, 
            bool searchPDBsRecursively, 
            List<string> dllPaths, 
            bool searchDLLRecursively, 
            bool framesOnSingleLine, 
            bool includeSourceInfo,
            bool relookupSource,
            bool includeOffsets,
            bool cachePDB,
            string outputFilePath)
        {
            this.cancelRequested = false;

            this.cachedSymbols.Clear();

            // delete and recreate the cached PDB folder
            var symCacheFolder = Path.Combine(Path.GetTempPath(), "SymCache");
            if (Directory.Exists(symCacheFolder))
            {
                new DirectoryInfo(symCacheFolder).GetFiles("*", SearchOption.AllDirectories)
                    .ToList()
                    .ForEach(file => file.Delete());
            }
            else
            {
                Directory.CreateDirectory(symCacheFolder);
            }

            var finalCallstack = new StringBuilder();
            var xmldoc = new XmlDocument() { XmlResolver = null };
            bool isXMLdoc = false;

            // we evaluate if the input is XML containing multiple stacks
            try
            {
                this.PercentComplete = 0;
                this.StatusMessage = "Inspecting input to determine processing plan...";

                using (var sreader = new StringReader(inputCallstackText))
                {
                    using (var reader = XmlReader.Create(sreader, new XmlReaderSettings() { XmlResolver = null }))
                    {
                        xmldoc.Load(reader);
                    }
                }

                isXMLdoc = true;
            }
            catch (XmlException)
            {
                // do nothing because this is not a XML doc
            }

            var listOfCallStacks = new List<StackWithCount>();

            if (!isXMLdoc)
            {
                this.StatusMessage = "Input being treated as a single callstack...";
                listOfCallStacks.Add(new StackWithCount()
                {
                    Callstack = inputCallstackText,
                    Count = 1
                });
            }
            else
            {
                this.StatusMessage = "Input is well formed XML, proceeding...";

                // since the input was XML containing multiple stacks, construct the list of stacks to process
                int stacknum = 0;
                var allstacknodes = xmldoc.SelectNodes("/HistogramTarget/Slot");

                // handle the case wherein we are dealing with a ring buffer output with individual events
                // and not a histogram
                if (0 == allstacknodes.Count)
                {
                    allstacknodes = xmldoc.SelectNodes("//event[count(./action[@name = 'callstack']) > 0]");

                    if (allstacknodes.Count > 0)
                    {
                        this.StatusMessage = "Preprocessing XEvent events...";

                        // process individual callstacks
                        foreach (XmlNode currstack in allstacknodes)
                        {
                            if (this.cancelRequested)
                            {
                                return "Operation cancelled.";
                            }

                            var callstackTextNode = currstack.SelectSingleNode("./action[@name = 'callstack'][1]/value[1]");
                            var callstackText = callstackTextNode.InnerText;

                            // proceed to extract the surrounding XML markup
                            callstackTextNode.ParentNode.RemoveChild(callstackTextNode);

                            var eventXMLMarkup = currstack.OuterXml.Replace("\r", string.Empty).Replace("\n", string.Empty);

                            var candidatestack = string.Format(CultureInfo.CurrentCulture,
                                "Event details: {0}:{2}{2}{1}", eventXMLMarkup, callstackText, Environment.NewLine);

                            listOfCallStacks.Add(new StackWithCount()
                            {
                                Callstack = candidatestack,
                                Count = 1
                            });

                            stacknum++;
                            this.PercentComplete = (int)((double)stacknum / allstacknodes.Count * 100.0);
                        }
                    }
                    else
                    {
                        this.StatusMessage = "XML input was detected but it does not appear to be a known schema. Cannot proceed, sorry!";
                    }
                }
                else
                {
                    this.StatusMessage = "Preprocessing XEvent histogram slots...";

                    // process histograms
                    foreach (XmlNode currstack in allstacknodes)
                    {
                        if (this.cancelRequested)
                        {
                            return "Operation cancelled.";
                        }

                        var slotcount = int.Parse(currstack.Attributes["count"].Value,
                            CultureInfo.CurrentCulture);

                        var candidatestack = string.Format(CultureInfo.CurrentCulture,
                            "Slot_{0}\t[count:{1}]:{3}{3}{2}",
                            stacknum, slotcount,
                            currstack.SelectSingleNode("./value[1]").InnerText,
                            Environment.NewLine);

                        listOfCallStacks.Add(new StackWithCount()
                        {
                            Callstack = candidatestack,
                            Count = slotcount
                        });

                        stacknum++;
                        this.PercentComplete = (int)((double)stacknum / allstacknodes.Count * 100.0);
                    }
                }
            }

            this.StatusMessage = "Resolving callstacks to symbols...";
            this.globalCounter = 0;

            // Create a pool of threads to process in parallel
            int numThreads = Math.Min(listOfCallStacks.Count, Environment.ProcessorCount);
            List<Thread> threads = new List<Thread>();
            for (int threadOrdinal = 0; threadOrdinal < numThreads; threadOrdinal++)
            {
                var tmpThread = new Thread(ProcessCallStack);
                threads.Add(tmpThread);
                tmpThread.Start(new ThreadParams()
                {
                    dllPaths = dllPaths,
                    framesOnSingleLine = framesOnSingleLine,
                    includeOffsets = includeOffsets,
                    includeSourceInfo = includeSourceInfo,
                    listOfCallStacks = listOfCallStacks,
                    numThreads = numThreads,
                    relookupSource = relookupSource,
                    searchDLLRecursively = searchDLLRecursively,
                    searchPDBsRecursively = searchPDBsRecursively,
                    symPath = symPath,
                    threadOrdinal = threadOrdinal,
                    cachePDB = cachePDB
                });
            }

            foreach (var tmpThread in threads)
            {
                tmpThread.Join();
            }

            if (this.cancelRequested)
            {
                return "Operation cancelled.";
            }

            this.StatusMessage = "Done with symbol resolution, finalizing output...";

            this.globalCounter = 0;

            // populate the output
            if (!string.IsNullOrEmpty(outputFilePath))
            {
                this.StatusMessage = $@"Writing output to file {outputFilePath}";
                using (var outStream = new StreamWriter(outputFilePath, false))
                {
                    foreach (var currstack in listOfCallStacks)
                    {
                        if (this.cancelRequested)
                        {
                            return "Operation cancelled.";
                        }

                        if (!string.IsNullOrEmpty(currstack.Resolvedstack))
                        {
                            outStream.WriteLine(currstack.Resolvedstack);
                        }
                        else
                        {
                            outStream.WriteLine("ERROR: Unable to resolve call stacks - is the file msdia140.dll registered using REGSVR32?");
                            break;
                        }

                        this.globalCounter++;
                        this.PercentComplete = (int)((double)this.globalCounter / listOfCallStacks.Count * 100.0);
                    }
                }
            }
            else
            {
                this.StatusMessage = "Consolidating output for screen display...";

                foreach (var currstack in listOfCallStacks)
                {
                    if (this.cancelRequested)
                    {
                        return "Operation cancelled.";
                    }

                    if (!string.IsNullOrEmpty(currstack.Resolvedstack))
                    {
                        finalCallstack.AppendLine(currstack.Resolvedstack);
                    }
                    else
                    {
                        finalCallstack = new StringBuilder("ERROR: Unable to resolve call stacks - is the file msdia140.dll registered using REGSVR32?");
                        break;
                    }

                    this.globalCounter++;
                    this.PercentComplete = (int)((double)this.globalCounter / listOfCallStacks.Count * 100.0);
                }
            }

            // Unfortunately the below is necessary to ensure that the handles to the cached PDB files opened by DIA 
            // and later deleted at the next invocation of this function, are released deterministically
            // This is despite we correctly releasing those interface pointers using Marshal.FinalReleaseComObject
            // Thankfully we only need to resort to this if the caller wants to cache PDBs in the temp folder
            if (cachePDB)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            this.StatusMessage = "Finished!";

            if (string.IsNullOrEmpty(outputFilePath))
            {
                return finalCallstack.ToString();
            }
            else
            {
                return $@"Output has been saved to {outputFilePath}";
            }
        }

        /// <summary>
        /// Function executed by worker threads to process callstacks.
        /// Threads work on portions of the listOfCallStacks based on their thread ordinal.
        /// </summary>
        /// <param name="obj"></param>
        private void ProcessCallStack(Object obj)
        {
            SafeNativeMethods.EstablishActivationContext();

            var tp = (ThreadParams)obj;

            Dictionary<string, DiaUtil> _diautils = new Dictionary<string, DiaUtil>();

            for (int tmpStackIndex = 0; tmpStackIndex < tp.listOfCallStacks.Count; tmpStackIndex++)
            {
                if (this.cancelRequested)
                {
                    break;
                }

                if (tmpStackIndex % tp.numThreads != tp.threadOrdinal)
                {
                    continue;
                }

                var currstack = tp.listOfCallStacks[tmpStackIndex];

                // split the callstack into lines, and for each line try to resolve
                string ordinalresolvedstack;

                lock (_syncRoot)
                {
                    ordinalresolvedstack = LoadDllsIfApplicable(currstack.Callstack, tp.searchDLLRecursively, tp.dllPaths);
                }

                // sometimes we see call stacks which are arranged horizontally (this typically is seen when copy-pasting directly
                // from the SSMS XEvent window (copying the callstack field without opening it in its own viewer)
                // in that case, space is a valid delimiter, and we need to support that as an option
                var delims = tp.framesOnSingleLine ? new char[3] { ' ', '\t', '\n' } : new char[1] { '\n' };

                var callStackLines = ordinalresolvedstack.Replace('\r', ' ').Split(delims, StringSplitOptions.RemoveEmptyEntries);

                // process any frames which are purely virtual address (in such cases, the caller should have specified base addresses)
                callStackLines = PreProcessVAs(callStackLines);

                // locate the PDBs and populate their DIA session helper classes
                if (LocateandLoadPDBs(_diautils,
                    tp.symPath,
                    tp.searchPDBsRecursively,
                    EnumModuleNames(callStackLines),
                    tp.cachePDB))
                {
                    // resolve symbols by using DIA
                    currstack.Resolvedstack = ResolveSymbols(_diautils,
                        callStackLines,
                        tp.includeSourceInfo,
                        tp.relookupSource,
                        tp.includeOffsets);
                }
                else
                {
                    currstack.Resolvedstack = string.Empty;
                    break;
                }

                var localCounter = Interlocked.Increment(ref this.globalCounter);
                this.PercentComplete = (int)((double)localCounter / tp.listOfCallStacks.Count * 100.0);
            }

            // cleanup any older COM objects
            if (_diautils != null)
            {
                foreach (var diautil in _diautils.Values)
                {
                    diautil.Dispose();
                }
            }

            SafeNativeMethods.DestroyActivationContext();
        }

        /// <summary>
        /// This method generates a PowerShell script to automate download of matched PDBs from the public symbol server.
        /// </summary>
        /// <param name="dllSearchPath"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public static List<Symbol> GetSymbolDetailsForBinaries(List<string> dllPaths, bool recurse)
        {
            if (dllPaths == null || dllPaths.Count == 0)
            {
                return new List<Symbol>();
            }

            var symbolsFound = new List<Symbol>();

            var moduleNames = new string[] { "ntdll", "kernel32", "kernelbase", "ntoskrnl", "sqldk", "sqlmin", "sqllang", "sqltses", "sqlaccess", "qds", "hkruntime", "hkengine", "hkcompile", "sqlos", "sqlservr" };

            foreach (var currentModule in moduleNames)
            {
                string finalFilePath = null;

                foreach (var currPath in dllPaths)
                {
                    if (!Directory.Exists(currPath))
                    {
                        continue;
                    }

                    var foundFiles = from f in Directory.EnumerateFiles(currPath, currentModule + ".*", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                                     where f.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) || f.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)
                                     select f;

                    if (foundFiles.Any())
                    {
                        finalFilePath = foundFiles.First();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(finalFilePath))
                {
                    using (var dllFileStream = new FileStream(finalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var dllImage = new PEImage(dllFileStream, false))
                        {

                            var internalPDBName = dllImage.DefaultPdb.Path;
                            var pdbGuid = dllImage.DefaultPdb.Guid;
                            var pdbAge = dllImage.DefaultPdb.Revision;

                            var usablePDBName = Path.GetFileNameWithoutExtension(internalPDBName);

                            var newSymbol = new Symbol()
                            {
                                PDBName = usablePDBName,

                                InternalPDBName = internalPDBName,

                                DownloadURL = string.Format(
                                    CultureInfo.CurrentCulture,
                                    @"https://msdl.microsoft.com/download/symbols/{0}.pdb/{1}/{0}.pdb",
                                    usablePDBName,
                                    pdbGuid.ToString("N", CultureInfo.CurrentCulture) + 
                                    pdbAge.ToString(CultureInfo.CurrentCulture)),

                                FileVersion = dllImage.GetFileVersionInfo().FileVersion
                            };

                            newSymbol.DownloadVerified = Symbol.IsURLValid(new Uri(newSymbol.DownloadURL));

                            symbolsFound.Add(newSymbol);
                        }
                    }
                }
            }

            return symbolsFound;
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    rwLockCachedSymbols.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
