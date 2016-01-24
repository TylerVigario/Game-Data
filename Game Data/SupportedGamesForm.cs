using CrashReporterDotNET;
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Windows.Forms;

namespace Game_Data
{
    public partial class SupportedGamesForm : Form
    {
        private static bool open = false;
        //
        AddSupportedGameForm addForm;

        private static void ReportCrash(Exception exception)
        {
            var reportCrash = new ReportCrash
            {
                ToEmail = "TylerVigario90@gmail.com"
            };
            reportCrash.Send(exception);
        }

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
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(Settings.Save_Path + "\\games.ini");
            List<SupportedGame> SupportedGames = new List<SupportedGame>();
            foreach (SectionData section in ini.Sections)
            {
                if (section.SectionName != "General")
                    SupportedGames.Add(new SupportedGame(section.Keys["Game_Name"], section.Keys["Process_Name"]));
            }
            supportedGamesList.AddObjects(SupportedGames);
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
            if (MessageBox.Show("Are you sure you want to remove " + ((supportedGamesList.SelectedObjects.Count > 1) ? supportedGamesList.SelectedObjects.Count.ToString() + " games" : '"' + ((SupportedGame)supportedGamesList.SelectedObject).Game_Name + '"') + " from the supported games list?", "Remove Supported Game", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var parser = new FileIniDataParser();
                IniData ini = parser.ReadFile(Settings.Save_Path + "\\games.ini");
                foreach (SupportedGame item in supportedGamesList.SelectedObjects)
                {
                    ini.Sections.RemoveSection(GameDatabase.gameNameSaferizer(item.Game_Name));
                    supportedGamesList.RemoveObject(item);
                    GameWatcher.RemoveSupportedGame(new SupportedGame(item.Game_Name, item.Process_Name));
                }
                parser.WriteFile(Settings.Save_Path + "\\games.ini", ini);
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

        void addForm_addGame(SupportedGame nGame, SupportedGame oItem)
        {
            if (oItem != null)
            {
                var parser = new FileIniDataParser();
                IniData ini = parser.ReadFile(Settings.Save_Path + "\\games.ini");
                string new_section = GameDatabase.gameNameSaferizer(nGame.Game_Name);
                ini.Sections.AddSection(new_section);
                ini[new_section].AddKey("Game_Name", nGame.Game_Name);
                ini[new_section].AddKey("Process_Name", nGame.Process_Name);
                ini.Sections.RemoveSection(GameDatabase.gameNameSaferizer(oItem.Game_Name));
                parser.WriteFile(Settings.Save_Path + "\\games.ini", ini);
                //
                GameDatabase.RenameGame(oItem, nGame);
                supportedGamesList.UpdateObject(nGame);
            }
            //
            supportedGamesList.AddObject(new SupportedGame(nGame.Game_Name, nGame.Process_Name));
            //
            try
            {
                NameValueCollection data = new NameValueCollection();
                data.Add("Game_Name", nGame.Game_Name);
                data.Add("Process_Name", nGame.Process_Name);
                new WebClient().UploadValues(new Uri("http://updater.logicpwn.com/addGame.php"), data);
            }
            catch (Exception ex) { if (connectedToInternet()) { ReportCrash(ex); } } 
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (supportedGamesList.SelectedObjects.Count > 1) { MessageBox.Show("You can not edit multiple items."); return; }
            SupportedGame item = (SupportedGame)supportedGamesList.SelectedObject;
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
}
