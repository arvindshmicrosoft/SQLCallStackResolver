using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    public class SQLBuildInfo
    {
        public string ProductMajorVersion;

        public string ProductLevel;

        public string Label;

        public string BuildNumber;

        public string KBInfo;

        public List<string> PDBUrls;

        public string MachineType;

        public override string ToString()
        {
            return string.Format($"{Label} - {BuildNumber} - {MachineType} ({KBInfo})");
        }

        public static Dictionary<string, SQLBuildInfo> GetSqlBuildInfo(string jsonFile)
        {
            var allBuilds = new Dictionary<string, SQLBuildInfo>();

            using (var rdr = new StreamReader(jsonFile))
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

                        if (!allBuilds.ContainsKey(currBuildInfo.ToString()))
                        {
                            allBuilds.Add(currBuildInfo.ToString(), currBuildInfo);
                        }
                        else
                        {
                            allBuilds[currBuildInfo.ToString()] = currBuildInfo;
                        }
                    }
                }

                rdr.Close();
            }

            return allBuilds;
        }

        public static void SaveSqlBuildInfo(List<SQLBuildInfo> allBuilds, string jsonFile)
        {
            using (var wrtr = new StreamWriter(jsonFile))
            {
                foreach (var bld in allBuilds)
                {
                    wrtr.WriteLine(JsonConvert.SerializeObject(bld));
                }

                //using (var jsonWrtr = new JsonTextWriter(wrtr))
                //{
                //    var serializer = new JsonSerializer();

                //    serializer.Serialize(jsonWrtr, allBuilds);
                //}
            }
        }
    }
}
