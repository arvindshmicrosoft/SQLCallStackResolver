namespace Microsoft.SqlServer.Utils.Misc.SQLCallStackResolver
{
    partial class MultilineInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultilineInput));
            this.OkButton = new System.Windows.Forms.Button();
            this.FormCancelButton = new System.Windows.Forms.Button();
            this.InputAddresses = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(240, 394);
            this.OkButton.Margin = new System.Windows.Forms.Padding(2);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(63, 28);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // FormCancelButton
            // 
            this.FormCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.FormCancelButton.Location = new System.Drawing.Point(319, 394);
            this.FormCancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.FormCancelButton.Name = "FormCancelButton";
            this.FormCancelButton.Size = new System.Drawing.Size(63, 28);
            this.FormCancelButton.TabIndex = 1;
            this.FormCancelButton.Text = "Cancel";
            this.FormCancelButton.UseVisualStyleBackColor = true;
            // 
            // InputAddresses
            // 
            this.InputAddresses.AllowDrop = true;
            this.InputAddresses.Location = new System.Drawing.Point(0, 0);
            this.InputAddresses.Margin = new System.Windows.Forms.Padding(2);
            this.InputAddresses.MaxLength = 999999999;
            this.InputAddresses.Multiline = true;
            this.InputAddresses.Name = "InputAddresses";
            this.InputAddresses.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.InputAddresses.Size = new System.Drawing.Size(622, 384);
            this.InputAddresses.TabIndex = 2;
            this.InputAddresses.Text = resources.GetString("InputAddresses.Text");
            this.InputAddresses.WordWrap = false;
            this.InputAddresses.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputAddresses_DragDrop);
            this.InputAddresses.DragOver += new System.Windows.Forms.DragEventHandler(this.InputAddresses_DragOver);
            this.InputAddresses.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputAddresses_KeyDown);
            // 
            // MultilineInput
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 429);
            this.Controls.Add(this.InputAddresses);
            this.Controls.Add(this.FormCancelButton);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultilineInput";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MultilineInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button FormCancelButton;
        private System.Windows.Forms.TextBox InputAddresses;
    }
}