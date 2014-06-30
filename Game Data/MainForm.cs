﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Thoughtful_Coding;

namespace Game_Data
{
    public partial class MainForm : Form
    {
        Mutex appMutex;
        //
        private delegate void gameChangedD(string game_name, string process_name);
        //
        AboutForm about_form;
        SettingsForm settings_form;
        SupportedGamesForm supported_games_form;
        List<WeeklyAveragesForm> game_stats_form = new List<WeeklyAveragesForm>();
        List<SessionManagerForm> session_manager_form = new List<SessionManagerForm>();
        List<GameInfoForm> game_info_form = new List<GameInfoForm>();
        //
        GameWatcher gameWatch;
        UpdaterForm selfUpdater;
        CalendarReport calendar_report;
        //
        List<string> messagePump = new List<string>();

        #region Form Load/Close

        public MainForm()
        {
            try
            {
                appMutex = Mutex.OpenExisting("game-data");
                //
                MessageBox.Show("Only one instance of Game Data may exist", "Game Data");
                Environment.Exit(0);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                appMutex = new Mutex(false, "game-data");
            }
            //
            InitializeComponent();
            //
            this.Text = "Game Data v" + Application.ProductVersion + " beta";
            trayIcon.Text = "Game Data v" + Application.ProductVersion + " beta";
            //
            Settings.load();
            //
            WindowGeometry.GeometryFromString(Settings.Main_Window_Geometry, this);
            gamesList.RestoreState(Convert.FromBase64String(Settings.GamesList_State));
            if (Settings.Start_Hidden) { HideMe(true); }
            //
            #region ObjectListView stuff

            TextOverlay textOverlay = this.gamesList.EmptyListMsgOverlay as TextOverlay;
            textOverlay.TextColor = Color.SteelBlue;
            textOverlay.BackColor = Color.WhiteSmoke;
            textOverlay.BorderColor = Color.DarkGray;
            textOverlay.Font = new Font("Tahoma", 26);
            gamesList.EmptyListMsg = "No sessions recorded";
            //
            gamesList.RestoreState(Convert.FromBase64String(Settings.GamesList_State));
            //
            this.Last_Played.AspectToStringConverter = delegate(object x)
            {
                DateTime last = (DateTime)x;
                if (last > DateTime.Now) { return "Right Now"; }
                else { return GameDatabase.calculateLastPlayedString(last); }
            };
            this.Total_Time.AspectToStringConverter = delegate(object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Maximum_Session_Time.AspectToStringConverter = delegate(object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Minimum_Session_Time.AspectToStringConverter = delegate(object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Average_Session_Time.AspectToStringConverter = delegate(object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Last_Session_Time.AspectToStringConverter = delegate(object x)
            {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };

            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            selfUpdater = new UpdaterForm(1);
            if (Settings.Update_Check_Interval > 0)
            {
                updateChecker.Interval = Settings.Update_Check_Interval * 60000;
                updateChecker.Enabled = true;
            }
            if (Settings.Update_Check_On_Startup)
            {
                selfUpdater.checkForUpdate();
                SupportedGamesForm.checkForUpdate();
            }
            //
            GameDatabase.Loaded += GameDatabase_Loaded;
            GameDatabase.RefreshGame += GameDatabase_RefreshGame;
            GameDatabase.FullRefresh += GameDatabase_FullRefresh;
            GameDatabase.Load();
        }

        void GameDatabase_Loaded()
        {
            GameDatabase.Loaded -= GameDatabase_Loaded;
            gamesList.SetObjects(GameDatabase.GamesData);
            //
            gameWatch = new GameWatcher(); // Multi thread
            GameWatcher.gameStarted += gameWatch_gameStarted;
            GameWatcher.gameClosed += gameWatch_gameClosed;
            gameWatch.Start(); // Multi thread
            //
            nextDay.Interval = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalMilliseconds;
            nextDay.Enabled = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && Settings.Close_To_Tray)
            {
                e.Cancel = true;
                HideMe();
            }
            else
            {
                closeGD(true);
            }
        }

        private void HideMe(bool first = false)
        {
            this.ShowInTaskbar = false;
            this.trayIcon.Visible = true;
            if (first) { this.WindowState = FormWindowState.Minimized; }
            else { this.Hide(); }
        }

        private void closeGD(bool skip_c = false)
        {
            if (!Settings.Exit_Confrimation || skip_c || (MessageBox.Show("Are you sure you want to exit Game Data?", "Game Data", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                Settings.Main_Window_Geometry = WindowGeometry.GeometryToString(this);
                Settings.GamesList_State = Convert.ToBase64String(gamesList.SaveState());
                gameWatch.Stop();
                //
                appMutex.WaitOne();
                appMutex.ReleaseMutex();
                appMutex.Dispose();
                gameWatch.Dispose();
                trayIcon.Dispose();
                Environment.Exit(0);
            }
        }

        private void gamesList_SelectionChanged(object sender, EventArgs e)
        {
            if (gamesList.SelectedObjects.Count > 0)
            {
                weeklyAveragesToolStripMenuItem.Enabled = true;
            }
            else { weeklyAveragesToolStripMenuItem.Enabled = false; }
        }

        #endregion

        #region Tray Icon

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            trayIcon.Visible = false;
            if (this.WindowState == FormWindowState.Minimized) { this.WindowState = FormWindowState.Normal; }
            else { this.Show(); }
            this.BringToFront();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trayIcon_DoubleClick(null, null);
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

        #region GameWatcher

        void gameWatch_gameStarted(string game_name, string process_name)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new gameChangedD(gameWatch_gameStarted), new object[] { game_name, process_name });
                return;
            }
            //
            if (GameDatabase.GameStarted(game_name))
            {
                displayStatus(game_name + " (" + process_name + ") has been added.", 5);
            }
            else
            {
                displayStatus(game_name + " (" + process_name + ") has started.", 5);
            }
            //
            GameDataCollector.watchGame(game_name, process_name);
        }

        void gameWatch_gameClosed(string game_name, string process_name)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new gameChangedD(gameWatch_gameClosed), new object[] { game_name, process_name });
                return;
            }
            //
            RunningSession session = GameDataCollector.stopWatchingGame(process_name);
            SessionData sData = GameDatabase.AddRecordedSession(game_name, session);
            foreach (SessionManagerForm form in session_manager_form)
            {
                if (form.Game_Loaded == game_name) { form.session_added(sData); }
            }
            displayStatus(game_name + " (" + process_name + ") has been closed.", 5);
        }

