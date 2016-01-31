using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Game_Data
{
    public partial class SupportedGamesForm : Form
    {
        private static bool open = false;
        private int nextID = 0;
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
            if (!String.IsNullOrEmpty(Settings.SupportedGames_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.SupportedGames_Window_Geometry, this); }
            if (!String.IsNullOrEmpty(Settings.SupportedGamesList_State)) { supportedGamesList.RestoreState(Convert.FromBase64String(Settings.SupportedGamesList_State)); }
            //
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(Settings.Save_Path + "\\games.ini");
            List<SupportedGame> SupportedGames = new List<SupportedGame>();
            foreach (SectionData section in ini.Sections)
            {
                if (section.SectionName != "General")
                {
                    SupportedGames.Add(new SupportedGame(section.SectionName, section.Keys["Game_Name"], section.Keys["Process_Name"]));
                    int temp = int.Parse(section.SectionName);
                    if (temp > nextID) { nextID = temp; }
                }
            }
            nextID++;
            supportedGamesList.AddObjects(SupportedGames);
        }

        private void SupportedGamesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AddSupportedGameForm.isOpen)
            {
                addForm.Close();
            }
            Settings.SupportedGames_Window_Geometry = WindowGeometry.GeometryToString(this);
            Settings.SupportedGamesList_State = Convert.ToBase64String(supportedGamesList.SaveState());
        }

        private void SupportedGamesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove " + ((supportedGamesList.SelectedObjects.Count > 1) ? supportedGamesList.SelectedObjects.Count.ToString() + " games from the supported games list? This will also erase session data for these games." : '"' + ((SupportedGame)supportedGamesList.SelectedObject).Game_Name + '"' + " from the supported games list? This will also erase session data."), "Remove Supported Game" + ((supportedGamesList.SelectedObjects.Count > 1) ? "s" : ""), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var parser = new FileIniDataParser();
                IniData ini = parser.ReadFile(Settings.Save_Path + "\\games.ini");
                foreach (SupportedGame item in supportedGamesList.SelectedObjects)
                {
                    supportedGamesList.RemoveObject(item);
                    ini.Sections.RemoveSection(item.ID);
                    GameWatcher.RemoveSupportedGame(item);
                    GameDatabase.RemoveGame(item.ID);
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
                addForm = new AddSupportedGameForm(nextID++);
                addForm.addGame += addForm_addGame;
                addForm.Show();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            SupportedGame item = (SupportedGame)supportedGamesList.SelectedObject;
            if (AddSupportedGameForm.isOpen)
            {
                addForm.BringToFront();
            }
            else
            {
                addForm = new AddSupportedGameForm(item);
                addForm.addGame += addForm_addGame;
                addForm.Show();
            }
        }

        void addForm_addGame(SupportedGame nGame, SupportedGame oGame)
        {
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(Application.StartupPath + "\\games.ini");
            if (oGame == null)
            {
                supportedGamesList.AddObject(nGame);
                GameWatcher.AddSupportedGame(nGame);
                ini.Sections.AddSection(nGame.ID);
                ini[nGame.ID].AddKey("Process_Name", nGame.Process_Name);
                ini[nGame.ID].AddKey("Game_Name", nGame.Game_Name);
            }
            else
            {
                supportedGamesList.RemoveObject(oGame);
                supportedGamesList.AddObject(nGame);
                if (nGame.Game_Name != oGame.Game_Name) { GameDatabase.RenameGame(oGame, nGame); }
                GameWatcher.EditSupportedGame(oGame, nGame);
                ini[nGame.ID]["Process_Name"] = nGame.Process_Name;
                ini[nGame.ID]["Game_Name"] = nGame.Game_Name;
            }
            parser.WriteFile(Application.StartupPath + "\\games.ini", ini);
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
                removeToolStripMenuItem.Enabled = true;
                if (supportedGamesList.SelectedObjects.Count > 1)
                {
                    editButton.Enabled = false;
                    editToolStripMenuItem.Enabled = false;
                }
                else
                {
                    editButton.Enabled = true;
                    editToolStripMenuItem.Enabled = true;
                }
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

    #region Supported Game

    public class SupportedGame
    {
        private string _id;
        private string _game_name;
        private string _process_name;

        public SupportedGame(string id_, string g_n, string p_n)
        {
            _id = id_;
            _game_name = g_n;
            _process_name = p_n;
        }

        public string ID {  get { return _id; } }

        public string Game_Name { get { return _game_name; } }

        public string Process_Name { get { return _process_name; } }
    }

    #endregion
}
