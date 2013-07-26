using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PuttyRun {
    internal class PuttyNode : TreeNode {

        private PuttyNode(PuttySession session) {
            this.Session = session;

            base.Text = session.Name;
            this.ImageIndex = (int)session.ConnectionType;
            this.SelectedImageIndex = this.ImageIndex;
        }

        public PuttyNode(String folderName) {
            this.Session = null;

            base.Text = folderName;
            this.ImageIndex = 6;
            this.SelectedImageIndex = this.ImageIndex;
        }


        public Boolean IsConnection { get { return (this.Session != null); } }
        public Boolean IsFolder { get { return (this.Session == null); } }

        public PuttySession Session { get; private set; }


        public override int GetHashCode() {
            return this.Session.SessionName.GetHashCode();
        }

        public override bool Equals(object obj) {
            var other = obj as PuttyNode;
            return (other != null) && (this == other);
        }

        public override string ToString() {
            return this.Text;
        }


        public static IEnumerable<PuttyNode> GetSessionNodes() {
            foreach (var session in PuttySession.GetSessions()) {
                yield return new PuttyNode(session);
            }
        }

    }
}
