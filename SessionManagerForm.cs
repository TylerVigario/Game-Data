using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Game_Data
{
    public partial class SessionManagerForm : Form
    {
        private GameData _game;
        private delegate void SetSessionsD(List<SessionData> sessions);
        //
        AddSessionForm add_session_form;

        public SessionManagerForm(GameData game)
        {
            InitializeComponent();
            //
            #region ObjectListView stuff

            if (!String.IsNullOrEmpty(Settings.SessionsList_State)) { sessionsList.RestoreState(Convert.FromBase64String(Settings.SessionsList_State)); }
            //
            this.Start_Time.AspectToStringConverter = delegate(object x) {
                DateTime time = (DateTime)x;
                return time.Month.ToString() + "/" + time.Day.ToString() + "/" + time.Year.ToString().Substring(2) + " " + time.ToShortTimeString();
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
            _game = game;
            this.Text = game.Name + " - Sessions";
        }

        public string Game_ID { get { return _game.ID; } }

        private void SessionManagerForm_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(LoadSessions)).Start();
            //
            if (!String.IsNullOrEmpty(Settings.SessionManager_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.SessionManager_Window_Geometry, this); }
            GameDatabase.GameClosed += GameDatabase_GameClosed;
        }

        private void GameDatabase_GameClosed(GameData game, SessionData session)
        {
            if (_game == game) { sessionsList.AddObject(session); }
        }

        private void LoadSessions()
        {
            sessionsList.AddObjects(GameDatabase.LoadGameSessions(_game.ID));
        }

        private void SessionManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AddSessionForm.isOpen)
            {
                add_session_form.Close();
            }
            Settings.SessionManager_Window_Geometry = WindowGeometry.GeometryToString(this);
            Settings.SessionsList_State = Convert.ToBase64String(sessionsList.SaveState());
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove " + ((sessionsList.SelectedObjects.Count > 1) ? sessionsList.SelectedObjects.Count.ToString() + " sessions?" : "this session?"), "Remove Session" + ((sessionsList.SelectedObjects.Count > 1) ? "s" : "") + " - " + _game.Name, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (SessionData session in sessionsList.SelectedObjects) { GameDatabase.RemoveSession(_game.ID, session); }
                sessionsList.RemoveObjects(sessionsList.SelectedObjects);
            }
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
                GameDatabase.RemoveSession(_game.ID, oSes);
            }
            sessionsList.AddObject(nSes);
            GameDatabase.AddSession(_game.ID, nSes);
        }

        private void sessionsList_SelectionChanged(object sender, EventArgs e)
        {
            if (sessionsList.SelectedObjects.Count > 0)
            {
                removeToolStripMenuItem.Enabled = true;
                button3.Enabled = true;
                if (sessionsList.SelectedObjects.Count > 1)
                {
                    mergToolStripMenuItem.Enabled = true;
                    editToolStripMenuItem.Enabled = false;
                    button2.Enabled = false;
                    button4.Enabled = true;
                }
                else
                {
                    mergToolStripMenuItem.Enabled = false;
                    editToolStripMenuItem.Enabled = true;
                    button2.Enabled = true;
                    button4.Enabled = false;
                }
            }
            else
            {
                removeToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
                mergToolStripMenuItem.Enabled = false;
                button2.Enabled = false;
                button2.Enabled = false;
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
            if (MessageBox.Show("Are you sure you want merge these " + sessionsList.SelectedObjects.Count.ToString() + " sessions?", "Merge Sessions - " + _game.Name, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<SessionData> ses = new List<SessionData>();
                SessionData s = new SessionData();
                foreach (SessionData session in sessionsList.SelectedObjects)
                {
                    if (session.Start_Time < s.Start_Time) { s.Start_Time = session.Start_Time; }
                    if (session.End_Time > s.End_Time) { s.End_Time = session.End_Time; }
                    ses.Add(session);
                }
                foreach (SessionData session in ses)
                {
                    sessionsList.RemoveObject(session);
                    GameDatabase.RemoveSession(_game.ID, session);
                }
                sessionsList.AddObject(s);
                GameDatabase.AddSession(_game.ID, s);
            }
        }
    }
}
