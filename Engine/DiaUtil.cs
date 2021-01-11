//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Wrapper class around DIA
    /// </summary>
    internal class DiaUtil
    {
        internal IDiaDataSource _IDiaDataSource;
        internal IDiaSession _IDiaSession;
        private bool disposedValue = false;
        public bool HasSourceInfo = false;

        private static object _syncRoot = new object();

        internal DiaUtil(string pdbName)
        {
            _IDiaDataSource = new DiaSource();
            _IDiaDataSource.loadDataFromPdb(pdbName);
            _IDiaDataSource.openSession(out _IDiaSession);

            this._IDiaSession.findChildrenEx(this._IDiaSession.globalScope,
                SymTagEnum.SymTagFunction,
                null,
                0,
                out IDiaEnumSymbols matchedSyms);

            foreach(IDiaSymbol sym in matchedSyms)
            {
                this._IDiaSession.findLinesByRVA(sym.relativeVirtualAddress,
                    (uint) sym.length,
                    out IDiaEnumLineNumbers enumLineNums);

                Marshal.ReleaseComObject(sym);

                if (enumLineNums.count > 0)
                {
                    // this PDB has at least 1 function with source info, so end the search
                    HasSourceInfo = true;

                    break;
                }

                Marshal.ReleaseComObject(enumLineNums);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Marshal.FinalReleaseComObject(_IDiaSession);
                    Marshal.FinalReleaseComObject(_IDiaDataSource);
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// This function builds up the PDB map, by searching for matched PDBs (based on name) and constructing the DIA session for each
        /// It is VERY important to specify the PDB search paths correctly, because there is no 'signature' information available 
        /// to match the PDB in any automatic way.
        /// </summary>
        /// <param name="_diautils">Internal dictionary object to hold cached instances of the PDB map</param>
        /// <param name="rootPaths">Symbol search paths</param>
        /// <param name="recurse">Boolean, whether to recursively search for matching PDBs</param>
        /// <param name="moduleNames">List of modules to search PDBs for</param>
        /// <param name="cachePDB">Cache a copy of PDBs into %TEMP%\SymCache</param>
        internal static bool LocateandLoadPDBs(Dictionary<string, DiaUtil> _diautils,
            string rootPaths,
            bool recurse,
            List<string> moduleNames,
            bool cachePDB)
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
                                    if (!foundFiles.Any())
                                    {
                                        // repeat the search but with a more relaxed filter. this (somewhat hacky) consideration is required
                                        // for modules like vcruntime140.dll where the PDB name is actually vcruntime140.amd64.pdb
                                        foundFiles = Directory.EnumerateFiles(currPath, currentModule + ".*.pdb", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                                    }

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
        /// Internal helper function to return the symbolized frame text (not including source info)
        /// </summary>
        /// <param name="moduleName">Module name for the current frame</param>
        /// <param name="mysym">DIA symbol object being "symbolized"</param>
        /// <param name="useUndecorateLogic">Whether to "undecorate" the symbol name - required for public PDBs</param>
        /// <param name="includeOffset">Boolean, whether to include the offset within the function in the output</param>
        /// <param name="displacement">Integer offset into the function</param>
        /// <returns></returns>
        internal static string GetSymbolizedFrame(string moduleName,
            IDiaSymbol mysym,
            bool useUndecorateLogic,
            bool includeOffset,
            int displacement
            )
        {
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

            return string.Format(CultureInfo.CurrentCulture,
                "{0}!{1}{2}",
                moduleName,
                funcname2,
                offsetStr);
        }

        /// <summary>
        /// Internal helper function to obtain source information for given symbol
        /// </summary>
        /// <param name="enumLineNums">Input object enumerating line number(s)</param>
        /// <param name="pdbHasSourceInfo">Boolean, whether the PDB for this module has source info</param>
        /// <returns></returns>
        internal static string GetSourceInfo(IDiaEnumLineNumbers enumLineNums,
            bool pdbHasSourceInfo)
        {
            var sbOutput = new StringBuilder();

            // only if we found line number information should we append to output 
            if (enumLineNums.count > 0)
            {
                for (uint tmpOrdinal = 0; tmpOrdinal < enumLineNums.count; tmpOrdinal++)
                {
                    if (tmpOrdinal > 0)
                    {
                        sbOutput.Append(" -- WARN: multiple matches -- ");
                    }

                    sbOutput.Append(string.Format(CultureInfo.CurrentCulture,
                        "({0}:{1})",
                        enumLineNums.Item(tmpOrdinal).sourceFile.fileName,
                        enumLineNums.Item(tmpOrdinal).lineNumber));

                    Marshal.FinalReleaseComObject(enumLineNums.Item(tmpOrdinal));
                }
            }
            else
            {
                if (pdbHasSourceInfo)
                {
                    sbOutput.Append("-- WARN: unable to find source info --");
                }
            }

            Marshal.FinalReleaseComObject(enumLineNums);

            return sbOutput.ToString();
        }

        /// <summary>
        /// Internal helper function to find any inline frames at a given RVA
        /// </summary>
        /// <param name="moduleName">Module name for current frame</param>
        /// <param name="useUndecorateLogic">Whether to "undecorate" the symbol (public symbols only)</param>
        /// <param name="includeOffset">Boolean, whether to include function offset in output</param>
        /// <param name="includeSourceInfo">Boolean, whether to include source info in output</param>
        /// <param name="rva">Relative Virtual Address at which potential inline frames need to be looked up</param>
        /// <param name="parentSym">The parent DIA symbol object to use for inline frame lookup</param>
        /// <param name="pdbHasSourceInfo">Boolean, whether the PDB for this module has source info</param>
        /// <returns></returns>
        internal static string ProcessInlineFrames(
            string moduleName,
            bool useUndecorateLogic,
            bool includeOffset,
            bool includeSourceInfo,
            uint rva,
            IDiaSymbol parentSym,
            bool pdbHasSourceInfo
            )
        {
            var sbInline = new StringBuilder();

            try
            {
                var inlineRVA = rva - 1;

                parentSym.findInlineFramesByRVA(
                    inlineRVA,
                    out IDiaEnumSymbols enumInlinees);

                foreach (IDiaSymbol inlineFrame in enumInlinees)
                {
                    var inlineeOffset = (int)(rva - inlineFrame.relativeVirtualAddress);

                    sbInline.Append("(Inline Function) ");
                    sbInline.Append(DiaUtil.GetSymbolizedFrame(
                        moduleName,
                        inlineFrame,
                        useUndecorateLogic,
                        includeOffset,
                        inlineeOffset
                        ));

                    if (includeSourceInfo)
                    {
                        inlineFrame.findInlineeLinesByRVA(
                        inlineRVA,
                        0,
                        out IDiaEnumLineNumbers enumLineNums);

                        sbInline.Append("\t");
                        sbInline.Append(DiaUtil.GetSourceInfo(enumLineNums,
                            pdbHasSourceInfo
                            ));
                    }

                    Marshal.ReleaseComObject(inlineFrame);

                    sbInline.AppendLine();
                }

                Marshal.ReleaseComObject(enumInlinees);
            }
            catch (COMException)
            {
                sbInline.AppendLine(" -- WARN: Unable to process inline frames");
            }

            return sbInline.ToString();
        }
    }
}
