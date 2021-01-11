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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    class DLLOrdinalHelper
    {
        /// <summary>
        /// This holds the mapping of the various DLL exports for a module and the address (offset) for each such export
        /// Only populated if the user provides the 'image path' to the DLLs
        /// </summary>
        Dictionary<string, Dictionary<int, ExportedSymbol>> _DLLOrdinalMap;

        internal void Initialize()
        {
            _DLLOrdinalMap = new Dictionary<string, Dictionary<int, ExportedSymbol>>();
        }

        /// <summary>
        /// This function loads DLLs from a specified path, so that we can then build the DLL export's ordinal / address map
        /// </summary>
        /// <param name="callstack"></param>
        /// <param name="recurse"></param>
        /// <param name="dllPaths"></param>
        /// <returns></returns>
        internal string LoadDllsIfApplicable(string callstack, bool recurse, List<string> dllPaths)
        {
            if (dllPaths == null)
            {
                return callstack;
            }

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
                    var foundFiles = Directory.EnumerateFiles(currPath,
                        currmodule + ".dll",
                        recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                    lock (_DLLOrdinalMap)
                    {
                        if (!_DLLOrdinalMap.ContainsKey(currmodule) && foundFiles.Any())
                        {
                            _DLLOrdinalMap.Add(currmodule,
                                ExportedSymbol.GetExports(foundFiles.First()));

                            break;
                        }
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
        /// This delegate is invoked by the Replace function and is used to compute the effective offset from module load address
        /// based on ordinal start address and original offset
        /// </summary>
        /// <param name="mtch"></param>
        /// <returns></returns>
        private string ReplaceOrdinalWithRealOffset(Match mtch)
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
    }
}
