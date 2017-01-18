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

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private StackResolver _resolver = new StackResolver();
        private string _baseAddressesString = null;

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> dllPaths = null;
            if (!string.IsNullOrEmpty(binaryPaths.Text))
            {
                dllPaths = binaryPaths.Text.Split(';').ToList();
            }

            this._resolver.ProcessBaseAddresses(this._baseAddressesString);

            finalOutput.Text = this._resolver.ResolveCallstacks(callStackInput.Text,
                pdbPaths.Text,
                pdbRecurse.Checked,
                dllPaths,
                DLLrecurse.Checked,
                FramesOnSingleLine.Checked
                );
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                callStackInput.SelectAll();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                finalOutput.SelectAll();
            }
        }        
    }
}
