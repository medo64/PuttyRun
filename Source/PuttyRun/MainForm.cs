using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PuttyRun {
    internal partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            mnu.Renderer = new ToolStripBorderlessProfessionalRenderer();

            tree.TreeViewNodeSorter = new Helpers.NodeSorter();

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

                case Keys.F2: {
                        mnuRename.PerformClick();
                    } break;

                case Keys.F5: {
                        mnuRefresh.PerformClick();
                    } break;

                case Keys.Control | Keys.F: {
                        Find(false);
                    } break;

                case Keys.Control | Keys.G: {
                        Find(true);
                    } break;
            }
        }


        private void tree_AfterSelect(object sender, TreeViewEventArgs e) {
            var node = e.Node as PuttyNode;

            var isFolder = (node != null) && node.IsFolder;
            var isConnection = (node != null) && node.IsConnection;
            var isDefaultConnection = (node != null) && node.IsDefaultConnection;
            var canConnect = isConnection && node.Session.HasBasicParameters;

            mnuConnect.Enabled = canConnect;
            mnuRename.Enabled = !isDefaultConnection;
        }

        private void tree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) {
            var node = e.Node as PuttyNode;
            e.CancelEdit = node.IsDefaultConnection;
        }

        private void tree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
            if (e.Label == null) { return; } //nothing has changed
            e.CancelEdit = true; //because we will change name ourself

            var newText = e.Label.Trim();
            if (newText.Length == 0) { return; } //don't allow empty names

            var currNode = e.Node as PuttyNode;
            if (currNode != null) {
                var nodes = (currNode.Parent == null) ? tree.Nodes : currNode.Parent.Nodes;
                foreach (PuttyNode node in nodes) {
                    if ((node != currNode) && string.Equals(newText, node.Text, StringComparison.Ordinal)) {
                        return; //name already exists
                    }
                }
            }

            //rename all kids
            currNode.Text = newText;
            RenameNodeAndKids(currNode);
            tree.BeginInvoke(new Action<PuttyNode>(delegate(PuttyNode node) {
                tree.BeginUpdate();
                tree.Sort();
                tree.SelectedNode = node;
                tree.EndUpdate();
            }), currNode);
        }


        private void tree_ItemDrag(object sender, ItemDragEventArgs e) {
            var node = e.Item as PuttyNode;
            Debug.WriteLine("V: MainForm Tree ItemDrag: '" + node.Text + "'.");
            if (!node.IsDefaultConnection) {
                this.DoDragDrop(node, DragDropEffects.Move);
            }
        }

        private void tree_DragOver(object sender, DragEventArgs e) {
            var fromNode = e.Data.GetData("PuttyRun.PuttyNode") as PuttyNode;
            if (fromNode == null) { return; }

            var dropNode = tree.GetNodeAt(tree.PointToClient(new Point(e.X, e.Y))) as PuttyNode;
            while ((dropNode != null) && !dropNode.IsFolder) { dropNode = dropNode.Parent as PuttyNode; }

            Debug.WriteLine("V: MainForm Tree DragOver: '" + fromNode.Text + "' -> '" + ((dropNode != null) ? dropNode.Text : "") + "'.");

            var noCommonParent = (fromNode.Parent != dropNode);
            while (noCommonParent && (dropNode != null)) {
                if (fromNode == dropNode) { noCommonParent = false; } //to avoid loops
                dropNode = dropNode.Parent as PuttyNode;
            }

            e.Effect = noCommonParent ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void tree_DragDrop(object sender, DragEventArgs e) {
            var fromNode = e.Data.GetData("PuttyRun.PuttyNode") as PuttyNode;
            var dropNode = tree.GetNodeAt(tree.PointToClient(new Point(e.X, e.Y))) as PuttyNode;
            while ((dropNode != null) && !dropNode.IsFolder) { dropNode = dropNode.Parent as PuttyNode; }

            Debug.WriteLine("V: MainForm Tree DragDrop: '" + fromNode.Text + "' => '" + ((dropNode != null) ? dropNode.Text : "") + "'.");

            var fromParentNodes = (fromNode.Parent != null) ? fromNode.Parent.Nodes : tree.Nodes;
            fromParentNodes.Remove(fromNode);
            if (dropNode == null) {
                tree.Nodes.Add(fromNode);
            } else {
                dropNode.Nodes.Add(fromNode);
            }
            RenameNodeAndKids(fromNode);

            tree.BeginUpdate();
            tree.Sort();
            tree.SelectedNode = fromNode;
            tree.EndUpdate();
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
                        psi.Arguments = "-load \"" + node.Session.FullSessionText + "\"";
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


        private void mnuNewFolder_Click(object sender, EventArgs e) {
            var parentNode = tree.SelectedNode as PuttyNode;
            while (parentNode != null) {
                if (parentNode.IsFolder) { break; }
                parentNode = parentNode.Parent as PuttyNode;
            }

            var baseFolderName = "New folder";
            var folderName = baseFolderName;
            bool isOk;
            var folderIndex = 1;
            var nodes = (parentNode == null) ? tree.Nodes : parentNode.Nodes;
            do {
                isOk = true;
                foreach (PuttyNode node in nodes) {
                    if (string.Equals(folderName, node.Text, StringComparison.InvariantCultureIgnoreCase)) {
                        isOk = false;
                    }
                }
                if (!isOk) {
                    folderIndex += 1;
                    folderName = baseFolderName + " (" + folderIndex.ToString(CultureInfo.CurrentCulture) + ")";
                }
            } while (!isOk);

            var newNode = new PuttyNode(folderName);
            if (parentNode == null) {
                tree.Nodes.Add(newNode);
            } else {
                parentNode.Nodes.Add(newNode);
            }

            if ((parentNode != null) && !parentNode.IsExpanded) { parentNode.Expand(); }
            tree.SelectedNode = newNode;
            newNode.BeginEdit();
        }

        private void mnuRename_Click(object sender, EventArgs e) {
            var node = tree.SelectedNode as PuttyNode;
            if (node != null) { node.BeginEdit(); }
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

            PuttyNode firstNode = null;
            foreach (var sessionNode in PuttyNode.GetSessionNodes()) {
                if (firstNode == null) { firstNode = sessionNode; }
                var folderNode = GetFolderNode(sessionNode.Session.Folder);
                if (folderNode != null) {
                    folderNode.Nodes.Add(sessionNode);
                } else {
                    tree.Nodes.Add(sessionNode);
                }
            }
            tree.ExpandAll();
            tree.SelectedNode = firstNode;
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
                if (node.IsFolder) {
                    var foundNode = FindNodeBySession(session, node.Nodes);
                    if (foundNode != null) { return foundNode; }
                } else {
                    if (node.Session.Equals(session)) { return node; }
                }
            }
            return null;
        }

        private void Find(bool allowNumber) {
            using (var frm = new FilterForm(treeImages, allowNumber)) {
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    var node = FindNodeBySession(frm.SelectedSession, tree.Nodes);
                    if (node != null) {
                        tree.SelectedNode = node;
                    }
                }
            }
        }

        private void RenameNodeAndKids(PuttyNode node) {
            if (node.Session != null) { node.Session.FullSessionText = GetFullPath(node); }
            foreach (PuttyNode item in node.Nodes) {
                RenameNodeAndKids(item);
            }
        }

        private string GetFullPath(PuttyNode node) {
            var sb = new StringBuilder(node.Text);
            while (node.Parent != null) {
                node = node.Parent as PuttyNode;
                sb.Insert(0, node.Text + @"\");
            }
            return sb.ToString();
        }

    }
}
