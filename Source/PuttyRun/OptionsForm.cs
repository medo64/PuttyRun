using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PuttyRun {
    internal partial class OptionsForm : Form {
        public OptionsForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            btnPuttyExeBrowse.Font = new Font(SystemFonts.MessageBoxFont.Name, SystemFonts.MessageBoxFont.SizeInPoints / 2, FontStyle.Italic);
        }


        private void Form_Load(object sender, System.EventArgs e) {
            if (Settings.NoRegistryWrites) {
                SetControlState(false);
                btnAllowRegistry.Visible = true;
            }

            if (Tray.Hotkey.IsRegistered) { Tray.Hotkey.Unregister(); }

            chbRunOnStartup.Checked = Settings.RunOnStartup;

            txtHotkey.Text = (Settings.ActivationHotkey != Keys.None) ? Helpers.GetKeyString(Settings.ActivationHotkey) : "";
            txtHotkey.Tag = Settings.ActivationHotkey;

            txtPuttyExe.Text = Settings.PuttyExecutable;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e) {
            if (Settings.ActivationHotkey != Keys.None) {
                try {
                    Tray.Hotkey.Register(Settings.ActivationHotkey);
                } catch (InvalidOperationException) {
                    Medo.MessageBox.ShowWarning(null, "Hotkey " + Helpers.GetKeyString(Settings.ActivationHotkey) + " is already in use.");
                }
            }
        }


        private void btnOK_Click(object sender, System.EventArgs e) {
            if (Settings.RunOnStartup != chbRunOnStartup.Checked) {
                Settings.RunOnStartup = chbRunOnStartup.Checked;
            }

            Keys newKey = (Keys)txtHotkey.Tag;
            if (Settings.ActivationHotkey != newKey) {
                Settings.ActivationHotkey = newKey;
            }

            txtPuttyExe.Text = txtPuttyExe.Text.Trim();
            if (!string.Equals(Settings.PuttyExecutable, txtPuttyExe.Text, System.StringComparison.Ordinal)) {
                Settings.PuttyExecutable = txtPuttyExe.Text;
            }
        }

        private void btnAllowRegistry_Click(object sender, EventArgs e) {
            if (Medo.MessageBox.ShowQuestion(this, "Do you allow this program use of registry in order to save its settings?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                btnAllowRegistry.Visible = false;
                SetControlState(true);
                Settings.Installed = true;
            }
        }


        private void txtHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            var key = e.KeyData;

            if ((key == Keys.Tab) || (key == (Keys.Shift | Keys.Tab))) { return; }

            if (key == Keys.Back) {
                txtHotkey.Tag = Keys.None;
                txtHotkey.Text = Helpers.GetKeyString(key);
            } else {
                var hasControl = ((key & Keys.Control) == Keys.Control);
                var hasAlt = ((key & Keys.Alt) == Keys.Alt);
                var hasShift = ((key & Keys.Shift) == Keys.Shift);
                Debug.WriteLine("V: " + Helpers.GetKeyString(key));
                if ((hasControl && hasAlt) || (hasControl && hasShift) || (hasAlt && hasShift)) {
                    var keyText = Helpers.GetKeyString(key);
                    if (keyText.Length > 0) {
                        txtHotkey.Tag = key;
                        txtHotkey.Text = keyText;
                    }
                } else {
                    txtHotkey.Text = "(use Ctrl+Alt, Ctrl+Shift, or Alt+Shift)";
                }
            }
        }

        private void txtHotkey_Leave(object sender, System.EventArgs e) {
            var key = (Keys)txtHotkey.Tag;
            txtHotkey.Text = Helpers.GetKeyString(key);
        }



        private void SetControlState(bool newState) {
            chbRunOnStartup.Enabled = newState;
            lblHotkey.Enabled = newState;
            txtHotkey.Enabled = newState;
            lblPuttyExe.Enabled = newState;
            txtPuttyExe.Enabled = newState;
            btnPuttyExeBrowse.Enabled = newState;
            btnOK.Enabled = newState;
        }

    }
}
