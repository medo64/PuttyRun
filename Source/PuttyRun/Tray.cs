using System;
using System.Windows.Forms;

namespace PuttyRun {
    internal static class Tray {

        private static readonly object SyncRoot = new object();
        private static NotifyIcon Icon = new NotifyIcon();

        public static Form Form { get; private set; }
        public static Medo.Windows.Forms.Hotkey Hotkey { get; private set; }

        public static void Initialize(Form form) {
            Tray.Form = form;

            Icon.Icon = Medo.Resources.ManifestResources.GetIcon("PuttyRun.Properties.App.ico", 16, 16);
            Icon.Text = Medo.Reflection.EntryAssembly.Title;

            Icon.MouseClick += delegate(Object sender, MouseEventArgs e) {
                if (e.Button == MouseButtons.Left) {
                    Tray.ShowForm();
                }
            };

            Icon.ContextMenu = new ContextMenu();
            Icon.ContextMenu.Popup += delegate(Object sender, EventArgs e) {
                var items = Icon.ContextMenu.MenuItems;
                items.Clear();

                var showItem = new MenuItem("&Show application") { DefaultItem = true };
                if (Hotkey.Key != Keys.None) { showItem.Text += "\t" + Helpers.GetKeyString(Hotkey.Key); }
                showItem.Click += delegate(Object sender2, EventArgs e2) {
                    Tray.ShowForm();
                };
                items.Add(showItem);


                items.Add(new MenuItem("-"));

                var exitItem = new MenuItem("E&xit");
                exitItem.Click += delegate(Object sender2, EventArgs e2) {
                    Tray.Hide();
                    Application.Exit();
                };
                items.Add(exitItem);
            };

            Tray.Hotkey = new Medo.Windows.Forms.Hotkey();
        }


        public static void Show() {
            Icon.Visible = true;
        }

        public static void Hide() {
            Icon.Visible = false;
        }


        internal static void ShowForm() {
            lock (Tray.SyncRoot) {
                Tray.Form.Show();
                if (Tray.Form.WindowState == FormWindowState.Minimized) { Tray.Form.WindowState = FormWindowState.Normal; }
                Tray.Form.Activate();
            }
        }


        internal static void ShowBalloonOnMinimize() {
            if (Settings.ShowBalloonOnNextMinimize) {
                Settings.ShowBalloonOnNextMinimize = false;
                var text = "Program continues to run in background.";
                if (Tray.Hotkey.IsRegistered) {
                    text += "\n\nPress " + Helpers.GetKeyString(Tray.Hotkey.Key) + " to show window again.";
                }
                Tray.Icon.ShowBalloonTip(0, "PuTTY Run", text, ToolTipIcon.Info);
            }
        }

    }
}
