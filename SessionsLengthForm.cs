using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Game_Data
{
    public partial class SessionsLengthForm : Form
    {
        private GameData game;

        public SessionsLengthForm(GameData data)
        {
            InitializeComponent();
            //
            game = data;
        }

        public string Game_ID { get { return game.ID; } }

        private void SessionsLengthForm_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Settings.SessionsLength_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.SessionsLength_Window_Geometry, this); }
            this.Text = game.Name + " - Sessions Length";
            //
            foreach (SessionData session in GameDatabase.LoadGameSessions(game.ID))
            {
                DataPoint pt = new DataPoint(session.Start_Time.Date.ToOADate(), session.Time_Span.TotalMinutes);
                chart1.Series[0].Points.Add(pt);
            }
        }


        private void SessionsLengthForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.SessionsLength_Window_Geometry = WindowGeometry.GeometryToString(this);
        }
    }
}
