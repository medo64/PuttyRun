using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PuttyRun {
    internal class PuttySession {

        private PuttySession(string sessionName) {
            this.FullSessionName = sessionName;
        }


        #region Basic properties

        private String _FullSessionName;
        public String FullSessionName {
            get { return this._FullSessionName; }
            private set {
                if (this._FullSessionName != null) { //rename
                    RenameRegistrySession(this._FullSessionName, value);
                }
                this._FullSessionName = value;
            }
        }

        public string FullSessionText {
            get { return DecodeText(this.FullSessionName); }
            set {
                if (this.IsDefault) { throw new InvalidOperationException("Cannot rename Default Settings."); }
                Debug.WriteLine("V: Session rename: '" + this.FullSessionText + "' => '" + value + "'.");
                this.FullSessionName = EncodeText(value);
            }
        }

        public Boolean IsDefault { get { return this.FullSessionText.Equals("Default Settings", StringComparison.Ordinal); } }


        public String Folder {
            get {
                string folder, name;
                ExtractFolderAndName(this.FullSessionName, out folder, out name);
                return folder;
            }
        }

        public String Name {
            get {
                string folder, name;
                ExtractFolderAndName(this.FullSessionName, out folder, out name);
                return name;
            }
        }


        #region Extract

        private static readonly Encoding SystemCodePage = Encoding.GetEncoding(0);

        private static string DecodeText(string sessionName) {
            var decodedBytes = new List<byte>();

            var encodedByteValue = (byte)0;
            var state = ExtractFolderAndNameState.CopyByte;
            foreach (var b in SystemCodePage.GetBytes(sessionName)) {
                switch (state) {
                    case ExtractFolderAndNameState.CopyByte: {
                            if (b == 0x25) { //%
                                state = ExtractFolderAndNameState.EncodedByte1;
                            } else {
                                decodedBytes.Add(b);
                            }
                        } break;

                    case ExtractFolderAndNameState.EncodedByte1: {
                            encodedByteValue = byte.Parse(Convert.ToChar(b).ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            state = ExtractFolderAndNameState.EncodedByte2;
                        } break;

                    case ExtractFolderAndNameState.EncodedByte2: {
                            encodedByteValue = (byte)(encodedByteValue * 16 + +byte.Parse(Convert.ToChar(b).ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                            decodedBytes.Add(encodedByteValue);
                            state = ExtractFolderAndNameState.CopyByte;
                        } break;

                }
            }

            return SystemCodePage.GetString(decodedBytes.ToArray());
        }

        private static string EncodeText(string sessionText) { //PuTTY's WINSTORE.C
            var sb = new StringBuilder();

            foreach (byte b in SystemCodePage.GetBytes(sessionText)) {
                switch (b) {
                    case 0x20: //' '
                    case 0x5C: //'\'
                    case 0x2A: //'*'
                    case 0x3F: //'?'
                    case 0x25: //'%'
                        sb.Append("%");
                        sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
                        break;

                    default:
                        if ((b < 0x20) || (b > 0x7E) //too low or too high
                         || ((sb.Length == 0) && (b == 0x2E))) { //first dot (.)
                            sb.Append("%");
                            sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
                        } else {
                            sb.Append(Convert.ToChar(b));
                        }
                        break;
                }
            }

            return sb.ToString();
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
            CopyByte, EncodedByte1, EncodedByte2
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
            using (var root = Registry.CurrentUser.OpenSubKey(RegistrySessionRoot + "\\" + this.FullSessionName)) {
                if (root != null) {
                    return root.GetValue(valueName) as string;
                } else {
                    return null;
                }
            }
        }

        private void SetRegistryString(string valueName, string value) {
            using (var root = Registry.CurrentUser.OpenSubKey(RegistrySessionRoot + "\\" + this.FullSessionName, true)) {
                if (root != null) {
                    root.DeleteValue(valueName, false);
                    root.SetValue(valueName, value);
                } else {
                    throw new InvalidOperationException("Cannot find registry key.");
                }
            }
        }


        private void RenameRegistrySession(string oldSessionName, string newSessionName) {
            if (string.Equals(oldSessionName, newSessionName, StringComparison.Ordinal)) { //ignore same name
                return;
            } else if (string.Equals(oldSessionName, newSessionName, StringComparison.OrdinalIgnoreCase)) { //use temp key for case changes
                RenameRegistrySession(oldSessionName, oldSessionName + " TEMP");
                oldSessionName += " TEMP";
            }

            using (var sessionKey = Registry.CurrentUser.OpenSubKey(RegistrySessionRoot, true)) {
                using (var oldKey = sessionKey.OpenSubKey(oldSessionName))
                using (var newKey = sessionKey.CreateSubKey(newSessionName)) {
                    foreach (var valueName in oldKey.GetValueNames()) {
                        var valueKind = oldKey.GetValueKind(valueName);
                        var value = oldKey.GetValue(valueName);
                        newKey.SetValue(valueName, value, valueKind);
                    }
                }
                sessionKey.DeleteSubKey(oldSessionName);
            }
        }

        #endregion


        #region Overrides

        public override int GetHashCode() {
            return this.FullSessionName.GetHashCode();
        }

        public override bool Equals(object obj) {
            var other = obj as PuttySession;
            return (other != null) && (this.FullSessionName.Equals(other.FullSessionName));
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
