using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    public partial class SQLBuildsForm : Form
    {
        const string MagicTagForBuilds = "Build";
        public string pathToPDBs = string.Empty;

        public SQLBuildsForm()
        {
            InitializeComponent();
        }

        private void Treeview_Load(object sender, EventArgs e)
        {
            var allBuilds = SQLBuildInfo.GetSqlBuildInfo(@"C:\workarea\SQLCallStackResolver\sqlbuildinfo.json");

            // top-level is major versions
            var sqlMajorVersions = (from SQLBuildInfo b in allBuilds.Values
                                   select b.ProductMajorVersion).Distinct();

            treeView1.Nodes.AddRange(sqlMajorVersions.Select(b => new TreeNode(b) { Name = b } ).ToArray());

            // within each major version, get product levels (RTM, SP1 etc.)
            foreach(var ver in sqlMajorVersions)
            {
                var prodLevels = (from SQLBuildInfo b in allBuilds.Values
                                  where b.ProductMajorVersion == ver
                                  select b.ProductLevel).Distinct();

                treeView1.Nodes[ver].Nodes.AddRange(prodLevels.Select(pl => new TreeNode(pl) { Name = pl } ).ToArray());

                // finally within each product level get the individual builds
                foreach (var pl in prodLevels)
                {
                    var blds = (from SQLBuildInfo b in allBuilds.Values
                                      where b.ProductMajorVersion == ver && b.ProductLevel == pl
                                      && b.PDBUrls.Count > 0
                                      select b).Distinct().OrderByDescending(b => b.BuildNumber);

                    treeView1.Nodes[ver].Nodes[pl].Nodes.AddRange(blds.Select(bld => new TreeNode(bld.ToString()) { Name = bld.ToString(), Tag = MagicTagForBuilds }).ToArray());
                }
            }
        }

        private void DownloadPDBs(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag as string == MagicTagForBuilds)
            {
                MessageBox.Show(treeView1.SelectedNode.Name);
            }
        }
    }
}
