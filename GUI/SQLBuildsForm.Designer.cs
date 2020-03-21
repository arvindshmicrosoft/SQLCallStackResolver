namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    partial class SQLBuildsForm
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
            this.treeviewSyms = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.downloadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.downloadProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.checkPDBAvail = new System.Windows.Forms.Button();
            this.dnldButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeviewSyms
            // 
            this.treeviewSyms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeviewSyms.Location = new System.Drawing.Point(0, 0);
            this.treeviewSyms.Name = "treeviewSyms";
            this.treeviewSyms.Size = new System.Drawing.Size(411, 474);
            this.treeviewSyms.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadStatus,
            this.downloadProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 522);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(411, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // downloadStatus
            // 
            this.downloadStatus.Name = "downloadStatus";
            this.downloadStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // downloadProgress
            // 
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.Size = new System.Drawing.Size(75, 16);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeviewSyms);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkPDBAvail);
            this.splitContainer1.Panel2.Controls.Add(this.dnldButton);
            this.splitContainer1.Size = new System.Drawing.Size(411, 544);
            this.splitContainer1.SplitterDistance = 474;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 3;
            // 
            // checkPDBAvail
            // 
            this.checkPDBAvail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkPDBAvail.Location = new System.Drawing.Point(66, 12);
            this.checkPDBAvail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkPDBAvail.Name = "checkPDBAvail";
            this.checkPDBAvail.Size = new System.Drawing.Size(128, 26);
            this.checkPDBAvail.TabIndex = 3;
            this.checkPDBAvail.Text = "Check PDB availability";
            this.checkPDBAvail.UseVisualStyleBackColor = true;
            this.checkPDBAvail.Click += new System.EventHandler(this.CheckPDBAvail_Click);
            // 
            // dnldButton
            // 
            this.dnldButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dnldButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.dnldButton.Location = new System.Drawing.Point(199, 12);
            this.dnldButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dnldButton.Name = "dnldButton";
            this.dnldButton.Size = new System.Drawing.Size(142, 26);
            this.dnldButton.TabIndex = 2;
            this.dnldButton.Text = "Download PDBs";
            this.dnldButton.UseVisualStyleBackColor = true;
            this.dnldButton.Click += new System.EventHandler(this.DownloadPDBs);
            // 
            // SQLBuildsForm
            // 
            this.AcceptButton = this.dnldButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 544);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SQLBuildsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Select SQL build";
            this.Load += new System.EventHandler(this.Treeview_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeviewSyms;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel downloadStatus;
        private System.Windows.Forms.ToolStripProgressBar downloadProgress;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button dnldButton;
        private System.Windows.Forms.Button checkPDBAvail;
    }
}