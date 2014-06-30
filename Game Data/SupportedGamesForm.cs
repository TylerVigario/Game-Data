using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Game_Data
{
    public partial class SupportedGamesForm : Form
    {
        private static bool open = false;
        //
        AddSupportedGameForm addForm;

        public SupportedGamesForm()
        {
            open = true;
            //
            InitializeComponent();
        }

        public static bool isOpen
        {
            get { return open; }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SupportedGamesForm_Load(object sender, EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.SupportedGames_Window_Geometry, this);
            supportedGamesList.RestoreState(Convert.FromBase64String(Settings.SupportedGamesList_State));
            //
            IniFile ini = new IniFile(Settings.Save_Path + "\\games.ini");
            string[] sections = ini.GetSectionNames();
            List<SupportedGameListItem> SupportedGames = new List<SupportedGameListItem>();
            foreach (string section in sections)
            {
                if (section != "General")
                    SupportedGames.Add(new SupportedGameListItem(ini.GetString(section, "Game_Name", ""), ini.GetString(section, "Process_Name", "")));
            }
            supportedGamesList.AddObjects(SupportedGames);
        }

        public static void checkForUpdate()
        {
            if (!File.Exists(Settings.Save_Path + "\\games.ini"))
            {
                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(downloadSupportedGamesList)).Start(0);
            }
            else
            {
                IniFile ini = new IniFile(Settings.Save_Path + "\\games.ini");
                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(downloadSupportedGamesList)).Start(ini.GetInt32("General", "Version", 0));
            }
        }
           

        private static void downloadSupportedGamesList(object lv)
        {
            try
            {
                int localVersion = (int)lv;
                WebClient client = new WebClient();
                //
                if (localVersion > 0)
                {
                    string response = client.DownloadString("http://updater.logicpwn.com/update.php?app_id=3");
                    Thoughtful_Coding.Update available_update = JsonConvert.DeserializeObject<Thoughtful_Coding.Update>(response);
                    if (localVersion >= Convert.ToInt32(available_update.version)) { return; }
                }
                //
                client.DownloadFile(new Uri("http://updater.logicpwn.com/download.php?app_id=3"), Settings.Save_Path + "\\games.ini");

                
            }
            catch (Exception ex) { if (connectedToInternet()) { Game_Data.Program.ApplicationThreadException(null, new System.Threading.ThreadExceptionEventArgs(ex)); } } 
        }

        private static bool connectedToInternet()
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

        private void SupportedGamesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.SupportedGames_Window_Geometry = WindowGeometry.GeometryToString(this);
            Settings.SupportedGamesList_State = Convert.ToBase64String(supportedGamesList.SaveState());
        }

        private void SupportedGamesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove " + ((supportedGamesList.SelectedObjects.Count > 1) ? supportedGamesList.SelectedObjects.Count.ToString() + " games" : '"' + ((SupportedGameListItem)supportedGamesList.SelectedObject).Game_Name + '"') + " from the supported games list?", "Remove Supported Game", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IniFile ini = new IniFile(Settings.Save_Path + "\\games.ini");
                foreach (SupportedGameListItem item in supportedGamesList.SelectedObjects)
                {
                    ini.DeleteSection(GameDatabase.gameNameSaferizer(item.Game_Name));
                    supportedGamesList.RemoveObject(item);
                    GameWatcher.RemoveSupportedGame(new SupportedGame(item.Game_Name, item.Process_Name));
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (AddSupportedGameForm.isOpen)
            {
                addForm.BringToFront();
            }
            else
            {
                addForm = new AddSupportedGameForm();
                addForm.addGame += addForm_addGame;
                addForm.Show();
            }
        }

        void addForm_addGame(SupportedGame nGame, SupportedGameListItem oItem)
        {
            if (oItem != null)
            {
                IniFile ini = new IniFile(Settings.Save_Path + "\\games.ini");
                ini.DeleteSection(GameDatabase.gameNameSaferizer(oItem.Game_Name));
                supportedGamesList.RemoveObject(oItem);
                // Tell main list to change name
                GameDatabase.RenameGame(oItem.Game_Name, nGame.Game_Name);
            }
            //
            supportedGamesList.AddObject(new SupportedGameListItem(nGame.Game_Name, nGame.Process_Name));
            //
            try
            {
                NameValueCollection data = new NameValueCollection();
                data.Add("Game_Name", nGame.Game_Name);
                data.Add("Process_Name", nGame.Process_Name);
                new WebClient().UploadValues(new Uri("http://updater.logicpwn.com/addGame.php"), data);
            }
            catch (Exception ex) { if (connectedToInternet()) { Game_Data.Program.ApplicationThreadException(this, new System.Threading.ThreadExceptionEventArgs(ex)); } } 
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (supportedGamesList.SelectedObjects.Count > 1) { MessageBox.Show("You can not edit multiple items."); return; }
            SupportedGameListItem item = (SupportedGameListItem)supportedGamesList.SelectedObject;
            AddSupportedGameForm addForm = new AddSupportedGameForm(item);
            addForm.addGame += addForm_addGame;
            addForm.Show();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editButton_Click(this, null);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeButton_Click(this, null);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addButton_Click(this, null);
        }

        private void supportedGamesList_SelectionChanged(object sender, EventArgs e)
        {
            if (supportedGamesList.SelectedObjects.Count > 0)
            {
                removeButton.Enabled = true;
                editButton.Enabled = true;
                removeToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = true;
            }
            else
            {
                removeButton.Enabled = false;
                editButton.Enabled = false;
                removeToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
            }
        }
    }

    #region SupportedGameListItem

    public class SupportedGameListItem
    {
        string _game_name = "";
        string _proc_name = "";

        public SupportedGameListItem(string game_name, string proc_name)
        {
            _game_name = game_name;
            _proc_name = proc_name;
        }

        public string Game_Name
        {
            get { return _game_name; }
            set { _game_name = value; }
        }

        public string Process_Name
        {
            get { return _proc_name; }
            set { _proc_name = value; }
        }
    }

    #endregion
}
