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
            this.genericOpenFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.callStackInput = new System.Windows.Forms.TextBox();
            this.finalOutput = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.FramesOnSingleLine = new System.Windows.Forms.CheckBox();
            this.BucketizeXEL = new System.Windows.Forms.CheckBox();
            this.LoadXELButton = new System.Windows.Forms.Button();
            this.EnterBaseAddresses = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DLLrecurse = new System.Windows.Forms.CheckBox();
            this.selectSQLPDB = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.binaryPaths = new System.Windows.Forms.TextBox();
            this.PDBPathPicker = new System.Windows.Forms.Button();
            this.BinaryPathPicker = new System.Windows.Forms.Button();
            this.pdbRecurse = new System.Windows.Forms.CheckBox();
            this.GetPDBDnldScript = new System.Windows.Forms.Button();
            this.pdbPaths = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.IncludeLineNumbers = new System.Windows.Forms.CheckBox();
            this.includeOffsets = new System.Windows.Forms.CheckBox();
            this.RelookupSource = new System.Windows.Forms.CheckBox();
            this.ResolveCallStackButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.cachePDB = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2.Controls.Add(this.ResolveCallStackButton);
            this.splitContainer2.Size = new System.Drawing.Size(1305, 615);
            this.splitContainer2.SplitterDistance = 445;
            this.splitContainer2.TabIndex = 30;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.callStackInput);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.finalOutput);
            this.splitContainer1.Size = new System.Drawing.Size(1305, 445);
            this.splitContainer1.SplitterDistance = 434;
            this.splitContainer1.TabIndex = 30;
            // 
            // callStackInput
            // 
            this.callStackInput.AllowDrop = true;
            this.callStackInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.callStackInput.Location = new System.Drawing.Point(0, 0);
            this.callStackInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.callStackInput.MaxLength = 999999999;
            this.callStackInput.Multiline = true;
            this.callStackInput.Name = "callStackInput";
            this.callStackInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.callStackInput.Size = new System.Drawing.Size(434, 445);
            this.callStackInput.TabIndex = 8;
            this.callStackInput.Text = resources.GetString("callStackInput.Text");
            this.callStackInput.WordWrap = false;
            // 
            // finalOutput
            // 
            this.finalOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.finalOutput.Location = new System.Drawing.Point(0, 0);
            this.finalOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.finalOutput.MaxLength = 999999999;
            this.finalOutput.Multiline = true;
            this.finalOutput.Name = "finalOutput";
            this.finalOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.finalOutput.Size = new System.Drawing.Size(867, 445);
            this.finalOutput.TabIndex = 8;
            this.finalOutput.WordWrap = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.FramesOnSingleLine);
            this.groupBox3.Controls.Add(this.BucketizeXEL);
            this.groupBox3.Controls.Add(this.LoadXELButton);
            this.groupBox3.Controls.Add(this.EnterBaseAddresses);
            this.groupBox3.Location = new System.Drawing.Point(4, 113);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(1237, 59);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options for callstack input";
            // 
            // FramesOnSingleLine
            // 
            this.FramesOnSingleLine.AutoSize = true;
            this.FramesOnSingleLine.Location = new System.Drawing.Point(169, 25);
            this.FramesOnSingleLine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FramesOnSingleLine.Name = "FramesOnSingleLine";
            this.FramesOnSingleLine.Size = new System.Drawing.Size(252, 21);
            this.FramesOnSingleLine.TabIndex = 23;
            this.FramesOnSingleLine.Text = "Callstack frames are in a single line";
            this.FramesOnSingleLine.UseVisualStyleBackColor = true;
            // 
            // BucketizeXEL
            // 
            this.BucketizeXEL.AutoSize = true;
            this.BucketizeXEL.Checked = true;
            this.BucketizeXEL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BucketizeXEL.Location = new System.Drawing.Point(9, 23);
            this.BucketizeXEL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BucketizeXEL.Name = "BucketizeXEL";
            this.BucketizeXEL.Size = new System.Drawing.Size(147, 21);
            this.BucketizeXEL.TabIndex = 22;
            this.BucketizeXEL.Text = "Bucketize XEvents";
            this.BucketizeXEL.UseVisualStyleBackColor = true;
            // 
            // LoadXELButton
            // 
            this.LoadXELButton.Location = new System.Drawing.Point(440, 15);
            this.LoadXELButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoadXELButton.Name = "LoadXELButton";
            this.LoadXELButton.Size = new System.Drawing.Size(240, 37);
            this.LoadXELButton.TabIndex = 15;
            this.LoadXELButton.Text = "Load callstacks from .XEL files";
            this.LoadXELButton.UseVisualStyleBackColor = true;
            this.LoadXELButton.Click += new System.EventHandler(this.LoadXELButton_Click);
            // 
            // EnterBaseAddresses
            // 
            this.EnterBaseAddresses.Location = new System.Drawing.Point(685, 15);
            this.EnterBaseAddresses.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.EnterBaseAddresses.Name = "EnterBaseAddresses";
            this.EnterBaseAddresses.Size = new System.Drawing.Size(255, 37);
            this.EnterBaseAddresses.TabIndex = 3;
            this.EnterBaseAddresses.Text = "Specify base addresses for modules";
            this.EnterBaseAddresses.UseVisualStyleBackColor = true;
            this.EnterBaseAddresses.Click += new System.EventHandler(this.EnterBaseAddresses_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cachePDB);
            this.groupBox2.Controls.Add(this.DLLrecurse);
            this.groupBox2.Controls.Add(this.selectSQLPDB);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.binaryPaths);
            this.groupBox2.Controls.Add(this.PDBPathPicker);
            this.groupBox2.Controls.Add(this.BinaryPathPicker);
            this.groupBox2.Controls.Add(this.pdbRecurse);
            this.groupBox2.Controls.Add(this.GetPDBDnldScript);
            this.groupBox2.Controls.Add(this.pdbPaths);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(4, 7);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(793, 100);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configure PDB symbols";
            // 
            // DLLrecurse
            // 
            this.DLLrecurse.AutoSize = true;
            this.DLLrecurse.Location = new System.Drawing.Point(477, 69);
            this.DLLrecurse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DLLrecurse.Name = "DLLrecurse";
            this.DLLrecurse.Size = new System.Drawing.Size(83, 21);
            this.DLLrecurse.TabIndex = 10;
            this.DLLrecurse.Text = "Recurse";
            this.DLLrecurse.UseVisualStyleBackColor = true;
            // 
            // selectSQLPDB
            // 
            this.selectSQLPDB.Location = new System.Drawing.Point(5, 23);
            this.selectSQLPDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.selectSQLPDB.Name = "selectSQLPDB";
            this.selectSQLPDB.Size = new System.Drawing.Size(69, 31);
            this.selectSQLPDB.TabIndex = 32;
            this.selectSQLPDB.Text = "Presets";
            this.selectSQLPDB.UseVisualStyleBackColor = true;
            this.selectSQLPDB.Click += new System.EventHandler(this.SelectSQLPDB_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Binary Path(s)";
            // 
            // binaryPaths
            // 
            this.binaryPaths.Location = new System.Drawing.Point(131, 68);
            this.binaryPaths.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.binaryPaths.Name = "binaryPaths";
            this.binaryPaths.Size = new System.Drawing.Size(305, 22);
            this.binaryPaths.TabIndex = 9;
            // 
            // PDBPathPicker
            // 
            this.PDBPathPicker.Location = new System.Drawing.Point(506, 29);
            this.PDBPathPicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PDBPathPicker.Name = "PDBPathPicker";
            this.PDBPathPicker.Size = new System.Drawing.Size(33, 23);
            this.PDBPathPicker.TabIndex = 29;
            this.PDBPathPicker.Text = "...";
            this.PDBPathPicker.UseVisualStyleBackColor = true;
            this.PDBPathPicker.Click += new System.EventHandler(this.PDBPathPicker_Click);
            // 
            // BinaryPathPicker
            // 
            this.BinaryPathPicker.Location = new System.Drawing.Point(440, 68);
            this.BinaryPathPicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BinaryPathPicker.Name = "BinaryPathPicker";
            this.BinaryPathPicker.Size = new System.Drawing.Size(33, 23);
            this.BinaryPathPicker.TabIndex = 20;
            this.BinaryPathPicker.Text = "...";
            this.BinaryPathPicker.UseVisualStyleBackColor = true;
            this.BinaryPathPicker.Click += new System.EventHandler(this.BinaryPathPicker_Click);
            // 
            // pdbRecurse
            // 
            this.pdbRecurse.AutoSize = true;
            this.pdbRecurse.Location = new System.Drawing.Point(545, 29);
            this.pdbRecurse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pdbRecurse.Name = "pdbRecurse";
            this.pdbRecurse.Size = new System.Drawing.Size(83, 21);
            this.pdbRecurse.TabIndex = 28;
            this.pdbRecurse.Text = "Recurse";
            this.pdbRecurse.UseVisualStyleBackColor = true;
            // 
            // GetPDBDnldScript
            // 
            this.GetPDBDnldScript.Location = new System.Drawing.Point(565, 63);
            this.GetPDBDnldScript.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GetPDBDnldScript.Name = "GetPDBDnldScript";
            this.GetPDBDnldScript.Size = new System.Drawing.Size(209, 30);
            this.GetPDBDnldScript.TabIndex = 13;
            this.GetPDBDnldScript.Text = "Get PDB download script";
            this.GetPDBDnldScript.UseVisualStyleBackColor = true;
            this.GetPDBDnldScript.Click += new System.EventHandler(this.GetPDBDnldScript_Click);
            // 
            // pdbPaths
            // 
            this.pdbPaths.Location = new System.Drawing.Point(188, 30);
            this.pdbPaths.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pdbPaths.Name = "pdbPaths";
            this.pdbPaths.Size = new System.Drawing.Size(312, 22);
            this.pdbPaths.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 26;
            this.label1.Text = "Path(s) to PDBs";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.IncludeLineNumbers);
            this.groupBox1.Controls.Add(this.includeOffsets);
            this.groupBox1.Controls.Add(this.RelookupSource);
            this.groupBox1.Location = new System.Drawing.Point(919, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(323, 103);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced Options";
            // 
            // IncludeLineNumbers
            // 
            this.IncludeLineNumbers.AutoSize = true;
            this.IncludeLineNumbers.Checked = true;
            this.IncludeLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IncludeLineNumbers.Location = new System.Drawing.Point(5, 46);
            this.IncludeLineNumbers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IncludeLineNumbers.Name = "IncludeLineNumbers";
            this.IncludeLineNumbers.Size = new System.Drawing.Size(293, 21);
            this.IncludeLineNumbers.TabIndex = 16;
            this.IncludeLineNumbers.Text = "Show source info (needs private symbols)";
            this.IncludeLineNumbers.UseVisualStyleBackColor = true;
            // 
            // includeOffsets
            // 
            this.includeOffsets.AutoSize = true;
            this.includeOffsets.Location = new System.Drawing.Point(5, 20);
            this.includeOffsets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.includeOffsets.Name = "includeOffsets";
            this.includeOffsets.Size = new System.Drawing.Size(164, 21);
            this.includeOffsets.TabIndex = 17;
            this.includeOffsets.Text = "Show function offsets";
            this.includeOffsets.UseVisualStyleBackColor = true;
            // 
            // RelookupSource
            // 
            this.RelookupSource.AutoSize = true;
            this.RelookupSource.Location = new System.Drawing.Point(5, 70);
            this.RelookupSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RelookupSource.Name = "RelookupSource";
            this.RelookupSource.Size = new System.Drawing.Size(297, 21);
            this.RelookupSource.TabIndex = 16;
            this.RelookupSource.Text = "Re-lookup source (needs private symbols)";
            this.RelookupSource.UseVisualStyleBackColor = true;
            // 
            // ResolveCallStackButton
            // 
            this.ResolveCallStackButton.Location = new System.Drawing.Point(803, 7);
            this.ResolveCallStackButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ResolveCallStackButton.Name = "ResolveCallStackButton";
            this.ResolveCallStackButton.Size = new System.Drawing.Size(112, 100);
            this.ResolveCallStackButton.TabIndex = 29;
            this.ResolveCallStackButton.Text = "Resolve callstacks!";
            this.ResolveCallStackButton.UseVisualStyleBackColor = true;
            this.ResolveCallStackButton.Click += new System.EventHandler(this.ResolveCallstacks_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 593);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1305, 22);
            this.statusStrip1.TabIndex = 31;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 16);
            // 
            // cachePDB
            // 
            this.cachePDB.AutoSize = true;
            this.cachePDB.Location = new System.Drawing.Point(634, 29);
            this.cachePDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cachePDB.Name = "cachePDB";
            this.cachePDB.Size = new System.Drawing.Size(109, 21);
            this.cachePDB.TabIndex = 33;
            this.cachePDB.Text = "Cache PDBs";
            this.cachePDB.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1305, 615);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "SQLCallstackResolver (http://aka.ms/sqlstack)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog genericOpenFileDlg;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox callStackInput;
        private System.Windows.Forms.TextBox finalOutput;
        private System.Windows.Forms.CheckBox DLLrecurse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox binaryPaths;
        private System.Windows.Forms.Button BinaryPathPicker;
        private System.Windows.Forms.Button GetPDBDnldScript;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox FramesOnSingleLine;
        private System.Windows.Forms.CheckBox BucketizeXEL;
        private System.Windows.Forms.Button LoadXELButton;
        private System.Windows.Forms.Button EnterBaseAddresses;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button selectSQLPDB;
        private System.Windows.Forms.Button PDBPathPicker;
        private System.Windows.Forms.CheckBox pdbRecurse;
        private System.Windows.Forms.TextBox pdbPaths;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox IncludeLineNumbers;
        private System.Windows.Forms.CheckBox includeOffsets;
        private System.Windows.Forms.CheckBox RelookupSource;
        private System.Windows.Forms.Button ResolveCallStackButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.CheckBox cachePDB;
    }
}
