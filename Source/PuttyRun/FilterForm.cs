using System;
using System.Drawing;
using System.Windows.Forms;

namespace PuttyRun {
    internal partial class FilterForm : Form {
        public FilterForm(ImageList imageList) {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            list.Columns[0].Width = list.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            list.SmallImageList = imageList;
        }


        private void txtFilter_KeyDown(object sender, KeyEventArgs e) {
            var selectedIndex = (list.SelectedIndices.Count > 0) ? list.SelectedIndices[0] : -1;

            switch (e.KeyData) {
                case Keys.Down: {
                        if ((selectedIndex != -1) && (selectedIndex < list.Items.Count - 1)) {
                            list.Items[selectedIndex + 1].Selected = true;
                        }
                        e.Handled = true;
                    } break;

                case Keys.Up: {
                        if ((selectedIndex != -1) && (selectedIndex > 0)) {
                            list.Items[selectedIndex - 1].Selected = true;
                        }
                        e.Handled = true;
                    } break;

                case Keys.Enter: {
                        btnSelect.PerformClick();
                    } break;
            }
        }

        private void txtFilter_TextChanged(object sender, System.EventArgs e) {
            btnSelect.Enabled = false;
            list.BeginUpdate();
            list.Items.Clear();

            var filter = txtFilter.Text.Trim();
            if (filter.Length > 0) {
                foreach (var session in PuttySession.GetSessions()) {
                    var name = session.Name;
                    if (name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0) {
                        var lvi = new ListViewItem(name, (int)session.ConnectionType) { Tag = session };
                        list.Items.Add(lvi);
                    }
                }
            }
            list.EndUpdate();
            if (list.Items.Count > 0) { list.Items[0].Selected = true; }
        }

        private void list_ItemActivate(object sender, EventArgs e) {
            btnSelect.PerformClick();
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e) {
            var selectedSession = (list.SelectedItems.Count > 0) ? (PuttySession)(list.SelectedItems[0].Tag) : null;
            btnSelect.Enabled = (selectedSession != null) && selectedSession.HasBasicParameters;
        }


        public PuttySession SelectedSession { get; private set; }

        private void btnSelect_Click(object sender, EventArgs e) {
            var selectedSession = (list.SelectedItems.Count > 0) ? (PuttySession)(list.SelectedItems[0].Tag) : null;
            if (selectedSession != null) {
                this.SelectedSession = selectedSession;
            }
        }

    }
}
