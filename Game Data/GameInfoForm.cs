using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Game_Data
{
    public partial class GameInfoForm : Form
    {
        private GameData game;
        private delegate void SetCalculatedFieldsD(string s_a_d, string a_d_t);

        public GameInfoForm(GameData _data)
        {
            InitializeComponent();
            //
            game = _data;
            this.label1.Text = game.Name;
        }

        public GameData Game_Loaded { get { return game; } }

        private void GameInfoForm_Load(object sender, EventArgs e)
        {
            if (game.Sessions > 1)
            {
                installedOnLabel.Text = GameDatabase.calculateLastPlayedString(game.First_Played);
                sessionsLabel.Text = game.Sessions.ToString();
                lastPlayedLabel.Text = GameDatabase.calculateLastPlayedString(game.Last_Played);
                totalTimeLabel.Text = GameDatabase.calculateTimeString(game.Total_Time, false);
                lastSessionTimeLabel.Text = GameDatabase.calculateTimeString(game.Last_Session_Time, false);
                longestSessionTimeLabel.Text = GameDatabase.calculateTimeString(game.Maximum_Session_Time, false);
                shortestSessionTimeLabel.Text = GameDatabase.calculateTimeString(game.Minimum_Session_Time, false);
                averageSessionTimeLabel.Text = GameDatabase.calculateTimeString(game.Average_Session_Time, false);
                //
                new Thread(new ThreadStart(ExtraCalculation)).Start();
            }
        }

        private void ExtraCalculation()
        {
            List<SessionData> sessions = GameDatabase.LoadSessions(game.Name);
            List<DateTime> datesPlayed = new List<DateTime>();
            foreach (SessionData session in sessions)
            {
                if (datesPlayed.Count == 0 || !datesPlayed.Contains(session.Start_Time.Date)) { datesPlayed.Add(session.Start_Time.Date); }
            }
            string s_a_d = (game.Sessions / datesPlayed.Count).ToString();
            string a_d_t = GameDatabase.calculateTimeString(TimeSpan.FromMilliseconds((game.Total_Time.TotalMilliseconds / datesPlayed.Count)), false);
            SetCalculatedFields(s_a_d, a_d_t);
        }

        private void SetCalculatedFields(string s_a_d, string a_d_t)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SetCalculatedFieldsD(SetCalculatedFields), new [] { s_a_d, a_d_t });
                return;
            }
            //
            sessionsADayLabel.Text = s_a_d;
            averageDailyTimeLabel.Text = a_d_t;
        }
    }
}
