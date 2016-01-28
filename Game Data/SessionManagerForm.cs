using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Game_Data
{
    public partial class SessionManagerForm : Form
    {
        private string _game_id;
        private delegate void SetSessionsD(List<SessionData> sessions);
        //
        AddSessionForm add_session_form;

        public SessionManagerForm(GameData game)
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
            _game_id = game.ID;
            this.Text = game.Name + " - Sessions";
        }

        public string Game_ID { get { return _game_id; } }

        private void SessionManagerForm_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(LoadSessions)).Start();
            //
            WindowGeometry.GeometryFromString(Settings.SessionManager_Window_Geometry, this);
            GameDatabase.GameClosed += GameDatabase_GameClosed;
        }

        private void GameDatabase_GameClosed(GameData _game, SessionData session)
        {
            if (_game.ID == _game_id)
            {
                sessionsList.AddObject(session);
            }
        }

        private void LoadSessions()
        {
            sessionsList.AddObjects(GameDatabase.LoadGameSessions(_game_id));
        }

        private void SessionManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.SessionManager_Window_Geometry = WindowGeometry.GeometryToString(this);
            Settings.SessionsList_State = Convert.ToBase64String(sessionsList.SaveState());
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (SessionData session in sessionsList.SelectedObjects) { GameDatabase.RemoveSession(_game_id, session); }
            sessionsList.RemoveObjects(sessionsList.SelectedObjects);
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

        void add_session_form_sessionAdded(SessionData nSes, SessionData oSes)
        {
            if (oSes != null)
            {
                sessionsList.RemoveObject(oSes);
                GameDatabase.RemoveSession(_game_id, oSes);
            }
            sessionsList.AddObject(nSes);
            GameDatabase.AddSession(_game_id, nSes);
        }

        private void sessionsList_SelectionChanged(object sender, EventArgs e)
        {
            if (sessionsList.SelectedObjects.Count > 0)
            {
                removeToolStripMenuItem.Enabled = true;
                if (sessionsList.SelectedObjects.Count > 1)
                {
                    mergToolStripMenuItem.Enabled = true;
                    editToolStripMenuItem.Enabled = false;
                }
                else
                {
                    mergToolStripMenuItem.Enabled = false;
                    editToolStripMenuItem.Enabled = true;
                }
            }
            else
            {
                removeToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
                mergToolStripMenuItem.Enabled = false;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AddSessionForm.isOpen)
            {
                add_session_form.BringToFront();
            }
            else
            {
                add_session_form = new AddSessionForm((SessionData)sessionsList.SelectedObject);
                add_session_form.FormClosed += add_session_form_FormClosed;
                add_session_form.sessionAdded += add_session_form_sessionAdded;
                add_session_form.Show();
            }
        }

        private void mergToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<SessionData> ses = new List<SessionData>();
            SessionData s = new SessionData();
            foreach (SessionData session in sessionsList.Objects)
            {
                if (session.Start_Time < s.Start_Time) { s.Start_Time = session.Start_Time; }
                if (session.End_Time > s.End_Time) { s.End_Time = session.End_Time; }
                ses.Add(session);
            }
            foreach (SessionData session in ses)
            {
                sessionsList.RemoveObject(session);
                GameDatabase.RemoveSession(_game_id, session);
            }
            sessionsList.AddObject(s);
            GameDatabase.AddSession(_game_id, s);
        }
    }
}
