using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PuttyRun {
    internal static class Settings {

        public static Boolean Installed {
            get { return Medo.Configuration.Settings.Read("Installed", false); }
        }

        public static Boolean NoRegistryWrites { get { return !Settings.Installed; } }


        public static Keys ActivationHotkey {
            get { return (Keys)Medo.Configuration.Settings.Read("ActivationHotkey", Convert.ToInt32(Keys.Control | Keys.Shift | Keys.P)); }
            set { Medo.Configuration.Settings.Write("ActivationHotkey", (int)value); }
        }


        public static Boolean ShowBalloonOnNextMinimize {
            get { return Medo.Configuration.Settings.Read("ShowBalloonOnNextMinimize", true); }
            set { Medo.Configuration.Settings.Write("ShowBalloonOnNextMinimize", value); }
        }


        private static readonly Medo.Configuration.RunOnStartup RunOnStartupConfig = new Medo.Configuration.RunOnStartup(Medo.Configuration.RunOnStartup.Current.Title, Medo.Configuration.RunOnStartup.Current.ExecutablePath, "/hide");

        public static Boolean RunOnStartup {
            get { return RunOnStartupConfig.RunForCurrentUser; }
            set {
                try {
                    RunOnStartupConfig.RunForCurrentUser = value;
                } catch (Exception) { }
            }
        }


        public static String PuttyExecutable {
            get {
                var savedExe = Medo.Configuration.Settings.Read("PuttyExecutable", null);
                if (savedExe != null) { return savedExe; }

                var foundExe = new FileInfo("putty.exe");
                if (foundExe.Exists) { return foundExe.FullName; }

                var pfExe = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"PuTTY\putty.exe"));
                if (pfExe.Exists) { return pfExe.FullName; }

                var env32Value = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                if (env32Value != null) {
                    var env32Exe = new FileInfo(Path.Combine(env32Value, @"PuTTY\putty.exe"));
                    if (env32Exe.Exists) { return env32Exe.FullName; }
                }

                var env64Value = Environment.GetEnvironmentVariable("ProgramFiles");
                if (env64Value != null) {
                    var env64Exe = new FileInfo(Path.Combine(env64Value, @"PuTTY\putty.exe"));
                    if (env64Exe.Exists) { return env64Exe.FullName; }
                }

                return null;
            }
            set {
                Medo.Configuration.Settings.Write("PuttyExecutable", value);
            }
        }

        public static Boolean PuttyExecutableExists {
            get { return File.Exists(Settings.PuttyExecutable); }
        }

        public static ProcessWindowStyle PuttyWindowStyle {
            get {
                var windowStyleInt = Medo.Configuration.Settings.Read("PuttyWindowStyle", (int)ProcessWindowStyle.Maximized);
                if (windowStyleInt == (int)ProcessWindowStyle.Normal) {
                    return ProcessWindowStyle.Normal;
                } else {
                    return ProcessWindowStyle.Maximized;
                }
            }
            set {
                Medo.Configuration.Settings.Write("PuttyWindowStyle", (int)value);
            }
        }
    }
}