        #endregion

        #region Status Bar

        private void displayStatus(string text, int timeout = 5)
        {
            if (!statusClear.Enabled)
            {
                statusClear.Enabled = false;
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

        private void calendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CalendarReport.isOpen)
            {
                calendar_report.BringToFront();
            }
            else
            {
                calendar_report = new CalendarReport();
                calendar_report.FormClosed += calendar_report_FormClosed;
                calendar_report.Show();
            }
        }

        void calendar_report_FormClosed(object sender, FormClosedEventArgs e)
        {
            calendar_report.Dispose();
        }

        void supported_games_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            supported_games_form.Dispose();
        }

        void settings_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings_form.Dispose();
            //
            int temp = Settings.Update_Check_Interval * 60000;
            if (temp != updateChecker.Interval) { updateChecker.Interval = temp; }
            temp = Settings.Collection_Resolution * 1000;
            if (temp != GameDataCollector.Collection_Res) { GameDataCollector.Collection_Change(); }
        }

        private void about_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            about_form.Dispose();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gamesListPrinter.ListView = gamesList;
            gamesListPrinter.PrintPreview();
        }

        private void counterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CounterForm().Show();
        }

        #endregion

        #region Game Menu

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamesList.SelectedObjects.Count == 1)
            {
                if (MessageBox.Show("Are you sure you want to remove " + '"' + ((GameData)gamesList.SelectedObject).Name + '"' + "'s  data?", "Remove Game Data", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    GameDatabase.RemoveGame(((GameData)gamesList.SelectedObject));
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
                        if (form.Game_Loaded.ToLower() == ((GameData)gamesList.SelectedObject).Name.ToLower())
                        {
                            form.BringToFront();
                            handled = true;
                            break;
                        }
                    }
                }
                if (!handled)
                {
                    SessionManagerForm temp = new SessionManagerForm(((GameData)gamesList.SelectedObject).Name);
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

        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamesList.SelectedObject != null)
            {
                if (((GameData)gamesList.SelectedObject).Sessions < 1) { MessageBox.Show("Nothing to show. Play a few sessions and then try again."); return; }
                bool handled = false;
                if (game_stats_form.Count > 0)
                {
                    foreach (WeeklyAveragesForm form in game_stats_form)
                    {
                        if (form.Game_Loaded.ToLower() == ((GameData)gamesList.SelectedObject).Name.ToLower())
                        {
                            form.BringToFront();
                            handled = true;
                            break;
                        }
                    }
                }
                if (!handled)
                {
                    WeeklyAveragesForm temp = new WeeklyAveragesForm(GameDatabase.GamesData.Find(delegate(GameData g) { return g.Name.ToLower() == ((GameData)gamesList.SelectedObject).Name.ToLower(); }));
                    temp.FormClosing += GameStats_FormClosing;
                    game_stats_form.Add(temp);
                    temp.Show();
                }
            }
        }

        void GameStats_FormClosing(object sender, FormClosingEventArgs e)
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

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamesList.SelectedObject != null)
            {
                bool handled = false;
                if (game_info_form.Count > 0)
                {
                    foreach (GameInfoForm form in game_info_form)
                    {
                        if (form.Game_Loaded.Name.ToLower() == ((GameData)gamesList.SelectedObject).Name.ToLower())
                        {
                            form.BringToFront();
                            handled = true;
                            break;
                        }
                    }
                }
                if (!handled)
                {
                    GameInfoForm temp = new GameInfoForm(GameDatabase.GamesData.Find(delegate(GameData g) { return g.Name.ToLower() == ((GameData)gamesList.SelectedObject).Name.ToLower(); }));
                    temp.FormClosing += GameInfo_FormClosing;
                    game_info_form.Add(temp);
                    temp.Show();
                }
            }
        }

        void GameInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameInfoForm form = sender as GameInfoForm;
            game_info_form.Remove(form);
        }

        #endregion

        #region Events

        void GameDatabase_FullRefresh()
        {
            gamesList.BuildList(true);
        }

        void GameDatabase_RefreshGame(GameData game)
        {
            gamesList.RefreshObject(game);
        }

        #endregion

        #region Timers

        private void nextDay_Tick(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            nextDay.Interval = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalMilliseconds;
            gamesList.BuildList(true);
        }

        private void updateChecker_Tick(object sender, EventArgs e)
        {
            selfUpdater.checkForUpdate();
            SupportedGamesForm.checkForUpdate();
        }

        #endregion

        private void monthlyAveragesToolStripMenuItem_Click(object sender, EventArgs e) /* In Progress */
        {

        }
    }
}