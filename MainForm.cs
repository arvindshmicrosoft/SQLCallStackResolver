﻿//------------------------------------------------------------------------------
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

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private StackResolver _resolver = new StackResolver();
        private string _baseAddressesString = null;

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

            finalOutput.Text = this._resolver.ResolveCallstacks(callStackInput.Text,
                pdbPaths.Text,
                pdbRecurse.Checked,
                dllPaths,
                DLLrecurse.Checked,
                FramesOnSingleLine.Checked,
                IncludeLineNumbers.Checked,
                RelookupSource.Checked,
                includeOffsets.Checked,
                cachePDB.Checked
                );
        }

        private void EnterBaseAddresses_Click(object sender, EventArgs e)
        {
            var baseAddressForm = new MultilineInput(this._baseAddressesString);
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
            var finalCmds = this._resolver.ObtainPDBDownloadCommandsfromDLL(binaryPaths.Text, DLLrecurse.Checked);

            if (string.IsNullOrEmpty(finalCmds))
            {
                return;
            }

            var outputCmds = new MultilineInput(finalCmds);
            outputCmds.ShowDialog(this);
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
                callStackInput.Text = this._resolver.ExtractFromXEL(genericOpenFileDlg.FileNames, BucketizeXEL.Checked);
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
                        allFilesContent.AppendLine(this._resolver.ExtractFromXEL(files, BucketizeXEL.Checked));
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
    }
}
