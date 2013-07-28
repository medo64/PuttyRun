namespace PuttyRun {
    partial class MainForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnu = new System.Windows.Forms.ToolStrip();
            this.mnuConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuNewFolder = new System.Windows.Forms.ToolStripButton();
            this.mnuRename = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRefresh = new System.Windows.Forms.ToolStripButton();
            this.mnuApp = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuAppFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppUpgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppDonate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAppAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptions = new System.Windows.Forms.ToolStripButton();
            this.tree = new System.Windows.Forms.TreeView();
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.staWarningNoPutty = new System.Windows.Forms.StatusStrip();
            this.staWarningNoPuttyLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrCheckPuttyExecutable = new System.Windows.Forms.Timer(this.components);
            this.mnu.SuspendLayout();
            this.staWarningNoPutty.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu
            // 
            this.mnu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuConnect,
            this.toolStripSeparator1,
            this.mnuNewFolder,
            this.mnuRename,
            this.toolStripSeparator2,
            this.mnuRefresh,
            this.mnuApp,
            this.mnuOptions});
            this.mnu.Location = new System.Drawing.Point(0, 0);
            this.mnu.Name = "mnu";
            this.mnu.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.mnu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mnu.Size = new System.Drawing.Size(302, 27);
            this.mnu.TabIndex = 0;
            // 
            // mnuConnect
            // 
            this.mnuConnect.Image = ((System.Drawing.Image)(resources.GetObject("mnuConnect.Image")));
            this.mnuConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(83, 24);
            this.mnuConnect.Text = "Connect";
            this.mnuConnect.ToolTipText = "Connect (Enter)";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // mnuNewFolder
            // 
            this.mnuNewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("mnuNewFolder.Image")));
            this.mnuNewFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuNewFolder.Name = "mnuNewFolder";
            this.mnuNewFolder.Size = new System.Drawing.Size(23, 24);
            this.mnuNewFolder.Text = "New folder";
            this.mnuNewFolder.Click += new System.EventHandler(this.mnuNewFolder_Click);
            // 
            // mnuRename
            // 
            this.mnuRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuRename.Image = ((System.Drawing.Image)(resources.GetObject("mnuRename.Image")));
            this.mnuRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuRename.Name = "mnuRename";
            this.mnuRename.Size = new System.Drawing.Size(23, 24);
            this.mnuRename.Text = "Rename";
            this.mnuRename.ToolTipText = "Rename (F2)";
            this.mnuRename.Click += new System.EventHandler(this.mnuRename_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // mnuRefresh
            // 
            this.mnuRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuRefresh.Image = ((System.Drawing.Image)(resources.GetObject("mnuRefresh.Image")));
            this.mnuRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuRefresh.Name = "mnuRefresh";
            this.mnuRefresh.Size = new System.Drawing.Size(23, 24);
            this.mnuRefresh.Text = "Refresh";
            this.mnuRefresh.ToolTipText = "Refresh (F5)";
            this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
            // 
            // mnuApp
            // 
            this.mnuApp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuApp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAppFeedback,
            this.mnuAppUpgrade,
            this.mnuAppDonate,
            this.toolStripMenuItem1,
            this.mnuAppAbout});
            this.mnuApp.Image = ((System.Drawing.Image)(resources.GetObject("mnuApp.Image")));
            this.mnuApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuApp.Name = "mnuApp";
            this.mnuApp.Size = new System.Drawing.Size(29, 24);
            this.mnuApp.Text = "Application";
            // 
            // mnuAppFeedback
            // 
            this.mnuAppFeedback.Name = "mnuAppFeedback";
            this.mnuAppFeedback.Size = new System.Drawing.Size(200, 24);
            this.mnuAppFeedback.Text = "Send feedback";
            this.mnuAppFeedback.Click += new System.EventHandler(this.mnuAppFeedback_Click);
            // 
            // mnuAppUpgrade
            // 
            this.mnuAppUpgrade.Name = "mnuAppUpgrade";
            this.mnuAppUpgrade.Size = new System.Drawing.Size(200, 24);
            this.mnuAppUpgrade.Text = "Check for upgrade";
            this.mnuAppUpgrade.Click += new System.EventHandler(this.mnuAppUpgrade_Click);
            // 
            // mnuAppDonate
            // 
            this.mnuAppDonate.Name = "mnuAppDonate";
            this.mnuAppDonate.Size = new System.Drawing.Size(200, 24);
            this.mnuAppDonate.Text = "Donate";
            this.mnuAppDonate.Click += new System.EventHandler(this.mnuAppDonate_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(197, 6);
            // 
            // mnuAppAbout
            // 
            this.mnuAppAbout.Name = "mnuAppAbout";
            this.mnuAppAbout.Size = new System.Drawing.Size(200, 24);
            this.mnuAppAbout.Text = "About";
            this.mnuAppAbout.Click += new System.EventHandler(this.mnuAppAbout_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuOptions.Image = ((System.Drawing.Image)(resources.GetObject("mnuOptions.Image")));
            this.mnuOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(23, 24);
            this.mnuOptions.Text = "Options";
            this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.FullRowSelect = true;
            this.tree.HideSelection = false;
            this.tree.ImageIndex = 6;
            this.tree.ImageList = this.treeImages;
            this.tree.LabelEdit = true;
            this.tree.Location = new System.Drawing.Point(0, 27);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 6;
            this.tree.ShowLines = false;
            this.tree.ShowNodeToolTips = true;
            this.tree.Size = new System.Drawing.Size(302, 366);
            this.tree.TabIndex = 1;
            this.tree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tree_BeforeLabelEdit);
            this.tree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tree_AfterLabelEdit);
            this.tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_ItemDrag);
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
            this.tree.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_DragDrop);
            this.tree.DragOver += new System.Windows.Forms.DragEventHandler(this.tree_DragOver);
            this.tree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tree_KeyDown);
            this.tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
            // 
            // treeImages
            // 
            this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
            this.treeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.treeImages.Images.SetKeyName(0, "ConnectionUnknown");
            this.treeImages.Images.SetKeyName(1, "ConnectionRaw");
            this.treeImages.Images.SetKeyName(2, "ConnectionTelnet");
            this.treeImages.Images.SetKeyName(3, "ConnectionRLogin");
            this.treeImages.Images.SetKeyName(4, "ConnectionSsh");
            this.treeImages.Images.SetKeyName(5, "ConnectionSerial");
            this.treeImages.Images.SetKeyName(6, "Folder");
            // 
            // staWarningNoPutty
            // 
            this.staWarningNoPutty.BackColor = System.Drawing.Color.Yellow;
            this.staWarningNoPutty.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staWarningNoPuttyLabel});
            this.staWarningNoPutty.Location = new System.Drawing.Point(0, 368);
            this.staWarningNoPutty.Name = "staWarningNoPutty";
            this.staWarningNoPutty.Size = new System.Drawing.Size(302, 25);
            this.staWarningNoPutty.SizingGrip = false;
            this.staWarningNoPutty.TabIndex = 3;
            this.staWarningNoPutty.Visible = false;
            // 
            // staWarningNoPuttyLabel
            // 
            this.staWarningNoPuttyLabel.Name = "staWarningNoPuttyLabel";
            this.staWarningNoPuttyLabel.Size = new System.Drawing.Size(287, 20);
            this.staWarningNoPuttyLabel.Spring = true;
            this.staWarningNoPuttyLabel.Text = "Cannot find PuTTY executable.";
            // 
            // tmrCheckPuttyExecutable
            // 
            this.tmrCheckPuttyExecutable.Enabled = true;
            this.tmrCheckPuttyExecutable.Interval = 10000;
            this.tmrCheckPuttyExecutable.Tick += new System.EventHandler(this.tmrCheckPuttyExecutable_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 393);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.staWarningNoPutty);
            this.Controls.Add(this.mnu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(320, 360);
            this.Name = "MainForm";
            this.Text = "PuTTY Run";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FormClosed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.mnu.ResumeLayout(false);
            this.mnu.PerformLayout();
            this.staWarningNoPutty.ResumeLayout(false);
            this.staWarningNoPutty.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mnu;
        private System.Windows.Forms.ToolStripButton mnuConnect;
        private System.Windows.Forms.ToolStripButton mnuRefresh;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton mnuApp;
        private System.Windows.Forms.ToolStripButton mnuOptions;
        private System.Windows.Forms.ImageList treeImages;
        private System.Windows.Forms.ToolStripMenuItem mnuAppFeedback;
        private System.Windows.Forms.ToolStripMenuItem mnuAppUpgrade;
        private System.Windows.Forms.ToolStripMenuItem mnuAppDonate;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuAppAbout;
        private System.Windows.Forms.StatusStrip staWarningNoPutty;
        private System.Windows.Forms.ToolStripStatusLabel staWarningNoPuttyLabel;
        private System.Windows.Forms.Timer tmrCheckPuttyExecutable;
        private System.Windows.Forms.ToolStripButton mnuNewFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton mnuRename;
    }
}

