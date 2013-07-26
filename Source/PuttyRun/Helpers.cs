using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PuttyRun {
    internal static class Helpers {

        public static string GetKeyString(Keys keyData) {
            if ((keyData & Keys.LWin) == Keys.LWin) { return string.Empty; }
            if ((keyData & Keys.RWin) == Keys.RWin) { return string.Empty; }

            var sb = new System.Text.StringBuilder();
            if ((keyData & Keys.Control) == Keys.Control) {
                if (sb.Length > 0) { sb.Append("+"); }
                sb.Append("Ctrl");
                keyData = keyData ^ Keys.Control;
            }

            if ((keyData & Keys.Alt) == Keys.Alt) {
                if (sb.Length > 0) { sb.Append("+"); }
                sb.Append("Alt");
                keyData = keyData ^ Keys.Alt;
            }

            if ((keyData & Keys.Shift) == Keys.Shift) {
                if (sb.Length > 0) { sb.Append("+"); }
                sb.Append("Shift");
                keyData = keyData ^ Keys.Shift;
            }

            switch (keyData) {
                case 0: return string.Empty;
                case Keys.ControlKey: return string.Empty;
                case Keys.Menu: return string.Empty;
                case Keys.ShiftKey: return string.Empty;
                default:
                    if (sb.Length > 0) { sb.Append("+"); }
                    sb.Append(keyData.ToString());
                    return sb.ToString();
            }
        }


    }
}
