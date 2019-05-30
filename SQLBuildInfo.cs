using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    internal class SQLBuildInfo
    {
        public string ProductMajorVersion;

        public string ProductLevel;

        public string Label;

        public string BuildNumber;

        public string KBInfo;

        public List<string> PDBUrls;

        public override string ToString()
        {
            return string.Format($"{Label} - {BuildNumber} ({KBInfo})");
        }
    }
}
