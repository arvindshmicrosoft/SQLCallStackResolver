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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.downloadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.downloadProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dnldButton = new System.Windows.Forms.Button();
            this.checkPDBAvail = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(548, 598);
            this.treeView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadStatus,
            this.downloadProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 645);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(548, 24);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // downloadStatus
            // 
            this.downloadStatus.Name = "downloadStatus";
            this.downloadStatus.Size = new System.Drawing.Size(0, 19);
            // 
            // downloadProgress
            // 
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.Size = new System.Drawing.Size(100, 18);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkPDBAvail);
            this.splitContainer1.Panel2.Controls.Add(this.dnldButton);
            this.splitContainer1.Size = new System.Drawing.Size(548, 669);
            this.splitContainer1.SplitterDistance = 598;
            this.splitContainer1.TabIndex = 3;
            // 
            // dnldButton
            // 
            this.dnldButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dnldButton.Location = new System.Drawing.Point(265, 8);
            this.dnldButton.Name = "dnldButton";
            this.dnldButton.Size = new System.Drawing.Size(189, 32);
            this.dnldButton.TabIndex = 2;
            this.dnldButton.Text = "Download PDBs";
            this.dnldButton.UseVisualStyleBackColor = true;
            this.dnldButton.Click += new System.EventHandler(this.DownloadPDBs);
            // 
            // checkPDBAvail
            // 
            this.checkPDBAvail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkPDBAvail.Location = new System.Drawing.Point(88, 8);
            this.checkPDBAvail.Name = "checkPDBAvail";
            this.checkPDBAvail.Size = new System.Drawing.Size(171, 32);
            this.checkPDBAvail.TabIndex = 3;
            this.checkPDBAvail.Text = "Check PDB availability";
            this.checkPDBAvail.UseVisualStyleBackColor = true;
            this.checkPDBAvail.Click += new System.EventHandler(this.checkPDBAvail_Click);
            // 
            // SQLBuildsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 669);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
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

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel downloadStatus;
        private System.Windows.Forms.ToolStripProgressBar downloadProgress;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button dnldButton;
        private System.Windows.Forms.Button checkPDBAvail;
    }
}