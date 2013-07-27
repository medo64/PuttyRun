using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PuttyRun {
    internal partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            mnu.Renderer = new ToolStripBorderlessProfessionalRenderer();

            Medo.Windows.Forms.State.SetupOnLoadAndClose(this);
        }


        private void Form_Load(object sender, System.EventArgs e) {
            FillNodes();
            tmrCheckPuttyExecutable_Tick(null, null);
        }


        private void Form_FormClosing(object sender, FormClosingEventArgs e) {
#if !DEBUG
            switch (e.CloseReason) {
                case CloseReason.ApplicationExitCall:
                case CloseReason.TaskManagerClosing:
                case CloseReason.WindowsShutDown:
                    break;

                default:
                    e.Cancel = true;
                    this.Hide();
                    Tray.ShowBalloonOnMinimize();
                    break;
            }
#endif
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e) {
            Application.Exit();
        }


        private void Form_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.Escape: {
                        this.Close();
                    } break;
                case Keys.F5: {
                        mnuRefresh.PerformClick();
                    } break;
                case Keys.Control | Keys.F: {
                        using (var frm = new FilterForm(treeImages)) {
                            if (frm.ShowDialog(this) == DialogResult.OK) {
                                var node = FindNodeBySession(frm.SelectedSession, tree.Nodes);
                                if (node != null) {
                                    tree.SelectedNode = node;
                                    mnuConnect.PerformClick();
                                }
                            }
                        }
                    } break;
            }
        }


        private void tree_AfterSelect(object sender, TreeViewEventArgs e) {
            var node = e.Node as PuttyNode;

            var isFolder = (node != null) && node.IsFolder;
            var isConnection = (node != null) && node.IsConnection;
            var canConnect = isConnection && node.Session.HasBasicParameters;
            var canModify = isConnection && !node.Session.SessionText.Equals("Default Settings", StringComparison.InvariantCulture);

            mnuConnect.Enabled = canConnect;
        }

        private void tree_KeyDown(object sender, KeyEventArgs e) {
            var node = tree.SelectedNode as PuttyNode;
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Space)) {
                if (node.IsConnection) {
                    mnuConnect.PerformClick();
                } else {
                    if (node.IsExpanded) { node.Collapse(); } else { node.Expand(); }
                }
            }
        }

        private void tree_MouseDown(object sender, MouseEventArgs e) {
            var node = tree.GetNodeAt(e.Location);
            if ((node != null) && !node.Equals(tree.SelectedNode)) { //so selection works for right-click also
                tree.SelectedNode = node;
            }
        }

        private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                var node = tree.SelectedNode as PuttyNode;
                if (node != null) {
                    if (node.IsConnection) {
                        mnuConnect.PerformClick();
                    }
                }
            }
        }


        private void tmrCheckPuttyExecutable_Tick(object sender, EventArgs e) {
            if (Settings.PuttyExecutableExists) {
                staWarningNoPutty.Visible = false;
                tmrCheckPuttyExecutable.Enabled = false; //don't check once you find it first time.
            } else {
                if (!staWarningNoPutty.Visible) { staWarningNoPutty.Visible = true; }
            }
        }


        #region Menu

        private void mnuConnect_Click(object sender, EventArgs e) {
            var node = tree.SelectedNode as PuttyNode;
            if ((node != null) && node.IsConnection) {
                try {
                    if (!Settings.PuttyExecutableExists) {
                        tmrCheckPuttyExecutable.Enabled = true;
                        tmrCheckPuttyExecutable_Tick(null, null);
                        Medo.MessageBox.ShowError(this, "Cannot find PuTTY executable.\n\nPlease configure it.");
                        mnuOptions.PerformClick();
                    }

                    if (Settings.PuttyExecutableExists) {
                        var psi = new ProcessStartInfo(Settings.PuttyExecutable);
                        psi.Arguments = "-load \"" + node.Session.SessionText + "\"";
                        psi.WindowStyle = Settings.PuttyWindowStyle;
                        var process = new Process() { StartInfo = psi };
                        process.Start();
                    }

                    if (Control.ModifierKeys != Keys.Shift) { this.Close(); }
                } catch (InvalidOperationException ex) {
                    Medo.MessageBox.ShowError(this, ex.Message);
                }
            }

        }


        private void mnuRefresh_Click(object sender, EventArgs e) {
            FillNodes();
        }


        private void mnuOptions_Click(object sender, EventArgs e) {
            using (var frm = new OptionsForm()) {
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    tmrCheckPuttyExecutable.Enabled = true;
                    tmrCheckPuttyExecutable_Tick(null, null);
                }
            }
        }


        private void mnuAppFeedback_Click(object sender, EventArgs e) {
            Medo.Diagnostics.ErrorReport.ShowDialog(this, null, new Uri("http://jmedved.com/feedback/"));
        }

        private void mnuAppUpgrade_Click(object sender, EventArgs e) {
            Medo.Services.Upgrade.ShowDialog(this, new Uri("http://jmedved.com/upgrade/"));
        }

        private void mnuAppDonate_Click(object sender, EventArgs e) {
            Process.Start("http://jmedved.com/donate/");
        }

        private void mnuAppAbout_Click(object sender, EventArgs e) {
            Medo.Windows.Forms.AboutBox.ShowDialog(this, new Uri("http://jmedved.com/puttyrun/"));
        }

        #endregion


        #region Fill nodes

        private Dictionary<string, PuttyNode> CachedFolderNodes = new Dictionary<string, PuttyNode>(StringComparer.InvariantCultureIgnoreCase);

        private void FillNodes() {
            tree.Nodes.Clear();
            this.CachedFolderNodes.Clear();

            foreach (var sessionNode in PuttyNode.GetSessionNodes()) {
                GetFolderNode(sessionNode.Session.Folder);
            }
            foreach (var sessionNode in PuttyNode.GetSessionNodes()) {
                var folderNode = GetFolderNode(sessionNode.Session.Folder);
                if (folderNode != null) {
                    folderNode.Nodes.Add(sessionNode);
                } else {
                    tree.Nodes.Add(sessionNode);
                }
            }
            tree.ExpandAll();
            tree.SelectedNode = tree.Nodes[0];
        }

        private PuttyNode GetFolderNode(string folderName) {
            if (string.IsNullOrEmpty(folderName)) { return null; }

            PuttyNode node;
            if (this.CachedFolderNodes.TryGetValue(folderName, out node)) {
                return node;
            } else {
                var parts = folderName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1) {
                    node = new PuttyNode(parts[0]);
                    tree.Nodes.Add(node);
                } else {
                    var parentNode = GetFolderNode(string.Join("\\", parts, 0, parts.Length - 1));
                    node = new PuttyNode(parts[parts.Length - 1]);
                    parentNode.Nodes.Add(node);
                }
                this.CachedFolderNodes.Add(folderName, node);
                return node;
            }
        }

        #endregion


        private PuttyNode FindNodeBySession(PuttySession session, TreeNodeCollection nodes) {
            foreach (PuttyNode node in nodes) {
                if ((node.Session != null) && node.Session.Equals(session)) { return node; }
            }
            return null;
        }

    }
}
