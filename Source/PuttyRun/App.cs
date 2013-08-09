using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace PuttyRun {
    static class App {

        [STAThread]
        static void Main() {
            bool createdNew;
            var mutexSecurity = new MutexSecurity();
            mutexSecurity.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow));
            using (var setupMutex = new Mutex(false, @"Global\JosipMedved_PuttyRun", out createdNew, mutexSecurity)) {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Medo.Configuration.Settings.Read("CultureName", "en-US"));

                Medo.Application.UnhandledCatch.ThreadException += new System.EventHandler<ThreadExceptionEventArgs>(UnhandledException);
                Medo.Application.UnhandledCatch.Attach();

                Medo.Configuration.Settings.NoRegistryWrites = Settings.NoRegistryWrites;
                Medo.Diagnostics.ErrorReport.DisableAutomaticSaveToTemp = Settings.NoRegistryWrites;
                Medo.Windows.Forms.State.NoRegistryWrites = Settings.NoRegistryWrites;

                Application.ApplicationExit += delegate(Object sender, EventArgs e) {
                    Tray.Hide();
                    Environment.Exit(0);
                };

                Tray.Initialize(new MainForm());

                Medo.Application.SingleInstance.NewInstanceDetected += delegate(Object sender, Medo.Application.NewInstanceEventArgs e) {
                    try {
                        if (Tray.Form.IsHandleCreated == false) {
                            Tray.Form.CreateControl();
                            Tray.Form.Handle.GetType();
                        }

                        NewInstanceDetectedProcDelegate method = new NewInstanceDetectedProcDelegate(NewInstanceDetectedProc);
                        Tray.Form.Invoke(method);
                    } catch (Exception) { }
                };
                if (Medo.Application.SingleInstance.IsOtherInstanceRunning) {
                    var currProcess = Process.GetCurrentProcess();
                    foreach (var iProcess in Process.GetProcessesByName(currProcess.ProcessName)) {
                        try {
                            if (iProcess.Id != currProcess.Id) {
                                NativeMethods.AllowSetForegroundWindow(iProcess.Id);
                                break;
                            }
                        } catch (Win32Exception) { }
                    }
                }
                Medo.Application.SingleInstance.Attach();

                Tray.Hotkey.HotkeyActivated += delegate(Object sender, EventArgs e) {
                    NewInstanceDetectedProc();
                };
                if (Settings.ActivationHotkey != Keys.None) {
                    try {
                        Tray.Hotkey.Register(Settings.ActivationHotkey);
                    } catch (InvalidOperationException) {
                        Medo.MessageBox.ShowWarning(null, "Hotkey " + Helpers.GetKeyString(Settings.ActivationHotkey) + " is already in use.");
                    }
                }

                Tray.Show();

                if (Medo.Application.Args.Current.ContainsKey("hide") == false) {
                    Tray.ShowForm();
                }

                Application.Run();
            }
        }


        private static void UnhandledException(object sender, ThreadExceptionEventArgs e) {
#if !DEBUG
            Medo.Diagnostics.ErrorReport.ShowDialog(null, e.Exception, new Uri("http://jmedved.com/feedback/"));
#else
            throw e.Exception;
#endif
        }


        private delegate void NewInstanceDetectedProcDelegate();
        private static void NewInstanceDetectedProc() {
            Tray.Show();
            Tray.ShowForm();
        }


        private static class NativeMethods {

            [DllImportAttribute("user32.dll", EntryPoint = "AllowSetForegroundWindow")]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern bool AllowSetForegroundWindow(int dwProcessId);

        }

    }
}
