using System;
using WeifenLuo.WinFormsUI.Docking;

namespace MapleShark {
	public partial class OutputForm : DockContent
    {
        public OutputForm(string pTitle)
        {
            InitializeComponent();
            Text = pTitle;
        }

        public void Append(string pOutput) { mTextBox.AppendText(pOutput); }

        private void mTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
