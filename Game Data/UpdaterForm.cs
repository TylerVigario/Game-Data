using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Thoughtful_Coding
{
    public partial class UpdaterForm : Form
    {
        private int app_id;

        public UpdaterForm(int _app_id)
        {
            InitializeComponent();
            //
            app_id = _app_id;
        }

        private void UpdaterForm_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " - Updater";
            //
            richTextBox1.SelectionStart = 0;
            //
            WebClient App_Updater = new WebClient();
            App_Updater.DownloadProgressChanged += App_Updater_DownloadProgressChanged;
            App_Updater.DownloadFileCompleted += App_Updater_DownloadFileCompleted;
            try
            {
                App_Updater.DownloadFileAsync(new Uri("http://updater.logicpwn.com/download.php?app_id=" + app_id.ToString()), Application.StartupPath + "\\update_installer.exe");
            }
            catch (Exception ex) { Game_Data.Program.ApplicationThreadException(this, new System.Threading.ThreadExceptionEventArgs(ex)); }
        }

        private void UpdaterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\update_installer.exe");
                Environment.Exit(0);
            }
            catch (Exception ex) { Game_Data.Program.ApplicationThreadException(this, new System.Threading.ThreadExceptionEventArgs(ex)); }
        }

        public void checkForUpdate()
        {
            try
            {
                WebClient client = new WebClient();
                string response = client.DownloadString("http://updater.logicpwn.com/update.php?app_id=" + app_id.ToString() + "&action=available_updates&local_version=" + Application.ProductVersion);
                List<Update> available_updates = JsonConvert.DeserializeObject<List<Update>>(response);
                if (available_updates.Count > 0)
                {
                    string message;
                    if (available_updates.Count > 1) { message = "There is " + available_updates.Count.ToString() + " updates available for this application. Would you like to update now?"; }
                    else { message = "There is one update available for this application. Would you like to update now?"; }
                    if (MessageBox.Show(message, Application.ProductName + " - Updater", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string changeLog = "";
                        foreach (Update available_update in available_updates) { changeLog += available_update.changes + "\r\n\r\n"; }
                        richTextBox1.Text = changeLog;
                        this.Show();
                    }
                }
            }
            catch (Exception ex) { if (connectedToInternet()) { Game_Data.Program.ApplicationThreadException(this, new System.Threading.ThreadExceptionEventArgs(ex)); } } 
        }

        private bool connectedToInternet()
        {
            try
            {
                IPAddress[] addresslist = Dns.GetHostAddresses("http://www.google.com");
                //
                if (addresslist[0].ToString().Length > 6)
                {
                    return true;
                }
                else { return false; }
            }
            catch { return false; }
        }

        #region App_Updater Events

        private delegate void DPD(object sender, DownloadProgressChangedEventArgs e);

        private void App_Updater_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //if (InvokeRequired) { BeginInvoke(new DPD(App_Updater_DownloadProgressChanged), new[] { sender, e }); return; }
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = e.ProgressPercentage.ToString() + "%";
        }

        private delegate void DFD(object sender, AsyncCompletedEventArgs e);

        private void App_Updater_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //if (InvokeRequired) { BeginInvoke(new DFD(App_Updater_DownloadFileCompleted), new[] { sender, e }); return; }
            button1.Text = "Install";
            button1.Enabled = true;
        }

        #endregion
    }

    public class Update
    {
        private string _id;
        private string _app_id;
        private string _version;
        private string _changes;
        private string _post_date;
        private string _download_count;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string app_id
        {
            get { return _app_id; }
            set { _app_id = value; }
        }

        public string version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string changes
        {
            get { return _changes; }
            set { _changes = value; }
        }

        public string post_date
        {
            get { return _post_date; }
            set { _post_date = value; }
        }

        public string download_count
        {
            get { return _download_count; }
            set { _download_count = value; }
        }
    }
}