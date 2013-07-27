using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace PuttyRun {
    internal partial class FilterForm : Form {
        public FilterForm(ImageList imageList, bool allowNumber) {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            if (allowNumber) {
                this.Text = "Goto";
                list.Columns[0].Width = list.ClientSize.Width - list.Columns[1].Width - SystemInformation.VerticalScrollBarWidth;
            } else {
                this.Text = "Find";
                list.Columns.RemoveAt(1);
                list.Columns[0].Width = list.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            list.SmallImageList = imageList;

            this.AllowNumber = allowNumber;
        }

        private readonly bool AllowNumber;


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

                int filterIndex;
                if (!this.AllowNumber || !int.TryParse(filter, NumberStyles.Integer, CultureInfo.CurrentCulture, out filterIndex)) { filterIndex = -1; }

                var index = 0;
                foreach (var session in PuttySession.GetSessions()) {
                    index += 1;

                    bool showSession;
                    if (filterIndex == -1) { //check the name
                        showSession = (session.Name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    } else { //check the number
                        showSession = (filterIndex == index);
                    }

                    if (showSession) {
                        var lvi = new ListViewItem(session.Name, (int)session.ConnectionType) { Tag = session };
                        if (this.AllowNumber) {
                            lvi.SubItems.Add(index.ToString());
                            lvi.UseItemStyleForSubItems = false;
                            lvi.SubItems[1].ForeColor = SystemColors.GrayText;
                        }
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
