namespace PuttyRun {
    partial class OptionsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.txtPuttyExe = new System.Windows.Forms.TextBox();
            this.lblPuttyExe = new System.Windows.Forms.Label();
            this.btnPuttyExeBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chbRunOnStartup = new System.Windows.Forms.CheckBox();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.txtHotkey = new System.Windows.Forms.TextBox();
            this.btnAllowRegistry = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPuttyExe
            // 
            this.txtPuttyExe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPuttyExe.Location = new System.Drawing.Point(12, 125);
            this.txtPuttyExe.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtPuttyExe.Name = "txtPuttyExe";
            this.txtPuttyExe.Size = new System.Drawing.Size(416, 22);
            this.txtPuttyExe.TabIndex = 5;
            // 
            // lblPuttyExe
            // 
            this.lblPuttyExe.AutoSize = true;
            this.lblPuttyExe.Location = new System.Drawing.Point(12, 105);
            this.lblPuttyExe.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.lblPuttyExe.Name = "lblPuttyExe";
            this.lblPuttyExe.Size = new System.Drawing.Size(128, 17);
            this.lblPuttyExe.TabIndex = 4;
            this.lblPuttyExe.Text = "PuTTY executable:";
            // 
            // btnPuttyExeBrowse
            // 
            this.btnPuttyExeBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPuttyExeBrowse.Location = new System.Drawing.Point(428, 125);
            this.btnPuttyExeBrowse.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnPuttyExeBrowse.Name = "btnPuttyExeBrowse";
            this.btnPuttyExeBrowse.Size = new System.Drawing.Size(22, 22);
            this.btnPuttyExeBrowse.TabIndex = 6;
            this.btnPuttyExeBrowse.Text = "...";
            this.btnPuttyExeBrowse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPuttyExeBrowse.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(274, 165);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 25);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 165);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chbRunOnStartup
            // 
            this.chbRunOnStartup.AutoSize = true;
            this.chbRunOnStartup.Location = new System.Drawing.Point(12, 12);
            this.chbRunOnStartup.Name = "chbRunOnStartup";
            this.chbRunOnStartup.Size = new System.Drawing.Size(148, 21);
            this.chbRunOnStartup.TabIndex = 1;
            this.chbRunOnStartup.Text = "Start with Windows";
            this.chbRunOnStartup.UseVisualStyleBackColor = true;
            // 
            // lblHotkey
            // 
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(12, 48);
            this.lblHotkey.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(56, 17);
            this.lblHotkey.TabIndex = 2;
            this.lblHotkey.Text = "Hotkey:";
            // 
            // txtHotkey
            // 
            this.txtHotkey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHotkey.Location = new System.Drawing.Point(12, 68);
            this.txtHotkey.Name = "txtHotkey";
            this.txtHotkey.ReadOnly = true;
            this.txtHotkey.Size = new System.Drawing.Size(438, 22);
            this.txtHotkey.TabIndex = 3;
            this.txtHotkey.Leave += new System.EventHandler(this.txtHotkey_Leave);
            this.txtHotkey.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtHotkey_PreviewKeyDown);
            // 
            // btnAllowRegistry
            // 
            this.btnAllowRegistry.Location = new System.Drawing.Point(12, 165);
            this.btnAllowRegistry.Name = "btnAllowRegistry";
            this.btnAllowRegistry.Size = new System.Drawing.Size(100, 25);
            this.btnAllowRegistry.TabIndex = 7;
            this.btnAllowRegistry.Text = "Allow save";
            this.btnAllowRegistry.UseVisualStyleBackColor = true;
            this.btnAllowRegistry.Visible = false;
            this.btnAllowRegistry.Click += new System.EventHandler(this.btnAllowRegistry_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(462, 202);
            this.Controls.Add(this.btnAllowRegistry);
            this.Controls.Add(this.txtHotkey);
            this.Controls.Add(this.lblHotkey);
            this.Controls.Add(this.chbRunOnStartup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnPuttyExeBrowse);
            this.Controls.Add(this.lblPuttyExe);
            this.Controls.Add(this.txtPuttyExe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPuttyExe;
        private System.Windows.Forms.Label lblPuttyExe;
        private System.Windows.Forms.Button btnPuttyExeBrowse;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chbRunOnStartup;
        private System.Windows.Forms.Label lblHotkey;
        private System.Windows.Forms.TextBox txtHotkey;
        private System.Windows.Forms.Button btnAllowRegistry;
    }
}