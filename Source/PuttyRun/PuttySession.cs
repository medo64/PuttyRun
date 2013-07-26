using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PuttyRun {
    internal class PuttySession {

        private PuttySession(string sessionName) {
            this.SessionName = sessionName;
        }


        #region Basic properties

        public String SessionName { get; set; }

        public string SessionText {
            get {
                return DecodeText(this.SessionName);
            }
        }

        public String Folder {
            get {
                string folder, name;
                ExtractFolderAndName(this.SessionName, out folder, out name);
                return folder;
            }
        }

        public String Name {
            get {
                string folder, name;
                ExtractFolderAndName(this.SessionName, out folder, out name);
                return name;
            }
        }

        #region Extract

        private static string DecodeText(string sessionName) {
            var sbSessionName = new StringBuilder();
            var encodedCharValue = (byte)0;
            var state = ExtractFolderAndNameState.CopyChar;
            foreach (var ch in sessionName) {
                switch (state) {
                    case ExtractFolderAndNameState.CopyChar: {
                            if (ch == '%') {
                                state = ExtractFolderAndNameState.EncodedChar1;
                            } else {
                                sbSessionName.Append(ch);
                            }
                        } break;

                    case ExtractFolderAndNameState.EncodedChar1: {
                            encodedCharValue = byte.Parse(ch.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            state = ExtractFolderAndNameState.EncodedChar2;
                        } break;

                    case ExtractFolderAndNameState.EncodedChar2: {
                            encodedCharValue = (byte)(encodedCharValue * 16 + +byte.Parse(ch.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                            sbSessionName.Append(ASCIIEncoding.ASCII.GetString(new byte[] { encodedCharValue }));
                            state = ExtractFolderAndNameState.CopyChar;
                        } break;

                }
            }

            return sbSessionName.ToString();
        }

        private static void ExtractFolderAndName(string sessionName, out string folder, out string name) {
            var decodedSessionName = DecodeText(sessionName);

            var parts = decodedSessionName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0) {
                folder = string.Join("\\", parts, 0, parts.Length - 1);
                name = parts[parts.Length - 1];
            } else {
                folder = "";
                name = "\\";
            }
        }

        private enum ExtractFolderAndNameState {
            CopyChar, EncodedChar1, EncodedChar2
        }

        #endregion

        #endregion

        #region Helper properties

        public Boolean HasBasicParameters {
            get {
                if (this.ConnectionType == PuttyConnectionType.Serial) {
                    return !string.IsNullOrEmpty(this.SerialLine);
                } else {
                    return !string.IsNullOrEmpty(this.HostName);
                }
            }
        }

        #endregion


        #region Category: Session

        public String HostName {
            get { return GetRegistryString("HostName"); }
            set { SetRegistryString("HostName", value); }
        }

        public String SerialLine {
            get { return GetRegistryString("SerialLine"); }
            set { SetRegistryString("SerialLine", value); }
        }

        public PuttyConnectionType ConnectionType {
            get {
                var protocol = GetRegistryString("Protocol");
                if (protocol != null) {
                    switch (protocol.ToUpperInvariant()) {
                        case "RAW": return PuttyConnectionType.Raw;
                        case "TELNET": return PuttyConnectionType.Telnet;
                        case "RLOGIN": return PuttyConnectionType.RLogin;
                        case "SSH": return PuttyConnectionType.Ssh;
                        case "SERIAL": return PuttyConnectionType.Serial;
                    }
                }
                return PuttyConnectionType.Unknown;
            }
            set {
                switch (value) {
                    case PuttyConnectionType.Raw: SetRegistryString("Protocol", "raw"); break;
                    case PuttyConnectionType.Telnet: SetRegistryString("Protocol", "telnet"); break;
                    case PuttyConnectionType.RLogin: SetRegistryString("Protocol", "rlogin"); break;
                    case PuttyConnectionType.Ssh: SetRegistryString("Protocol", "ssh"); break;
                    case PuttyConnectionType.Serial: SetRegistryString("Protocol", "serial"); break;
                    default: throw new ArgumentOutOfRangeException("value", "Unrecognized connection type.");
                }
            }
        }

        #endregion


        #region Registry

        private static readonly String RegistrySessionRoot = @"Software\SimonTatham\PuTTY\Sessions";

        private string GetRegistryString(string valueName) {
            using (var root = Registry.CurrentUser.OpenSubKey(RegistrySessionRoot + "\\" + this.SessionName)) {
                if (root != null) {
                    return root.GetValue(valueName) as string;
                } else {
                    return null;
                }
            }
        }

        private void SetRegistryString(string valueName, string value) {
            using (var root = Registry.CurrentUser.OpenSubKey(RegistrySessionRoot + "\\" + this.SessionName, true)) {
                if (root != null) {
                    root.DeleteValue(valueName, false);
                    root.SetValue(valueName, value);
                } else {
                    throw new InvalidOperationException("Cannot find registry key.");
                }
            }
        }

        #endregion


        #region Overrides

        public override int GetHashCode() {
            return this.SessionName.GetHashCode();
        }

        public override bool Equals(object obj) {
            var other = obj as PuttySession;
            return (other != null) && (this.SessionName.Equals(other.SessionName));
        }

        public override string ToString() {
            return this.Name;
        }

        #endregion


        #region Static

        public static IEnumerable<PuttySession> GetSessions() {
            var sessionNames = new List<String>();
            using (var root = Registry.CurrentUser.OpenSubKey(RegistrySessionRoot)) {
                if (root != null) {
                    foreach (var sessionName in root.GetSubKeyNames()) {
                        sessionNames.Add(sessionName);
                    }
                }
            }
            sessionNames.Sort();
            foreach (var sessionName in sessionNames) {
                yield return new PuttySession(sessionName);
            }
        }

        #endregion

    }
}
