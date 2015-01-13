using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MapleShark
{
    public partial class SearchForm : DockContent
    {
    	public static SearchForm Instance { get; private set; }
        public SearchForm()
        {
            InitializeComponent();
        	Instance = this;
        }

        public MainForm MainForm { get { return ParentForm as MainForm; } }


        private void mNextOpcodeButton_Click(object pSender, EventArgs pArgs)
        {
            bool headerDefined = HeaderBox.Text != "";
            byte header = 0;
            if(headerDefined)
                header = byte.Parse(HeaderBox.Text);
            bool typeDefined = Typebox.Text != "";
            byte type = 0;
            if(typeDefined)
                type = byte.Parse(Typebox.Text);

            SessionForm session = DockPanel.ActiveDocument as SessionForm;
            if (session == null) return;
            session.FilterOut = (packet) =>
                            {
                                bool ret = false;
                                if (headerDefined && packet.Header != header)
                                    ret = true;
                                if (typeDefined && packet.Type != type)
                                    ret = true;
                                return ret;
                            };
            session.ListView.Focus();
            session.RefreshPackets();

        }

        private void resetFilter_Click(object sender, EventArgs e)
        {
            HeaderBox.Text = "";
            Typebox.Text = "";
            mNextOpcodeButton_Click(sender, e);
        }

    }
}
