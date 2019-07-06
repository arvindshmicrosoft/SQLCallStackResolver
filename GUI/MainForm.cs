//------------------------------------------------------------------------------
//<copyright company="Microsoft">
//    The MIT License (MIT)
//    
//    Copyright (c) 2017 Microsoft
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
//</copyright>
//------------------------------------------------------------------------------
namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Text;
    using System.IO;
    using System.Net;
    using System.Globalization;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private StackResolver _resolver = new StackResolver();
        private string _baseAddressesString = null;
        internal static string SqlBuildInfoFileName = @"sqlbuildinfo.json";

        private void ResolveCallstacks_Click(object sender, EventArgs e)
        {
            List<string> dllPaths = null;
            if (!string.IsNullOrEmpty(binaryPaths.Text))
            {
                dllPaths = binaryPaths.Text.Split(';').ToList();
            }

            var res = this._resolver.ProcessBaseAddresses(this._baseAddressesString);
            if (!res)
            {
                MessageBox.Show(
                            this,
                            "Cannot interpret the module base address information. Make sure you just have the output of the following query (no column headers, no other columns) copied from SSMS using the Grid Results\r\n\r\nselect name, base_address from sys.dm_os_loaded_modules where name not like '%.rll'",
                            "Unable to load base address information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                return;
            }

            this.ShowStatus("Resolving callstacks; please wait. This may take a while!");

            finalOutput.Text = this._resolver.ResolveCallstacks(callStackInput.Text,
                pdbPaths.Text,
                pdbRecurse.Checked,
                dllPaths,
                DLLrecurse.Checked,
                FramesOnSingleLine.Checked,
                IncludeLineNumbers.Checked,
                RelookupSource.Checked,
                includeOffsets.Checked
                );

            this.ShowStatus(string.Empty);
        }

        private void EnterBaseAddresses_Click(object sender, EventArgs e)
        {
            var baseAddressForm = new MultilineInput(this._baseAddressesString, true);
            DialogResult res = baseAddressForm.ShowDialog(this);

            if (res == DialogResult.OK)
            {
                this._baseAddressesString = baseAddressForm.baseaddressesstring;
            }
            else
            {
                return;
            }
        }

        private void CallStackInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                callStackInput.SelectAll();
            }
        }

        private void FinalOutput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                finalOutput.SelectAll();
            }
        }

        private void GetPDBDnldScript_Click(object sender, EventArgs e)
        {
            this.ShowStatus("Getting PDB download script... please wait. This may take a while!");

            var finalCmds = this._resolver.ObtainPDBDownloadCommandsfromDLL(binaryPaths.Text,
                DLLrecurse.Checked,
                false);

            if (string.IsNullOrEmpty(finalCmds))
            {
                return;
            }

            this.ShowStatus(string.Empty);

            var outputCmds = new MultilineInput(finalCmds, false);
            outputCmds.ShowDialog(this);
        }

        private void ShowStatus(string txt)
        {
            this.statusLabel.Text = txt;
            Application.DoEvents();
        }

        private void LoadXELButton_Click(object sender, EventArgs e)
        {
            genericOpenFileDlg.Multiselect = true;
            genericOpenFileDlg.CheckPathExists = true;
            genericOpenFileDlg.CheckFileExists = true;
            genericOpenFileDlg.FileName = String.Empty;
            genericOpenFileDlg.Filter = "XEL files (*.xel)|*.xel|All files (*.*)|*.*";
            genericOpenFileDlg.Title = "Select XEL file";

            var res = genericOpenFileDlg.ShowDialog(this);

            if (res != DialogResult.Cancel)
            {
                this.ShowStatus("Loading from XEL files; please wait. This may take a while!");

                callStackInput.Text = this._resolver.ExtractFromXEL(genericOpenFileDlg.FileNames, BucketizeXEL.Checked);

                this.ShowStatus(string.Empty);
            }
        }

        private void CallStackInput_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length != 0)
                {
                    var allFilesContent = new StringBuilder();

                    // sample the first file selected and if it is XEL assume all the files are XEL
                    // if there is any other format in between, it will be rejected by the ExtractFromXEL code
                    if (Path.GetExtension(files[0]).ToLower() == ".xel")
                    {
                        this.ShowStatus("XEL file was dragged; please wait while we extract events from the file");

                        allFilesContent.AppendLine(this._resolver.ExtractFromXEL(files, BucketizeXEL.Checked));

                        this.ShowStatus(string.Empty);
                    }
                    else
                    {
                        // handle the files as text input
                        foreach (var currFile in files)
                        {
                            allFilesContent.AppendLine(File.ReadAllText(currFile));
                        }
                    }

                    callStackInput.Text = allFilesContent.ToString();
                }
            }
        }

        private void CallStackInput_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void PDBPathPicker_Click(object sender, EventArgs e)
        {
            genericOpenFileDlg.Multiselect = false;
            genericOpenFileDlg.CheckPathExists = false;
            genericOpenFileDlg.CheckFileExists = false;
            genericOpenFileDlg.FileName = "select folder only";
            genericOpenFileDlg.Filter = "All files (*.*)|*.*";
            genericOpenFileDlg.Title = "Select FOLDER path to your PDBs";

            var res = genericOpenFileDlg.ShowDialog(this);

            if (res != DialogResult.Cancel)
            {
                pdbPaths.AppendText((pdbPaths.TextLength == 0 ? string.Empty : ";") + Path.GetDirectoryName(genericOpenFileDlg.FileName));
            }
        }

        private void BinaryPathPicker_Click(object sender, EventArgs e)
        {
            genericOpenFileDlg.Multiselect = false;
            genericOpenFileDlg.CheckPathExists = false;
            genericOpenFileDlg.CheckFileExists = false;
            genericOpenFileDlg.FileName = "select folder only";
            genericOpenFileDlg.Filter = "All files (*.*)|*.*";
            genericOpenFileDlg.Title = "Select FOLDER path to the SQL binaries";

            var res = genericOpenFileDlg.ShowDialog(this);

            if (res != DialogResult.Cancel)
            {
                binaryPaths.AppendText((binaryPaths.TextLength == 0 ? string.Empty : ";") + Path.GetDirectoryName(genericOpenFileDlg.FileName));
            }
        }

        private void SelectSQLPDB_Click(object sender, EventArgs e)
        {
            var sqlbuildsForm = new SQLBuildsForm
            {
                pathToPDBs = System.Configuration.ConfigurationManager.AppSettings["PDBDownloadFolder"]
            };

            DialogResult res = sqlbuildsForm.ShowDialog(this);

            this.pdbPaths.AppendText(sqlbuildsForm.pathToPDBs);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // get the timestamp of the local sqlbuildinfo.json file
            var localBuildInfoFile = new FileInfo(SqlBuildInfoFileName);

            DateTime lastUpdDateTime = DateTime.MinValue;

            var sqlBuildInfoUpdateURLs = System.Configuration.ConfigurationManager.AppSettings["SQLBuildInfoUpdateURLs"].Split(';');
            var sqlBuildInfoURLs = System.Configuration.ConfigurationManager.AppSettings["SQLBuildInfoURLs"].Split(';');

            // get the timestamp of the first valid file within SQLBuildInfoURLs
            foreach (var url in sqlBuildInfoUpdateURLs)
            {
                HttpWebResponse httpResp = null;
                var httpReq = (HttpWebRequest) WebRequest.Create(url);

                try
                {
                    httpResp = (HttpWebResponse)httpReq.GetResponse();
                }
                catch(WebException)
                {
                }

                if (httpResp != null)
                {
                    if (httpResp.StatusCode == HttpStatusCode.OK)
                    {
                        using (var strm = new StreamReader(httpResp.GetResponseStream()))
                        {
                            string lastUpd = strm.ReadToEnd();
                            lastUpdDateTime = DateTime.ParseExact(lastUpd, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));
                        }

                        if (lastUpdDateTime > localBuildInfoFile.LastWriteTimeUtc)
                        {
                            // if the server timestamp > local timestamp, prompt to download
                            var res = MessageBox.Show(this,
                                "The SQLBuildInfo.json file was updated recently on GitHub. Do you wish to update your copy with the newer version?",
                                "SQL Build info updated",
                                MessageBoxButtons.YesNo);

                            if (DialogResult.Yes == res)
                            {
                                foreach (var jsonURL in sqlBuildInfoURLs)
                                {
                                    httpResp = null;
                                    httpReq = (HttpWebRequest)WebRequest.Create(jsonURL);

                                    try
                                    {
                                        httpResp = (HttpWebResponse)httpReq.GetResponse();
                                    }
                                    catch (WebException)
                                    {
                                    }

                                    if (httpResp != null)
                                    {
                                        if (httpResp.StatusCode == HttpStatusCode.OK)
                                        {
                                            try
                                            {
                                                using (var strm = new StreamReader(httpResp.GetResponseStream()))
                                                {
                                                    var jsonContent = strm.ReadToEnd();

                                                    using (var writer = new StreamWriter(SqlBuildInfoFileName))
                                                    {
                                                        writer.Write(jsonContent);
                                                        writer.Flush();
                                                        writer.Close();
                                                    }
                                                }
                                            }
                                            catch(WebException)
                                            {
                                                MessageBox.Show(this,
                                                    "Could not download SQL Build Info file due to HTTP errors.",
                                                    "Error",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}
