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
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class SQLBuildInfo
    {
        public string ProductMajorVersion = "<<ProductMajorVersion>>";

        public string ProductLevel = "<<ProductLevel>>";

        public string Label = "<<BuildName>>";

        public string BuildNumber = "<<BuildNumber>>";

        public string KBInfo = "<<KBArticle>>";

        public List<Symbol> SymbolDetails;

        public string MachineType = "<<x64|x86>>";

        public override string ToString()
        {
            return string.Format($"{ProductMajorVersion} {ProductLevel} {Label} - {BuildNumber} - {MachineType} ({KBInfo})");
        }

        public static SortedDictionary<string, SQLBuildInfo> GetSqlBuildInfo(string jsonFile)
        {
            var allBuilds = new SortedDictionary<string, SQLBuildInfo>();

            using (var fs = new FileStream(jsonFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (var rdr = new StreamReader(fs))
                {
                    using (var jsonRdr = new JsonTextReader(rdr))
                    {
                        jsonRdr.SupportMultipleContent = true;

                        var serializer = new JsonSerializer();

                        while (true)
                        {
                            if (!jsonRdr.Read())
                            {
                                break;
                            }

                            var currBuildInfo = serializer.Deserialize<SQLBuildInfo>(jsonRdr);

                            currBuildInfo.BuildNumber = currBuildInfo.BuildNumber.Trim();
                            currBuildInfo.KBInfo = currBuildInfo.KBInfo.Trim();
                            currBuildInfo.Label = currBuildInfo.Label.Trim();
                            currBuildInfo.ProductLevel = currBuildInfo.ProductLevel.Trim();
                            currBuildInfo.ProductMajorVersion = currBuildInfo.ProductMajorVersion.Trim();

                            if (!allBuilds.ContainsKey(currBuildInfo.ToString()))
                            {
                                allBuilds.Add(currBuildInfo.ToString(), currBuildInfo);
                            }
                            else
                            {
                                allBuilds[currBuildInfo.ToString()] = currBuildInfo;
                            }
                        }

                        jsonRdr.Close();
                    }

                    rdr.Close();
                }
            }

            return new SortedDictionary<string, SQLBuildInfo>(allBuilds);
        }

        public static void SaveSqlBuildInfo(List<SQLBuildInfo> allBuilds, string jsonFile)
        {
            using (var fs = new FileStream(jsonFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                using (var wrtr = new StreamWriter(fs))
                {
                    foreach (var bld in allBuilds)
                    {
                        wrtr.WriteLine(JsonConvert.SerializeObject(bld));
                    }

                    wrtr.Flush();
                    wrtr.Close();
                }
            }
        }

        public static string GetDownloadScriptPowerShell(SQLBuildInfo bld, bool includeMarkdown)
        {
            var symcmds = new StringBuilder();

            if (null != bld.SymbolDetails && bld.SymbolDetails.Where(s => s.DownloadVerified).Count() > 0)
            {
                if (includeMarkdown)
                {
                    symcmds.AppendLine($"# {bld}");
                    symcmds.AppendLine("``` powershell");
                }
                symcmds.AppendLine($"# {bld}");
                symcmds.AppendLine($"$outputFolder = 'c:\\sqlsyms\\{bld.BuildNumber}\\{bld.MachineType}' # <<change this output folder if needed>>'");
                symcmds.AppendLine($"mkdir -f $outputFolder");
                foreach (var sym in bld.SymbolDetails)
                {
                    if (!sym.DownloadVerified) continue;

                    symcmds.AppendLine($"Invoke-WebRequest -uri '{sym.DownloadURL}' -OutFile \"$outputFolder\\{sym.PDBName}.pdb\" # File version {sym.FileVersion}");
                }

                if (includeMarkdown) symcmds.AppendLine("```");
                symcmds.AppendLine();
            }

            return symcmds.ToString();
        }
    }
}
