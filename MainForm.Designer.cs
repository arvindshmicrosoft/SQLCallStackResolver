//------------------------------------------------------------------------------
//<copyright company="Microsoft">
//
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
//</copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pdbPaths = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pdbRecurse = new System.Windows.Forms.CheckBox();
            this.callStackInput = new System.Windows.Forms.TextBox();
            this.finalOutput = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.DLLrecurse = new System.Windows.Forms.CheckBox();
            this.binaryPaths = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FramesOnSingleLine = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1680, 752);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(254, 83);
            this.button1.TabIndex = 0;
            this.button1.Text = "Resolve callstacks!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 755);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Path(s) to PDBs";
            // 
            // pdbPaths
            // 
            this.pdbPaths.Location = new System.Drawing.Point(198, 752);
            this.pdbPaths.Name = "pdbPaths";
            this.pdbPaths.Size = new System.Drawing.Size(870, 31);
            this.pdbPaths.TabIndex = 2;
            this.pdbPaths.Text = "C:\\Users\\arvindsh\\Desktop\\sp1_gdr_pdbs";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.CheckFileExists = false;
            this.openFileDialog1.CheckPathExists = false;
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pdbRecurse
            // 
            this.pdbRecurse.AutoSize = true;
            this.pdbRecurse.Checked = true;
            this.pdbRecurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pdbRecurse.Location = new System.Drawing.Point(1074, 756);
            this.pdbRecurse.Name = "pdbRecurse";
            this.pdbRecurse.Size = new System.Drawing.Size(217, 29);
            this.pdbRecurse.TabIndex = 4;
            this.pdbRecurse.Text = "Check sub folders";
            this.pdbRecurse.UseVisualStyleBackColor = true;
            // 
            // callStackInput
            // 
            this.callStackInput.Location = new System.Drawing.Point(0, 2);
            this.callStackInput.MaxLength = 999999999;
            this.callStackInput.Multiline = true;
            this.callStackInput.Name = "callStackInput";
            this.callStackInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.callStackInput.Size = new System.Drawing.Size(566, 731);
            this.callStackInput.TabIndex = 5;
            this.callStackInput.Text = resources.GetString("callStackInput.Text");
            this.callStackInput.WordWrap = false;
            this.callStackInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            // 
            // finalOutput
            // 
            this.finalOutput.Location = new System.Drawing.Point(572, 2);
            this.finalOutput.MaxLength = 999999999;
            this.finalOutput.Multiline = true;
            this.finalOutput.Name = "finalOutput";
            this.finalOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.finalOutput.Size = new System.Drawing.Size(1375, 731);
            this.finalOutput.TabIndex = 6;
            this.finalOutput.WordWrap = false;
            this.finalOutput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox3_KeyDown);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1318, 752);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(170, 83);
            this.button2.TabIndex = 3;
            this.button2.Text = "Enter base addresses";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DLLrecurse
            // 
            this.DLLrecurse.AutoSize = true;
            this.DLLrecurse.Checked = true;
            this.DLLrecurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DLLrecurse.Location = new System.Drawing.Point(1074, 807);
            this.DLLrecurse.Name = "DLLrecurse";
            this.DLLrecurse.Size = new System.Drawing.Size(217, 29);
            this.DLLrecurse.TabIndex = 10;
            this.DLLrecurse.Text = "Check sub folders";
            this.DLLrecurse.UseVisualStyleBackColor = true;
            // 
            // binaryPaths
            // 
            this.binaryPaths.Location = new System.Drawing.Point(198, 803);
            this.binaryPaths.Name = "binaryPaths";
            this.binaryPaths.Size = new System.Drawing.Size(870, 31);
            this.binaryPaths.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 806);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Path(s) to DLLs";
            // 
            // FramesOnSingleLine
            // 
            this.FramesOnSingleLine.AutoSize = true;
            this.FramesOnSingleLine.Location = new System.Drawing.Point(1511, 756);
            this.FramesOnSingleLine.Name = "FramesOnSingleLine";
            this.FramesOnSingleLine.Size = new System.Drawing.Size(153, 54);
            this.FramesOnSingleLine.TabIndex = 11;
            this.FramesOnSingleLine.Text = "Frames on \r\nsingle line?";
            this.FramesOnSingleLine.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1946, 847);
            this.Controls.Add(this.FramesOnSingleLine);
            this.Controls.Add(this.DLLrecurse);
            this.Controls.Add(this.binaryPaths);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.finalOutput);
            this.Controls.Add(this.callStackInput);
            this.Controls.Add(this.pdbRecurse);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pdbPaths);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SQL callstack resolver";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pdbPaths;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox pdbRecurse;
        private System.Windows.Forms.TextBox callStackInput;
        private System.Windows.Forms.TextBox finalOutput;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox DLLrecurse;
        private System.Windows.Forms.TextBox binaryPaths;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox FramesOnSingleLine;
    }
}

