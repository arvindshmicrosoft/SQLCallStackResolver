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
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public partial class MultilineInput : Form
    {
        public MultilineInput(string initialtext, bool showFilepicker)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(initialtext))
            {
                this.InputAddresses.Text = initialtext;
            }

            if (showFilepicker)
            {
                loadFromFile.Visible = true;
            }
            else
            {
                loadFromFile.Visible = false;
            }
        }

        public string baseaddressesstring
        {
            get
            {
                return this.InputAddresses.Text;
            }
        }

        private void InputAddresses_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                InputAddresses.SelectAll();
            }
        }

        private void InputAddresses_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length != 0)
                {
                    var allFilesContent = new StringBuilder();
                    foreach (var currFile in files)
                    {
                        allFilesContent.AppendLine(File.ReadAllText(currFile));
                    }

                    InputAddresses.Text = allFilesContent.ToString();
                }
            }
        }

        private void InputAddresses_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void loadFromFile_Click(object sender, System.EventArgs e)
        {
            fileDlg.Multiselect = false;
            fileDlg.CheckPathExists = true;
            fileDlg.CheckFileExists = true;
            fileDlg.FileName = string.Empty;
            fileDlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDlg.Title = "Select file";

            var res = fileDlg.ShowDialog(this);

            if (res != DialogResult.Cancel)
            {
                InputAddresses.Text = File.ReadAllText(fileDlg.FileName);
            }
        }
    }
}
