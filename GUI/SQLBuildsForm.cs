using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    public partial class SQLBuildsForm : Form
    {
        const string MagicTagForBuilds = "Build";
        public string pathToPDBs = string.Empty;
        private bool activeDownload = false;

        public SQLBuildsForm()
        {
            InitializeComponent();
        }

        private void Treeview_Load(object sender, EventArgs e)
        {
            var allBuilds = SQLBuildInfo.GetSqlBuildInfo(@"sqlbuildinfo.json");

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

                    treeView1.Nodes[ver].Nodes[pl].Nodes.AddRange(blds.Select(bld => new TreeNode(bld.ToString()) { Name = bld.ToString(), Tag = bld }).ToArray());
                }
            }
        }

        private void DownloadPDBs(object sender, EventArgs e)
        {
            var bld = treeView1.SelectedNode.Tag as SQLBuildInfo;

            if (bld != null)
            {
                MessageBox.Show(treeView1.SelectedNode.Name + string.Join(",", bld.PDBUrls));

                if (bld.PDBUrls.Count > 0)
                {
                    dnldButton.Enabled = false;

                    Directory.CreateDirectory($@"{pathToPDBs}\{bld.BuildNumber}");
                    using (var client = new WebClient())
                    {
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

                        foreach (var url in bld.PDBUrls)
                        //var url = bld.PDBUrls[0];
                        {
                            var uri = new Uri(url);
                            string filename = Path.GetFileName(uri.LocalPath);

                            downloadStatus.Text = filename;
                            activeDownload = true;

                            client.DownloadFileAsync(new Uri(url), $@"{pathToPDBs}\{bld.BuildNumber}\{filename}");

                            while (activeDownload)
                            {
                                Thread.Sleep(300);
                                Application.DoEvents();
                            }
                        }

                        downloadStatus.Text = "Done.";
                        dnldButton.Enabled = true;
                    }
                }
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            downloadProgress.ProgressBar.Value = (int)percentage;
            statusStrip1.Refresh();

            //var mi = new MethodInvoker(() => downloadProgress.ProgressBar.Value = (int)percentage);
            //downloadProgress.ProgressBar.Invoke(mi);

            //downloadProgress.Value = int.Parse(percentage).ToString());
            //Application.DoEvents();
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadProgress.ProgressBar.Value = 0;
            statusStrip1.Refresh();
            activeDownload = false;
        }
    }
}
