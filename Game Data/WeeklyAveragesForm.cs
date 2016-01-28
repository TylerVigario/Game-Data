using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Game_Data
{
    public partial class WeeklyAveragesForm : Form
    {
        private string game;

        public WeeklyAveragesForm(GameData data)
        {
            InitializeComponent();
            //
            game = data.ID;
        }

        public string Game_Loaded { get { return game; } }

        private int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        private void GameStatsForm_Load(object sender, System.EventArgs e)
        {
            WindowGeometry.GeometryFromString(Settings.GameStats_Window_Geometry, this);
            GameName.Text = game;
            //
            new Thread(new ThreadStart(ThreadedLoad)).Start();
        }

        private void ThreadedLoad()
        {
            int[] dow = new int[7];
            double[] mod = new double[7];
            //
            List<int> weeksCounted = new List<int>();
            foreach (SessionData sData in GameDatabase.LoadGameSessions(game))
            {
                int weekNumber = GetWeekNumber(sData.Start_Time);
                if (!weeksCounted.Contains(weekNumber)) { weeksCounted.Add(weekNumber); }
                switch (sData.Start_Time.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dow[0]++;
                        mod[0] += (sData.End_Time - sData.Start_Time).TotalMinutes;
                        break;
                    case DayOfWeek.Monday:
                        dow[1]++;
                        mod[1] += (sData.End_Time - sData.Start_Time).TotalMinutes;
                        break;
                    case DayOfWeek.Tuesday:
                        dow[2]++;
                        mod[2] += (sData.End_Time - sData.Start_Time).TotalMinutes;
                        break;
                    case DayOfWeek.Wednesday:
                        dow[3]++;
                        mod[3] += (sData.End_Time - sData.Start_Time).TotalMinutes;
                        break;
                    case DayOfWeek.Thursday:
                        dow[4]++;
                        mod[4] += (sData.End_Time - sData.Start_Time).TotalMinutes;
                        break;
                    case DayOfWeek.Friday:
                        dow[5]++;
                        mod[5] += (sData.End_Time - sData.Start_Time).TotalMinutes;
                        break;
                    case DayOfWeek.Saturday:
                        dow[6]++;
                        mod[6] += (sData.End_Time - sData.Start_Time).TotalMinutes;
                        break;
                }
            }
            //
            for (int i = 0; i < sessionsADay.Series[0].Points.Count; i++)
            {
                sessionsADay.Series[0].Points[i].YValues[0] = dow[i] / weeksCounted.Count;
                sessionsADay.Series[1].Points[i].YValues[0] = (mod[i] / 60) / weeksCounted.Count;
            }
        }

        private void GameStatsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.GameStats_Window_Geometry = WindowGeometry.GeometryToString(this);
        }
    }
}
