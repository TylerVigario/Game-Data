using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Game_Data
{
    public partial class SessionManagerForm : Form
    {
        private string game;
        private delegate void SetSessionsD(List<SessionData> sessions);
        //
        AddSessionForm add_session_form;

        public SessionManagerForm(string game_name)
        {
            InitializeComponent();
            //
            #region ObjectListView stuff

            sessionsList.RestoreState(Convert.FromBase64String(Settings.SessionsList_State));
            //
            this.Start_Time.AspectToStringConverter = delegate(object x) {
                return GameDatabase.calculateLastPlayedString((DateTime)x, true);
            };
            this.Time_Span.AspectToStringConverter = delegate(object x) {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            //
            this.Start_Time.GroupKeyGetter = delegate(object rowObject) {
                DateTime date = ((SessionData)rowObject).Start_Time;
                return new DateTime(date.Year, date.Month, 1);
            };
            this.Start_Time.GroupKeyToTitleConverter = delegate(object groupKey) { return ((DateTime)groupKey).ToString("MMMM yyyy"); };
            this.sessionsList.ShowGroups = true;
            this.sessionsList.SortGroupItemsByPrimaryColumn = false;

            #endregion
            //
            game = game_name;
            this.Text = game_name + " - Sessions";
        }

        public string Game_Loaded { get { return game; } }

        private void SessionManagerForm_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(LoadSessions)).Start();
            //
            WindowGeometry.GeometryFromString(Settings.SessionManager_Window_Geometry, this);
            GameDatabase.GameClosed += GameDatabase_GameClosed;
        }

        private void GameDatabase_GameClosed(GameData _game, SessionData session)
        {
            if (_game.Name == game)
            {
                sessionsList.AddObject(session);
            }
        }

        private void LoadSessions()
        {
            sessionsList.AddObjects(GameDatabase.LoadGameSessions(game));
        }

        private void SessionManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.SessionManager_Window_Geometry = WindowGeometry.GeometryToString(this);
            Settings.SessionsList_State = Convert.ToBase64String(sessionsList.SaveState());
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (SessionData session in sessionsList.SelectedObjects) { GameDatabase.RemoveSession(game, session); }
            sessionsList.RemoveObjects(sessionsList.SelectedObjects);
        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*if (sessionsList.SelectedObjects.Count > 1)
            {
                sessionsList.AddObject(GameDatabase.MergeSessions(game, (List<SessionListItem>)sessionsList.SelectedObjects));
                sessionsList.RemoveObjects(sessionsList.SelectedObjects);
            }*/
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AddSessionForm.isOpen)
            {
                add_session_form.BringToFront();
            }
            else
            {
                add_session_form = new AddSessionForm();
                add_session_form.FormClosed += add_session_form_FormClosed;
                add_session_form.sessionAdded += add_session_form_sessionAdded;
                add_session_form.Show();
            }
        }

        void add_session_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            add_session_form.Dispose();
        }

        void add_session_form_sessionAdded(SessionData session)
        {
            sessionsList.AddObject(session);
            GameDatabase.AddSession(game, session);
        }

        private void sessionsList_SelectionChanged(object sender, EventArgs e)
        {
            if (sessionsList.SelectedObjects.Count > 0)
            {
                removeToolStripMenuItem.Enabled = true;
                if (sessionsList.SelectedObjects.Count > 1) { mergeToolStripMenuItem.Enabled = true; }
                else { mergeToolStripMenuItem.Enabled = false; }
            }
            else
            {
                removeToolStripMenuItem.Enabled = false;
                mergeToolStripMenuItem.Enabled = false;
            }
        }
    }
}
