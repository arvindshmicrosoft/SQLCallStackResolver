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
            if (disposing)
            {
                _resolver.Dispose();

                if (components != null)
                {
                    components.Dispose();
                }
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
            this.ResolveCallStackButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pdbPaths = new System.Windows.Forms.TextBox();
            this.genericOpenFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.pdbRecurse = new System.Windows.Forms.CheckBox();
            this.callStackInput = new System.Windows.Forms.TextBox();
            this.finalOutput = new System.Windows.Forms.TextBox();
            this.EnterBaseAddresses = new System.Windows.Forms.Button();
            this.DLLrecurse = new System.Windows.Forms.CheckBox();
            this.binaryPaths = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FramesOnSingleLine = new System.Windows.Forms.CheckBox();
            this.GetPDBDnldScript = new System.Windows.Forms.Button();
            this.LoadXELButton = new System.Windows.Forms.Button();
            this.IncludeLineNumbers = new System.Windows.Forms.CheckBox();
            this.RelookupSource = new System.Windows.Forms.CheckBox();
            this.includeOffsets = new System.Windows.Forms.CheckBox();
            this.PDBPathPicker = new System.Windows.Forms.Button();
            this.BinaryPathPicker = new System.Windows.Forms.Button();
            this.BucketizeXEL = new System.Windows.Forms.CheckBox();
            this.cachePDB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ResolveCallStackButton
            // 
            this.ResolveCallStackButton.Location = new System.Drawing.Point(1172, 481);
            this.ResolveCallStackButton.Margin = new System.Windows.Forms.Padding(2);
            this.ResolveCallStackButton.Name = "ResolveCallStackButton";
            this.ResolveCallStackButton.Size = new System.Drawing.Size(117, 53);
            this.ResolveCallStackButton.TabIndex = 0;
            this.ResolveCallStackButton.Text = "Resolve callstacks!";
            this.ResolveCallStackButton.UseVisualStyleBackColor = true;
            this.ResolveCallStackButton.Click += new System.EventHandler(this.ResolveCallstacks_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 483);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Path(s) to PDBs";
            // 
            // pdbPaths
            // 
            this.pdbPaths.Location = new System.Drawing.Point(132, 481);
            this.pdbPaths.Margin = new System.Windows.Forms.Padding(2);
            this.pdbPaths.Name = "pdbPaths";
            this.pdbPaths.Size = new System.Drawing.Size(305, 22);
            this.pdbPaths.TabIndex = 2;
            // 
            // pdbRecurse
            // 
            this.pdbRecurse.AutoSize = true;
            this.pdbRecurse.Location = new System.Drawing.Point(479, 481);
            this.pdbRecurse.Margin = new System.Windows.Forms.Padding(2);
            this.pdbRecurse.Name = "pdbRecurse";
            this.pdbRecurse.Size = new System.Drawing.Size(83, 21);
            this.pdbRecurse.TabIndex = 4;
            this.pdbRecurse.Text = "Recurse";
            this.pdbRecurse.UseVisualStyleBackColor = true;
            // 
            // callStackInput
            // 
            this.callStackInput.AllowDrop = true;
            this.callStackInput.Location = new System.Drawing.Point(0, 1);
            this.callStackInput.Margin = new System.Windows.Forms.Padding(2);
            this.callStackInput.MaxLength = 999999999;
            this.callStackInput.Multiline = true;
            this.callStackInput.Name = "callStackInput";
            this.callStackInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.callStackInput.Size = new System.Drawing.Size(379, 469);
            this.callStackInput.TabIndex = 5;
            this.callStackInput.Text = resources.GetString("callStackInput.Text");
            this.callStackInput.WordWrap = false;
            this.callStackInput.DragDrop += new System.Windows.Forms.DragEventHandler(this.CallStackInput_DragDrop);
            this.callStackInput.DragOver += new System.Windows.Forms.DragEventHandler(this.CallStackInput_DragOver);
            this.callStackInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CallStackInput_KeyDown);
            // 
            // finalOutput
            // 
            this.finalOutput.Location = new System.Drawing.Point(381, 1);
            this.finalOutput.Margin = new System.Windows.Forms.Padding(2);
            this.finalOutput.MaxLength = 999999999;
            this.finalOutput.Multiline = true;
            this.finalOutput.Name = "finalOutput";
            this.finalOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.finalOutput.Size = new System.Drawing.Size(918, 469);
            this.finalOutput.TabIndex = 6;
            this.finalOutput.WordWrap = false;
            this.finalOutput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FinalOutput_KeyDown);
            // 
            // EnterBaseAddresses
            // 
            this.EnterBaseAddresses.Location = new System.Drawing.Point(738, 474);
            this.EnterBaseAddresses.Margin = new System.Windows.Forms.Padding(2);
            this.EnterBaseAddresses.Name = "EnterBaseAddresses";
            this.EnterBaseAddresses.Size = new System.Drawing.Size(85, 60);
            this.EnterBaseAddresses.TabIndex = 3;
            this.EnterBaseAddresses.Text = "Enter base addresses";
            this.EnterBaseAddresses.UseVisualStyleBackColor = true;
            this.EnterBaseAddresses.Click += new System.EventHandler(this.EnterBaseAddresses_Click);
            // 
            // DLLrecurse
            // 
            this.DLLrecurse.AutoSize = true;
            this.DLLrecurse.Checked = true;
            this.DLLrecurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DLLrecurse.Location = new System.Drawing.Point(479, 512);
            this.DLLrecurse.Margin = new System.Windows.Forms.Padding(2);
            this.DLLrecurse.Name = "DLLrecurse";
            this.DLLrecurse.Size = new System.Drawing.Size(83, 21);
            this.DLLrecurse.TabIndex = 10;
            this.DLLrecurse.Text = "Recurse";
            this.DLLrecurse.UseVisualStyleBackColor = true;
            // 
            // binaryPaths
            // 
            this.binaryPaths.Location = new System.Drawing.Point(132, 514);
            this.binaryPaths.Margin = new System.Windows.Forms.Padding(2);
            this.binaryPaths.Name = "binaryPaths";
            this.binaryPaths.Size = new System.Drawing.Size(305, 22);
            this.binaryPaths.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 516);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Binary Path(s)";
            // 
            // FramesOnSingleLine
            // 
            this.FramesOnSingleLine.AutoSize = true;
            this.FramesOnSingleLine.Location = new System.Drawing.Point(831, 474);
            this.FramesOnSingleLine.Margin = new System.Windows.Forms.Padding(2);
            this.FramesOnSingleLine.Name = "FramesOnSingleLine";
            this.FramesOnSingleLine.Size = new System.Drawing.Size(101, 38);
            this.FramesOnSingleLine.TabIndex = 11;
            this.FramesOnSingleLine.Text = "Frames on \r\nsingle line?";
            this.FramesOnSingleLine.UseVisualStyleBackColor = true;
            // 
            // GetPDBDnldScript
            // 
            this.GetPDBDnldScript.Location = new System.Drawing.Point(643, 474);
            this.GetPDBDnldScript.Margin = new System.Windows.Forms.Padding(2);
            this.GetPDBDnldScript.Name = "GetPDBDnldScript";
            this.GetPDBDnldScript.Size = new System.Drawing.Size(91, 60);
            this.GetPDBDnldScript.TabIndex = 13;
            this.GetPDBDnldScript.Text = "Get PDB dnld script";
            this.GetPDBDnldScript.UseVisualStyleBackColor = true;
            this.GetPDBDnldScript.Click += new System.EventHandler(this.GetPDBDnldScript_Click);
            // 
            // LoadXELButton
            // 
            this.LoadXELButton.Location = new System.Drawing.Point(567, 474);
            this.LoadXELButton.Margin = new System.Windows.Forms.Padding(2);
            this.LoadXELButton.Name = "LoadXELButton";
            this.LoadXELButton.Size = new System.Drawing.Size(72, 60);
            this.LoadXELButton.TabIndex = 14;
            this.LoadXELButton.Text = "Load XEL file(s)";
            this.LoadXELButton.UseVisualStyleBackColor = true;
            this.LoadXELButton.Click += new System.EventHandler(this.LoadXELButton_Click);
            // 
            // IncludeLineNumbers
            // 
            this.IncludeLineNumbers.AutoSize = true;
            this.IncludeLineNumbers.Checked = true;
            this.IncludeLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IncludeLineNumbers.Location = new System.Drawing.Point(831, 512);
            this.IncludeLineNumbers.Margin = new System.Windows.Forms.Padding(2);
            this.IncludeLineNumbers.Name = "IncludeLineNumbers";
            this.IncludeLineNumbers.Size = new System.Drawing.Size(101, 21);
            this.IncludeLineNumbers.TabIndex = 15;
            this.IncludeLineNumbers.Text = "LineNums?";
            this.IncludeLineNumbers.UseVisualStyleBackColor = true;
            // 
            // RelookupSource
            // 
            this.RelookupSource.AutoSize = true;
            this.RelookupSource.Location = new System.Drawing.Point(937, 475);
            this.RelookupSource.Margin = new System.Windows.Forms.Padding(2);
            this.RelookupSource.Name = "RelookupSource";
            this.RelookupSource.Size = new System.Drawing.Size(90, 38);
            this.RelookupSource.TabIndex = 16;
            this.RelookupSource.Text = "Relookup\r\nsrc?";
            this.RelookupSource.UseVisualStyleBackColor = true;
            // 
            // includeOffsets
            // 
            this.includeOffsets.AutoSize = true;
            this.includeOffsets.Location = new System.Drawing.Point(937, 512);
            this.includeOffsets.Margin = new System.Windows.Forms.Padding(2);
            this.includeOffsets.Name = "includeOffsets";
            this.includeOffsets.Size = new System.Drawing.Size(83, 21);
            this.includeOffsets.TabIndex = 17;
            this.includeOffsets.Text = "Offsets?";
            this.includeOffsets.UseVisualStyleBackColor = true;
            // 
            // PDBPathPicker
            // 
            this.PDBPathPicker.Location = new System.Drawing.Point(441, 481);
            this.PDBPathPicker.Name = "PDBPathPicker";
            this.PDBPathPicker.Size = new System.Drawing.Size(33, 23);
            this.PDBPathPicker.TabIndex = 19;
            this.PDBPathPicker.Text = "...";
            this.PDBPathPicker.UseVisualStyleBackColor = true;
            this.PDBPathPicker.Click += new System.EventHandler(this.PDBPathPicker_Click);
            // 
            // BinaryPathPicker
            // 
            this.BinaryPathPicker.Location = new System.Drawing.Point(441, 514);
            this.BinaryPathPicker.Name = "BinaryPathPicker";
            this.BinaryPathPicker.Size = new System.Drawing.Size(33, 23);
            this.BinaryPathPicker.TabIndex = 20;
            this.BinaryPathPicker.Text = "...";
            this.BinaryPathPicker.UseVisualStyleBackColor = true;
            this.BinaryPathPicker.Click += new System.EventHandler(this.BinaryPathPicker_Click);
            // 
            // BucketizeXEL
            // 
            this.BucketizeXEL.AutoSize = true;
            this.BucketizeXEL.Checked = true;
            this.BucketizeXEL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BucketizeXEL.Location = new System.Drawing.Point(1031, 481);
            this.BucketizeXEL.Margin = new System.Windows.Forms.Padding(2);
            this.BucketizeXEL.Name = "BucketizeXEL";
            this.BucketizeXEL.Size = new System.Drawing.Size(129, 21);
            this.BucketizeXEL.TabIndex = 21;
            this.BucketizeXEL.Text = "Bucketize XEL?";
            this.BucketizeXEL.UseVisualStyleBackColor = true;
            // 
            // cachePDB
            // 
            this.cachePDB.AutoSize = true;
            this.cachePDB.Location = new System.Drawing.Point(1031, 512);
            this.cachePDB.Margin = new System.Windows.Forms.Padding(2);
            this.cachePDB.Name = "cachePDB";
            this.cachePDB.Size = new System.Drawing.Size(109, 21);
            this.cachePDB.TabIndex = 22;
            this.cachePDB.Text = "Copy PDBs?";
            this.cachePDB.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1283, 542);
            this.Controls.Add(this.cachePDB);
            this.Controls.Add(this.BucketizeXEL);
            this.Controls.Add(this.BinaryPathPicker);
            this.Controls.Add(this.PDBPathPicker);
            this.Controls.Add(this.includeOffsets);
            this.Controls.Add(this.RelookupSource);
            this.Controls.Add(this.IncludeLineNumbers);
            this.Controls.Add(this.LoadXELButton);
            this.Controls.Add(this.GetPDBDnldScript);
            this.Controls.Add(this.FramesOnSingleLine);
            this.Controls.Add(this.DLLrecurse);
            this.Controls.Add(this.binaryPaths);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.finalOutput);
            this.Controls.Add(this.callStackInput);
            this.Controls.Add(this.pdbRecurse);
            this.Controls.Add(this.EnterBaseAddresses);
            this.Controls.Add(this.pdbPaths);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ResolveCallStackButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SQL callstack resolver";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ResolveCallStackButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pdbPaths;
        private System.Windows.Forms.OpenFileDialog genericOpenFileDlg;
        private System.Windows.Forms.CheckBox pdbRecurse;
        private System.Windows.Forms.TextBox callStackInput;
        private System.Windows.Forms.TextBox finalOutput;
        private System.Windows.Forms.Button EnterBaseAddresses;
        private System.Windows.Forms.CheckBox DLLrecurse;
        private System.Windows.Forms.TextBox binaryPaths;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox FramesOnSingleLine;
        private System.Windows.Forms.Button GetPDBDnldScript;
        private System.Windows.Forms.Button LoadXELButton;
        private System.Windows.Forms.CheckBox IncludeLineNumbers;
        private System.Windows.Forms.CheckBox RelookupSource;
        private System.Windows.Forms.CheckBox includeOffsets;
        private System.Windows.Forms.Button PDBPathPicker;
        private System.Windows.Forms.Button BinaryPathPicker;
        private System.Windows.Forms.CheckBox BucketizeXEL;
        private System.Windows.Forms.CheckBox cachePDB;
    }
}

