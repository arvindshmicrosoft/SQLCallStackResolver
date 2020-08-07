﻿//------------------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    public partial class SQLBuildsForm : Form
    {
        public string pathToPDBs = string.Empty;
        public string lastDownloadedSymFolder = string.Empty;
        private bool activeDownload = false;

        public SQLBuildsForm()
        {
            InitializeComponent();
        }

        private void Treeview_Load(object sender, EventArgs e)
        {
            var allBuilds = SQLBuildInfo.GetSqlBuildInfo(MainForm.SqlBuildInfoFileName);

            // top-level is major versions
            var sqlMajorVersions = allBuilds.Values.Select(b => b.ProductMajorVersion).
                OrderByDescending(b => b).Distinct();

            treeviewSyms.Nodes.AddRange(sqlMajorVersions.Select(b => new TreeNode(b) { Name = b }).ToArray());

            // within each major version, get product levels (RTM, SP1 etc.)
            foreach (var ver in sqlMajorVersions)
            {
                var prodLevels = allBuilds.Values.Where(b => b.ProductMajorVersion == ver).
                    Select(b => b.ProductLevel).OrderByDescending(b => b).Distinct();

                treeviewSyms.Nodes[ver].Nodes.AddRange(prodLevels.Select(pl => new TreeNode(pl) { Name = pl }).ToArray());

                // finally within each product level get the individual builds
                foreach (var pl in prodLevels)
                {
                    var blds = allBuilds.Values.Where(b => b.ProductMajorVersion == ver && b.ProductLevel == pl
                                      && b.SymbolDetails.Count > 0).Distinct().
                                      OrderByDescending(b => b.BuildNumber);

                    treeviewSyms.Nodes[ver].Nodes[pl].Nodes.AddRange(blds.Select(bld => new TreeNode(bld.ToString()) { Name = bld.ToString(), Tag = bld }).ToArray());
                }
            }
        }

        private void DownloadPDBs(object sender, EventArgs e)
        {
            if (treeviewSyms.SelectedNode is null)
            {
                return;
            }

            if (treeviewSyms.SelectedNode.Tag is SQLBuildInfo bld)
            {
                if (bld.SymbolDetails.Count > 0)
                {
                    dnldButton.Enabled = false;

                    lastDownloadedSymFolder = $@"{pathToPDBs}\{bld.BuildNumber}";
                    Directory.CreateDirectory(lastDownloadedSymFolder);
                    using (var client = new WebClient())
                    {
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);

                        foreach (var sym in bld.SymbolDetails)
                        {
                            var url = sym.DownloadURL;

                            if (!string.IsNullOrEmpty(url))
                            {
                                var uri = new Uri(url);
                                string filename = Path.GetFileName(uri.LocalPath);

                                if (File.Exists($@"{lastDownloadedSymFolder}\{filename}"))
                                {
                                    continue;
                                }

                                downloadStatus.Text = filename;
                                activeDownload = true;

                                client.DownloadFileAsync(new Uri(url), $@"{lastDownloadedSymFolder}\{filename}");

                                while (activeDownload)
                                {
                                    Thread.Sleep(300);
                                    Application.DoEvents();
                                }
                            }
                        }

                        downloadStatus.Text = "Done.";
                        dnldButton.Enabled = true;
                    }
                }
            }
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = e.BytesReceived;
            double totalBytes = e.TotalBytesToReceive;
            double percentage = bytesIn / totalBytes * 100;
            downloadProgress.ProgressBar.Value = (int)percentage;
            statusStrip1.Refresh();
        }

        void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadProgress.ProgressBar.Value = 0;
            statusStrip1.Refresh();
            activeDownload = false;
        }

        private void CheckPDBAvail_Click(object sender, EventArgs e)
        {
            if (treeviewSyms.SelectedNode is null)
            {
                return;
            }

            if (treeviewSyms.SelectedNode.Tag is SQLBuildInfo bld)
            {
                if (bld.SymbolDetails.Count > 0)
                {
                    List<string> failedUrls = new List<string>();

                    foreach (var sym in bld.SymbolDetails)
                    {
                        var url = sym.DownloadURL;

                        downloadStatus.Text = url;

                        if (!Symbol.IsURLValid(new Uri(url)))
                        {
                            failedUrls.Add(url);
                        }
                    }

                    if (failedUrls.Count > 0)
                    {
                        MessageBox.Show(string.Join(",", failedUrls));
                    }
                    else
                    {
                        downloadStatus.Text = "All PDBs for this build are available!";
                    }
                }
            }
        }

        private void findNext_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeviewSyms.Nodes)
            {
                if (CheckIfAnyNodesMatch(node))
                {
                    return;
                }
            }

            downloadStatus.Text = "No matches found.";

            treeviewSyms.SelectedNode = null;
            treeviewSyms.Refresh();
            Application.DoEvents();
        }

        private bool CheckIfAnyNodesMatch(TreeNode node)
        {
            if (node.Tag is SQLBuildInfo bld)
            {
                if (bld.ToString().ToLower(CultureInfo.CurrentCulture).Contains(searchText.Text.ToLower(CultureInfo.CurrentCulture)))
                {
                    treeviewSyms.SelectedNode = node;
                    treeviewSyms.Select();
                    treeviewSyms.Refresh();
                    Application.DoEvents();
                    return true;
                }
            }

            foreach (TreeNode child in node.Nodes)
            {
                if (CheckIfAnyNodesMatch(child))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
