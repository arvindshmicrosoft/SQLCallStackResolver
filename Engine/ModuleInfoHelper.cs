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
    using System.Diagnostics.Contracts;
    using System.Text.RegularExpressions;

    public static class ModuleInfoHelper
    {
        private static Regex rgxPDBName = new Regex(@"^(?<pdb>.+)(\.pdb)$", RegexOptions.IgnoreCase);
        private static Regex rgxFileName = new Regex(@"^(?<module>.+)\.(dll|exe)$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Given a set of rows each containing several comma-separated fields, return a set of resolved Symbol
        /// objects each of which have PDB GUID and age details.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Dictionary<string, Symbol> ParseModuleInfo(string input)
        {
            var retval = new Dictionary<string, Symbol>();

            Contract.Requires(!string.IsNullOrEmpty(input));

            // split into multiple lines
            var lines = input.Split('\n');

            foreach(var line in lines)
            {
                Guid pdbGuid = Guid.Empty;
                string moduleName = null;
                string pdbName = null;

                // foreach line, split into comma-delimited fields
                var fields = line.Split(',');

                foreach (var rawfield in fields)
                {
                    var field = rawfield.Trim().TrimEnd('"').TrimStart('"');
                    Guid tmpGuid = Guid.Empty;
                    // for each field, attempt using regexes to detect file name and GUIDs
                    if (Guid.TryParse(field, out tmpGuid))
                    {
                        pdbGuid = tmpGuid;
                    }

                    if (string.IsNullOrEmpty(moduleName))
                    {
                        var matchFilename = rgxFileName.Match(field);
                        if (matchFilename.Success)
                        {
                            moduleName = matchFilename.Groups["module"].Value;
                        }
                    }

                    if (string.IsNullOrEmpty(pdbName))
                    {
                        var matchPDBName = rgxPDBName.Match(field);
                        if (matchPDBName.Success)
                        {
                            pdbName = matchPDBName.Groups["pdb"].Value;
                        }
                    }
                }

                int pdbAge = int.MinValue;
                // assumption is that last field is pdbAge - TODO parameterize
                _ = int.TryParse(fields[fields.Length - 1], out pdbAge);

                if (string.IsNullOrEmpty(pdbName))
                {
                    // fall back to module name as PDB name
                    pdbName = moduleName;
                }

                // check if we have all 3 details
                if (!string.IsNullOrEmpty(pdbName)
                    && pdbAge != int.MinValue
                    && pdbGuid != Guid.Empty)
                {
                    retval.Add(moduleName, new Symbol()
                    {
                        PDBName = pdbName + ".pdb",
                        PDBAge = pdbAge,
                        PDBGuid = pdbGuid.ToString("N")
                    });
                }
            }

            return retval;
        }
    }
}
