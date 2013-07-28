using System;
using System.Collections;
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

        public class NodeSorter : IComparer {
            public int Compare(object item1, object item2) {
                var node1 = item1 as PuttyNode;
                var node2 = item2 as PuttyNode;

                if (node1.IsFolder == node2.IsFolder) {
                    if (node1.IsDefaultConnection) { return -1; }
                    if (node2.IsDefaultConnection) { return +1; }
                    return string.Compare(node1.Name, node2.Name, StringComparison.CurrentCulture);
                } else {
                    return node1.IsFolder ? -1 : +1;
                }
            }
        }

    }
}
