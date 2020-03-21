//------------------------------------------------------------------------------
//    The MIT License (MIT)
//    
//    Copyright (c) 2019 Arvind Shyamsundar
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
        private List<ModuleInfo> _loadedModules = new List<ModuleInfo>();

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
                    if (foundFiles.Count() > 0)
                    {
                        _DLLOrdinalMap.Add(currmodule, GetExports(foundFiles.First()));

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
        /// Read a XEL file, consume all callstacks, hash them and return the equivalent XML
        /// </summary>
        /// <param name="xelFiles">List of paths to XEL files to read</param>
        /// <returns>XML equivalent of the histogram corresponding to these events</returns>
        public string ExtractFromXEL(string[] xelFiles, bool bucketize)
        {
            var callstackSlots = new Dictionary<string, long>();
            var callstackRaw = new Dictionary<string, string>();
            var xmlEquivalent = new StringBuilder();

            // the below feels quite hacky. Unfortunately till such time that we have strong typing in XELite I believe this is necessary
            var relevantKeyNames = new string[] { "callstack", "call_stack", "stack_frames" };

            foreach (var xelFileName in xelFiles)
            {
                if (File.Exists(xelFileName))
                {
                        var xeStream = new XEFileEventStreamer(xelFileName);

                        xeStream.ReadEventStream(
                            () =>
                            {
                                return Task.CompletedTask;
                            },
                            evt =>
                            {
                                var allStacks = (from actTmp in evt.Actions
                                                 where relevantKeyNames.Contains(actTmp.Key.ToLower())
                                                 select actTmp.Value as string)
                                                    .Union(
                                                    from fldTmp in evt.Fields
                                                    where relevantKeyNames.Contains(fldTmp.Key.ToLower())
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
                                        var evtId = string.Format("File: {0}, Timestamp: {1}, UUID: {2}:",
                                            xelFileName,
                                            evt.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.mi"),
                                            evt.UUID);

                                        lock (callstackRaw)
                                        {
                                            callstackRaw.Add(evtId, callStackString);
                                        }
                                    }
                                }

                                return Task.CompletedTask;
                            },
                            CancellationToken.None).Wait();
                }
            }

            if (bucketize)
            {
                xmlEquivalent.AppendLine("<HistogramTarget truncated=\"0\" buckets=\"256\">");

                foreach (var item in callstackSlots.OrderByDescending(key => key.Value))
                {
                    xmlEquivalent.AppendFormat("<Slot count=\"{0}\"><value>{1}</value></Slot>", item.Value, item.Key);
                    xmlEquivalent.AppendLine();
                }

                xmlEquivalent.AppendLine("</HistogramTarget>");
            }
            else
            {
                foreach (var item in callstackRaw)
                {
                    xmlEquivalent.AppendLine(item.Key);
                    xmlEquivalent.AppendLine(item.Value);
                }
            }

            return xmlEquivalent.ToString();
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

            return moduleName + ".dll+" +
                string.Format("0x{0:X}\r\n", _DLLOrdinalMap[moduleName][int.Parse(mtch.Groups["ordinal"].Value)].Address + offsetSpecified);
        }

        /// <summary>
        /// Helper function to load a DLL and then lookup exported functions. For this we use CLRMD and specifically the PEHeader class
        /// </summary>
        /// <param name="DLLPath"></param>
        /// <returns></returns>
        public unsafe Dictionary<int, ExportedSymbol> GetExports(string DLLPath)
        {
            using (var dllStream = new FileStream(DLLPath, FileMode.Open, FileAccess.Read))
            {
                var dllImage = new PEImage(dllStream);

                var dir = dllImage.OptionalHeader.ExportDirectory;
                var offset = dllImage.RvaToOffset(Convert.ToInt32(dir.VirtualAddress));

                // this is the placeholder for the final mapping of ordinal # to address map
                Dictionary<int, ExportedSymbol> exports = null;

                using (var mmf = MemoryMappedFile.CreateFromFile(dllStream, null, 0, MemoryMappedFileAccess.Read, null, HandleInheritability.None, false))
                {
                    using (var _accessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read))
                    {
                        _accessor.Read(offset, out IMAGE_EXPORT_DIRECTORY exportDirectory);

                        var count = exportDirectory.NumberOfFunctions;
                        exports = new Dictionary<int, ExportedSymbol>(count);

                        var namesOffset = exportDirectory.AddressOfNames != 0 ? dllImage.RvaToOffset(exportDirectory.AddressOfNames) : 0;
                        var ordinalOffset = exportDirectory.AddressOfOrdinals != 0 ? dllImage.RvaToOffset(exportDirectory.AddressOfOrdinals) : 0;
                        var functionsOffset = dllImage.RvaToOffset(exportDirectory.AddressOfFunctions);

                        var ordinalBase = (int)exportDirectory.Base;

                        var name = new sbyte[64];
                        fixed (sbyte* p = name)
                        {
                            for (uint i = 0; i < count; i++)
                            {
                                // read function address
                                var address = _accessor.ReadUInt32(functionsOffset + i * 4);

                                exports.Add((int)(ordinalBase + i), new ExportedSymbol
                                {
                                    Name = string.Format("Ordinal{0}", ordinalBase + i),
                                    Address = address
                                });
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
                        retval[frameNum] = string.Format("{0}+0x{1:X}", moduleName, offset);
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
        private List<string> EnumModuleNames(string[] callStack)
        {
            List<string> uniqueModuleNames = new List<string>();
            var reconstructedCallstack = new StringBuilder();
            foreach (var frame in callStack)
            {
                reconstructedCallstack.AppendLine(frame);
            }

            // using the ".dll!0x" to locate the module names
            var rgxModuleName = new Regex(@"(?<module>\w+)(\.dll(!(?<symbolizedfunc>.+))*)*\s*\+(0[xX])*");
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

            var rgxModuleName = new Regex(@"(?<module>\w+)(\.dll)*\s*\+\s*(0[xX])*(?<offset>[0-9a-fA-F]+)\s*");
            var rgxAlreadySymbolizedFrame = new Regex(@"(?<module>\w+)(\.dll)!(?<symbolizedfunc>.+?)\s*\+\s*(0[xX])*(?<offset>[0-9a-fA-F]+)\s*");

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
                        var something = currentFrame;

                        var myDIAsession = _diautils[matchAlreadySymbolized.Groups["module"].Value]._IDiaSession;
                        myDIAsession.findChildren(myDIAsession.globalScope,
                            SymTagEnum.SymTagNull,
                            matchAlreadySymbolized.Groups["symbolizedfunc"].Value,
                            0,
                            out IDiaEnumSymbols matchedSyms);

                        if (matchedSyms.count > 0)
                        {
                            IDiaSymbol a = matchedSyms.Item(0);

                            // for this 're-lookup source' case, it is appropriate to use the seg / address / offset
                            // than use RVA. Using RVA seems to produce totally incorrect results in many cases
                            var rva = a.addressOffset;
                            var seg = a.addressSection;

                            uint offset = Convert.ToUInt32(matchAlreadySymbolized.Groups["offset"].Value, 16);
                            rva = rva + offset;

                            myDIAsession.findLinesByAddr(seg, rva, 1, out IDiaEnumLineNumbers enumLineNums);

                            string tmpsourceInfo = string.Empty;

                            // only if we found line number information should we append to output 
                            if (enumLineNums.count > 0)
                            {
                                tmpsourceInfo = string.Format("({0}:{1})", enumLineNums.Item(0).sourceFile.fileName, enumLineNums.Item(0).lineNumber);
                            }

                            finalCallstack.AppendFormat("{0}!{1}\t{2}",
                                matchAlreadySymbolized.Groups["module"].Value,
                                matchAlreadySymbolized.Groups["symbolizedfunc"].Value,
                                tmpsourceInfo);
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
            var matchedModule = from mod in _loadedModules
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
            var symKey = moduleName + rva.ToString();

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
                offsetStr = string.Format("+{0}", displacement);
            }

            // try to find if we have source and line number info and include it based on the param
            string sourceInfo = string.Empty;

            if (includeSourceInfo)
            {
                _diautils[moduleName]._IDiaSession.findLinesByRVA(rva, 1, out IDiaEnumLineNumbers enumLineNums);

                // only if we found line number information should we append to output 
                if (enumLineNums.count > 0)
                {
                    sourceInfo = string.Format("({0}:{1})", enumLineNums.Item(0).sourceFile.fileName, enumLineNums.Item(0).lineNumber);
                }
            }

            // make sure we cleanup COM allocations for the resolved sym
            Marshal.FinalReleaseComObject(mysym);

            result = string.Format("{0}!{1}{2}\t{3}", moduleName, funcname2, offsetStr, sourceInfo).Trim();

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

            _loadedModules.Clear();

            var rgxmoduleaddress = new Regex(@"(?<filepath>.+)(\t+| +)(?<baseaddress>0x[0-9a-fA-F`]+)");
            var mcmodules = rgxmoduleaddress.Matches(baseAddressesString);

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

                    _loadedModules.Add(newModule);
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
            _loadedModules = (from mod in _loadedModules orderby mod.BaseAddress select mod).ToList();

            // loop through the list, computing their end address
            for (int moduleIndex = 1; moduleIndex < _loadedModules.Count; moduleIndex++)
            {
                // the previous modules end address will be current module's end address - 1 byte
                _loadedModules[moduleIndex - 1].EndAddress = _loadedModules[moduleIndex].BaseAddress - 1;
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
        private bool LocateandLoadPDBs(Dictionary<string, DiaUtil> _diautils, string rootPaths, bool recurse, List<string> moduleNames, bool cachePDB)
        {
            // loop through each module, trying to find matched PDB files
            var splitRootPaths = rootPaths.Split(';');
            foreach (string currentModule in moduleNames)
            {
                if (!_diautils.ContainsKey(currentModule))
                {
                    // check if the PDB is already cached locally
                    var cachedPDBFile = Path.Combine(Path.GetTempPath(), "SymCache", currentModule + ".pdb");
                    lock (this)
                    {
                        if (!File.Exists(cachedPDBFile))
                        {
                            foreach (var currPath in splitRootPaths)
                            {
                                if (Directory.Exists(currPath))
                                {
                                    var foundFiles = Directory.EnumerateFiles(currPath, currentModule + ".pdb", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                                    if (foundFiles.Count() > 0)
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
            bool cachePDB)
        {
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
            var xmldoc = new XmlDocument();
            bool isXMLdoc = false;

            // we evaluate if the input is XML containing multiple stacks
            try
            {
                xmldoc.LoadXml(inputCallstackText);
                isXMLdoc = true;
            }
            catch (XmlException)
            {
                // do nothing because this is not a XML doc
            }

            var listOfCallStacks = new List<StackWithCount>();

            if (!isXMLdoc)
            {
                listOfCallStacks.Add(new StackWithCount()
                {
                    Callstack = inputCallstackText,
                    Count = 1
                });
            }
            else
            {
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
                        // process individual callstacks
                        foreach (XmlNode currstack in allstacknodes)
                        {
                            var callstackTextNode = currstack.SelectSingleNode("./action[@name = 'callstack'][1]/value[1]");
                            var callstackText = callstackTextNode.InnerText;

                            // proceed to extract the surrounding XML markup
                            callstackTextNode.ParentNode.RemoveChild(callstackTextNode);

                            var eventXMLMarkup = currstack.OuterXml.Replace("\r", string.Empty).Replace("\n", string.Empty);

                            var candidatestack = string.Format("Event details: {0}:\r\n\r\n{1}", eventXMLMarkup, callstackText);
                            listOfCallStacks.Add(new StackWithCount()
                            {
                                Callstack = candidatestack,
                                Count = 1
                            });

                            stacknum++;
                        }
                    }
                }
                else
                {
                    // process histograms
                    foreach (XmlNode currstack in allstacknodes)
                    {
                        var slotcount = int.Parse(currstack.Attributes["count"].Value);
                        var candidatestack = string.Format("Slot_{0}\t[count:{1}]:\r\n\r\n{2}", stacknum, slotcount, currstack.SelectSingleNode("./value[1]").InnerText);
                        listOfCallStacks.Add(new StackWithCount()
                        {
                            Callstack = candidatestack,
                            Count = slotcount
                        });

                        stacknum++;
                    }
                }
            }

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

            // populate the output
            foreach (var currstack in listOfCallStacks)
            {
                if (!string.IsNullOrEmpty(currstack.Resolvedstack))
                {
                    finalCallstack.AppendLine(currstack.Resolvedstack);
                }
                else
                {
                    finalCallstack = new StringBuilder("ERROR: Unable to resolve call stacks - is the file msdia140.dll registered using REGSVR32?");
                    break;
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

            return finalCallstack.ToString();
        }

        /// <summary>
        /// Function executed by worker threads to process callstacks.
        /// Threads work on portions of the listOfCallStacks based on their thread ordinal.
        /// </summary>
        /// <param name="obj"></param>
        private void ProcessCallStack(Object obj)
        {
            var tp = (ThreadParams)obj;

            Dictionary<string, DiaUtil> _diautils = new Dictionary<string, DiaUtil>();

            for (int tmpStackIndex = 0; tmpStackIndex < tp.listOfCallStacks.Count; tmpStackIndex++)
            {
                if (tmpStackIndex % tp.numThreads != tp.threadOrdinal)
                {
                    continue;
                }

                var currstack = tp.listOfCallStacks[tmpStackIndex];

                // split the callstack into lines, and for each line try to resolve
                string ordinalresolvedstack;

                lock (this)
                {
                    ordinalresolvedstack = LoadDllsIfApplicable(currstack.Callstack, tp.searchDLLRecursively, tp.dllPaths);
                }

                // sometimes we see call stacks which are arranged horizontally (this typically is seen in Excel or custom reporting tools)
                // in that case, space is a valid delimiter, and we need to support that as an option
                var delims = tp.framesOnSingleLine ? new char[2] { ' ', '\n' } : new char[1] { '\n' };

                var callStackLines = ordinalresolvedstack.Replace('\r', ' ').Split(delims, StringSplitOptions.RemoveEmptyEntries);

                // process any frames which are purely virtual address (in such cases, the caller should have specified base addresses)
                callStackLines = PreProcessVAs(callStackLines);

                // locate the PDBs and populate their DIA session helper classes
                if (LocateandLoadPDBs(_diautils, tp.symPath, tp.searchPDBsRecursively, EnumModuleNames(callStackLines), tp.cachePDB))
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
            }

            // cleanup any older COM objects
            if (_diautils != null)
            {
                foreach (var diautil in _diautils.Values)
                {
                    diautil.Dispose();
                }
            }
        }

        /// <summary>
        /// This method generates a PowerShell script to automate download of matched PDBs from the public symbol server.
        /// </summary>
        /// <param name="dllSearchPath"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public List<Symbol> GetSymbolDetailsForBinaries(string dllSearchPath, bool recurse)
        {
            if (string.IsNullOrEmpty(dllSearchPath))
            {
                return new List<Symbol>();
            }

            var symbolsFound = new List<Symbol>();

            var moduleNames = new string[] { "ntdll", "kernel32", "kernelbase", "ntoskrnl", "sqldk", "sqlmin", "sqllang", "sqltses", "sqlaccess", "qds", "hkruntime", "hkengine", "hkcompile", "sqlos", "sqlservr" };

            foreach (var currentModule in moduleNames)
            {
                var splitRootPaths = dllSearchPath.Split(';');
                string finalFilePath = null;

                foreach (var currPath in splitRootPaths)
                {
                    if (!Directory.Exists(currPath))
                    {
                        continue;
                    }

                    var foundFiles = from f in Directory.EnumerateFiles(currPath, currentModule + ".*", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                                     where f.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) || f.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)
                                     select f;

                    if (foundFiles.Count() > 0)
                    {
                        finalFilePath = foundFiles.First();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(finalFilePath))
                {
                    using (var dllFileStream = new FileStream(finalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var dllImage = new PEImage(dllFileStream, false);

                        var internalPDBName = dllImage.DefaultPdb.FileName;
                        var pdbGuid = dllImage.DefaultPdb.Guid;
                        var pdbAge = dllImage.DefaultPdb.Revision;

                        var usablePDBName = Path.GetFileNameWithoutExtension(internalPDBName);

                        var newSymbol = new Symbol()
                        {
                            PDBName = usablePDBName,

                            InternalPDBName = internalPDBName,

                            DownloadURL = string.Format(@"https://msdl.microsoft.com/download/symbols/{0}.pdb/{1}/{0}.pdb",
                                usablePDBName,
                                pdbGuid.ToString("N") + pdbAge.ToString()),

                            FileVersion = dllImage.GetFileVersionInfo().FileVersion
                        };

                        newSymbol.DownloadVerified = Symbol.IsURLValid(newSymbol.DownloadURL);

                        symbolsFound.Add(newSymbol);
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
        }
    }
}
