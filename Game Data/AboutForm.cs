using System;
using System.Windows.Forms;

namespace Game_Data
{
    public partial class AboutForm : Form
    {
        private static bool open = false;

        public AboutForm()
        {
            open = true;
            //
            InitializeComponent();
        }

        public static bool isOpen
        {
            get { return open; }
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.Text = "About Game Data v" + Application.ProductVersion + " beta";
        }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://logicpwn.pcriot.com");
        }
    }
}
