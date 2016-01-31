using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Game_Data
{
    public partial class MainForm : Form
    {
        Mutex appMutex;
        private bool allowshowdisplay = true;
        List<string> messagePump = new List<string>();
        #region Forms
        AboutForm about_form;
        SettingsForm settings_form;
        SupportedGamesForm supported_games_form;
        List<WeeklyAveragesForm> game_stats_form = new List<WeeklyAveragesForm>();
        List<SessionManagerForm> session_manager_form = new List<SessionManagerForm>();
        List<SessionsLengthForm> sessions_length_form = new List<SessionsLengthForm>();
        List<GameInfoForm> game_info_form = new List<GameInfoForm>();
        #endregion

        #region Form Load/Close

        public MainForm()
        {
            #region One instance

            try
            {
                appMutex = Mutex.OpenExisting("game-data");
                MessageBox.Show("Only one instance of Game Data may exist", "Game Data");
                Environment.Exit(0);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                appMutex = new Mutex(false, "game-data");
            }

            #endregion
            //
            InitializeComponent();
            //
            Settings.load();
            if (!String.IsNullOrEmpty(Settings.Main_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.Main_Window_Geometry, this); }
            if (Settings.Start_Hidden)
            {
                allowshowdisplay = false;
                this.ShowInTaskbar = false;
                trayIcon.Visible = true;
            }
            //
            #region ObjectListView stuff

            if (!String.IsNullOrEmpty(Settings.GamesList_State)) { gamesList.RestoreState(Convert.FromBase64String(Settings.GamesList_State)); }
            this.Last_Played.AspectToStringConverter = delegate (object x)
            {
                return GameDatabase.calculateLastPlayedString((DateTime)x);
            };
            this.Total_Time.AspectToStringConverter = delegate (object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Maximum_Session_Time.AspectToStringConverter = delegate (object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Minimum_Session_Time.AspectToStringConverter = delegate (object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Average_Session_Time.AspectToStringConverter = delegate (object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Last_Session_Time.AspectToStringConverter = delegate (object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };

            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GameDatabase.Loaded += GameDatabase_Loaded;
            GameDatabase.GameStarted += GameDatabase_GameStarted;
            GameDatabase.GameClosed += GameDatabase_GameClosed;
            GameDatabase.GameRemoved += GameDatabase_GameRemoved;
            GameDatabase.GameRenamed += GameDatabase_GameRenamed;
            GameDatabase.SessionAdded += GameDatabase_SessionAdded;
            GameDatabase.SessionRemoved += GameDatabase_SessionRemoved;
            GameDatabase.Load();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            { 
                closeGD();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (Settings.Minimize_To_Tray)
                {
                    allowshowdisplay = false;
                    this.ShowInTaskbar = false;
                    trayIcon.Visible = true;
                }
            }
        }

        private void closeGD()
        {
            if (!Settings.Exit_Confirmation || (MessageBox.Show("Are you sure you want to exit Game Data?", "Game Data", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                Settings.Main_Window_Geometry = WindowGeometry.GeometryToString(this);
                Settings.GamesList_State = Convert.ToBase64String(gamesList.SaveState());
                Settings.Dispose();
                GameDatabase.Dispose();
                appMutex.WaitOne();
                appMutex.ReleaseMutex();
                appMutex.Dispose();
                trayIcon.Dispose();
                Environment.Exit(0);
            }
        }

        #endregion

        #region Tray Icon

        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.allowshowdisplay = true;
                this.Visible = !this.Visible;
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                trayIcon.Visible = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trayIcon_MouseClick(null, new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeGD();
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            allSettingsToolStripMenuItem_Click(this, null);
        }

        #endregion

        #region GameDatabase events

        private void listUpdate_Tick(object sender, EventArgs e)
        {
            gamesList.RefreshObjects(GameDatabase.GamesData);
        }

        void GameDatabase_Loaded(List<GameData> games)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new LoadedD(GameDatabase_Loaded), new object[] { games });
                return;
            }
            gamesList.SetObjects(games);
            if (games.Count > 0)
            {
                listUpdate.Enabled = true;
            }
        }

        private void GameDatabase_GameStarted(GameData game, DateTime time)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new GameStartedD(GameDatabase_GameStarted), new object[] { game, time });
                return;
            }
            gamesList.SelectObject(game);
            if (gamesList.SelectedObject != null) { gamesList.RefreshObject(game); }
            else { gamesList.AddObject(game); }
            if (time.AddMinutes(1) > DateTime.Now)
            {
                displayStatus(game.Name + " has started.", 5);
            }
            else
            {
                displayStatus(game.Name + " started at " + time.ToShortTimeString() + '.', 5);
            }
            listUpdate.Enabled = true;
        }

        private void GameDatabase_GameClosed(GameData game, SessionData session)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SessionD(GameDatabase_GameClosed), new object[] { game, session });
                return;
            }
            displayStatus(game.Name + " has been closed after " + GameDatabase.calculateTimeString(session.Time_Span, false) + '.', 5);
        }

        private void GameDatabase_GameRemoved(GameData game)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new GameRemovedD(GameDatabase_GameRemoved), new object[] { game });
                return;
            }
            gamesList.RemoveObject(game);
            if (gamesList.Items.Count == 0) { listUpdate.Enabled = false; }
        }

        private void GameDatabase_GameRenamed(GameData old_, GameData new_)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new GameRenamedD(GameDatabase_GameRenamed), new object[] { old_, new_ });
                return;
            }
            gamesList.RemoveObject(old_);
            gamesList.AddObject(new_);
        }

        private void GameDatabase_SessionAdded(GameData game, SessionData session)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SessionD(GameDatabase_SessionAdded), new object[] { game, session });
                return;
            }
            gamesList.RefreshObject(game);
            listUpdate.Enabled = true;
        }

        private void GameDatabase_SessionRemoved(GameData game, SessionData session)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SessionD(GameDatabase_SessionRemoved), new object[] { game, session });
                return;
            }
            gamesList.RefreshObject(game);
        }

        #endregion

        #region Status Bar

        private void displayStatus(string text, int timeout = 5)
        {
            if (!statusClear.Enabled)
            {
                statusLabel.Text = text;
                if (timeout > 0)
                {
                    statusClear.Interval = timeout * 1000;
                    statusClear.Enabled = true;
                }
            }
            else { messagePump.Add(text); }
        }

        private void statusClear_Tick(object sender, EventArgs e)
        {
            statusLabel.Text = "";
            //
            if (messagePump.Count > 0)
            {
                System.Threading.Thread.Sleep(500);
                statusLabel.Text = messagePump[0];
                messagePump.Remove(messagePump[0]);
            }
            else { statusClear.Enabled = false; }
        }

        #endregion

        #region Top Menu

        private void gamesList_SelectionChanged(object sender, EventArgs e)
        {
            if (gamesList.SelectedObjects.Count > 0)
            {
                infoToolStripMenuItem.Enabled = true;
                sessionsToolStripMenuItem.Enabled = true;
                removeToolStripMenuItem.Enabled = true;
                if (((GameData)gamesList.SelectedObject).Sessions > 0)
                {
                    weeklyAveragesToolStripMenuItem.Enabled = true;
                    sessionsLengthToolStripMenuItem.Enabled = true;
                }
                else
                {
                    weeklyAveragesToolStripMenuItem.Enabled = false;
                    sessionsLengthToolStripMenuItem.Enabled = false;
                }
            }
            else
            {
                infoToolStripMenuItem.Enabled = false;
                weeklyAveragesToolStripMenuItem.Enabled = false;
                sessionsLengthToolStripMenuItem.Enabled = false;
                sessionsToolStripMenuItem.Enabled = false;
                removeToolStripMenuItem.Enabled = false;
            }
        }

        private void allSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingsForm.isOpen)
            {
                settings_form.BringToFront();
            }
            else
            {
                settings_form = new SettingsForm();
                settings_form.FormClosed += settings_form_FormClosed;
                settings_form.Show();
            }
        }

        private void supportedGamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SupportedGamesForm.isOpen)
            {
                supported_games_form.BringToFront();
            }
            else
            {
                supported_games_form = new SupportedGamesForm();
                supported_games_form.FormClosed += supported_games_form_FormClosed;
                supported_games_form.Show();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AboutForm.isOpen)
            {
                about_form.BringToFront();
            }
            else
            {
                about_form = new AboutForm();
                about_form.FormClosed += about_form_FormClosed;
                about_form.Show();
            }
        }

        void supported_games_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            supported_games_form.Dispose();
        }

        void settings_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings_form.Dispose();
        }

        private void about_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            about_form.Dispose();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool handled = false;
            if (game_info_form.Count > 0)
            {
                foreach (GameInfoForm form in game_info_form)
                {
                    if (form.Game_Loaded.ID == ((GameData)gamesList.SelectedObject).ID)
                    {
                        form.BringToFront();
                        handled = true;
                        break;
                    }
                }
            }
            if (!handled)
            {
                GameInfoForm temp = new GameInfoForm(GameDatabase.GamesData.Find(delegate (GameData g) { return g.ID == ((GameData)gamesList.SelectedObject).ID; }));
                temp.FormClosing += GameInfo_FormClosing;
                game_info_form.Add(temp);
                temp.Show();
            }
        }

        void GameInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameInfoForm form = sender as GameInfoForm;
            game_info_form.Remove(form);
        }

        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool handled = false;
            if (game_stats_form.Count > 0)
            {
                foreach (WeeklyAveragesForm form in game_stats_form)
                {
                    if (form.Game_ID == ((GameData)gamesList.SelectedObject).ID)
                    {
                        form.BringToFront();
                        handled = true;
                        break;
                    }
                }
            }
            if (!handled)
            {
                WeeklyAveragesForm temp = new WeeklyAveragesForm((GameData)gamesList.SelectedObject);
                temp.FormClosing += GameStats_FormClosing;
                game_stats_form.Add(temp);
                temp.Show();
            }
        }

        void GameStats_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionManagerForm form = sender as SessionManagerForm;
            session_manager_form.Remove(form);
        }

        private void sessionsLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool handled = false;
            if (sessions_length_form.Count > 0)
            {
                foreach (SessionsLengthForm form in sessions_length_form)
                {
                    if (form.Game_ID == ((GameData)gamesList.SelectedObject).ID)
                    {
                        form.BringToFront();
                        handled = true;
                        break;
                    }
                }
            }
            if (!handled)
            {
                SessionsLengthForm temp = new SessionsLengthForm((GameData)gamesList.SelectedObject);
                temp.FormClosing += SessionsLength_FormClosing;
                sessions_length_form.Add(temp);
                temp.Show();
            }
        }

        private void SessionsLength_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionsLengthForm form = sender as SessionsLengthForm;
            sessions_length_form.Remove(form);
        }

        #endregion

        #region Game Menu

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamesList.SelectedObjects.Count == 1)
            {
                if (MessageBox.Show("Are you sure you want to remove " + '"' + ((GameData)gamesList.SelectedObject).Name + '"' + "'s  data?", "Remove Game Data", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    GameDatabase.RemoveGame(((GameData)gamesList.SelectedObject).ID);
                }
            }
            else if (gamesList.SelectedObjects.Count > 1)
            {
                List<GameData> selectedItems = (List<GameData>)gamesList.SelectedObjects;
                if (MessageBox.Show("Are you sure you want to remove " + gamesList.SelectedObjects.Count.ToString() + " games?", "Remove Game Data", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    GameDatabase.RemoveGames(selectedItems);
                }
            }
        }

        private void sessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamesList.SelectedObject != null)
            {
                bool handled = false;
                if (session_manager_form.Count > 0)
                {
                    foreach (SessionManagerForm form in session_manager_form)
                    {
                        if (form.Game_ID == ((GameData)gamesList.SelectedObject).ID)
                        {
                            form.BringToFront();
                            handled = true;
                            break;
                        }
                    }
                }
                if (!handled)
                {
                    SessionManagerForm temp = new SessionManagerForm((GameData)gamesList.SelectedObject);
                    temp.FormClosing += SessionManager_FormClosing;
                    session_manager_form.Add(temp);
                    temp.Show();
                }
            }
        }

        void SessionManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionManagerForm form = sender as SessionManagerForm;
            session_manager_form.Remove(form);
        }

        private void gamesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            sessionsToolStripMenuItem_Click(this, null);
        }

        private void gameMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gamesList.SelectedObject == null)
            {
                e.Cancel = true;
            }
        }

        #endregion
    }
}
