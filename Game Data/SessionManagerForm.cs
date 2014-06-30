using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
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
            this.Date.AspectToStringConverter = delegate(object x) {
                return GameDatabase.calculateLastPlayedString((DateTime)x, true);
            };
            this.Time.AspectToStringConverter = delegate(object x) {
                return GameDatabase.calculateTimeString((TimeSpan)x);
            };
            this.Average_CPU_Usage.AspectToStringConverter = delegate(object x) {
                double acu = (double)x;
                if (acu > 0) { return acu.ToString("P1", System.Globalization.CultureInfo.InvariantCulture); }
                else { return ""; }
            };
            this.Average_Memory_Usage.AspectToStringConverter = delegate(object x) {
                long amu = (long)x;
                if (amu > 0)
                {
                    double temp = Math.Round(Convert.ToDouble((long)x) / 1024, 1);
                    return string.Format("{0:n1}", temp + "MB");
                }
                else { return ""; }
            };
            //
            this.Date.GroupKeyGetter = delegate(object rowObject) {
                DateTime date = ((SessionListItem)rowObject).Date;
                return new DateTime(date.Year, date.Month, 1);
            };
            this.Date.GroupKeyToTitleConverter = delegate(object groupKey) { return ((DateTime)groupKey).ToString("MMMM yyyy"); };
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
        }

        private void LoadSessions()
        {
            List<SessionListItem> sessions = new List<SessionListItem>();
            List<SessionData> sessionsData = GameDatabase.LoadSessions(game);
            if (sessionsData != null)
            {
                foreach (SessionData session in sessionsData) { sessions.Add(new SessionListItem(session)); }
                sessionsList.AddObjects(sessions);
            }
        }

        private void SessionManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.SessionManager_Window_Geometry = WindowGeometry.GeometryToString(this);
            Settings.SessionsList_State = Convert.ToBase64String(sessionsList.SaveState());
        }

        public void session_added(SessionData data)
        {
            sessionsList.AddObject(new SessionListItem(data));
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sessionsList.SelectedObjects.Count > 1)
            {
                //GameDatabase.RemoveSessions(game, );
                //sessionsList.RemoveObjects(sessionsList.SelectedObjects.);
            }
            else if (sessionsList.SelectedObjects.Count == 1)
            {
                GameDatabase.RemoveSession(game, (SessionListItem)sessionsList.SelectedObject);
                sessionsList.RemoveObject(sessionsList.SelectedObject);
            }
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
            sessionsList.AddObject(new SessionListItem(session));
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

    #region SessionListItem

    public class SessionListItem
    {
        DateTime _date = DateTime.MinValue;
        TimeSpan _time = TimeSpan.Zero;
        double _average_cpu_usage = 0;
        long _average_memory_usage = 0;

        public SessionListItem() { }

        public SessionListItem(SessionData data)
        {
            _date = data.End_Time;
            _time = data.End_Time - data.Start_Time;
            _average_cpu_usage = data.Average_CPU_Usage;
            _average_memory_usage = data.Average_Working_Set;
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public double Average_CPU_Usage
        {
            get { return _average_cpu_usage; }
            set { _average_cpu_usage = value; }
        }

        public long Average_Memory_Usage
        {
            get { return _average_memory_usage; }
            set { _average_memory_usage = value; }
        }
    }

    #endregion
}
