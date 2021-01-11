using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    public static class ModuleInfoHelper
    {
        private static Regex rgxPDBName = new Regex(@"^(?<module>.+)(\.pdb)$", RegexOptions.IgnoreCase);
        private static Regex rgxFileName = new Regex(@"^(?<module>.+)\.(dll|exe)$", RegexOptions.IgnoreCase);

        public static Dictionary<string, Symbol> ParseModuleInfo(string input)
        {
            var retval = new Dictionary<string, Symbol>();

            // split into multiple lines
            var lines = input.Split('\n');

            foreach(var line in lines)
            {
                Guid pdbGuid = Guid.Empty;
                int pdbAge = int.MinValue;
                string moduleName = null;
                string pdbName = null;

                // foreach line, split into comma-delimited fields
                var fields = line.Split(',');

                foreach (var rawfield in fields)
                {
                    var field = rawfield.TrimEnd('"').TrimStart('"');
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
                            pdbName = matchPDBName.Groups["module"].Value;
                        }
                    }
                }

                // assumption is that last field is pdbAge - TODO parameterize
                int.TryParse(fields[fields.Length - 1], out pdbAge);

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
