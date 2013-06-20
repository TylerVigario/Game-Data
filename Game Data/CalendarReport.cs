using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Calendar.NET;

namespace Game_Data
{
    public partial class CalendarReport : Form
    {
        private static bool open = false;

        public CalendarReport()
        {
            open = true;
            //
            InitializeComponent();
        }

        public static bool isOpen
        {
            get { return open; }
        }

        private void CalendarReport_Load(object sender, EventArgs e)
        {
            calendar1.CalendarDate = DateTime.Now;
            GameDatabase.Loaded += GameDatabase_Loaded;
            GameDatabase.Load();
        }

        void GameDatabase_Loaded()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EmptyD(GameDatabase_Loaded));
                return;
            }
            //
            List<PlayDay> daysPlayed = new List<PlayDay>();
            foreach (CachedSessionsList sessions in GameDatabase.CachedSessions)
            {
                foreach (SessionData session in sessions.Sessions)
                {
                    PlayDay day = daysPlayed.Find(delegate(PlayDay x) { return x.Date == session.Start_Time.Date; });
                    if (day != null)
                    {
                        DayGame game = day.Games.Find(delegate(DayGame x) { return x.Game == sessions.Game_Name; });
                        if (game != null)
                        {
                            game.AddTime(session.End_Time - session.Start_Time);
                        }
                        else
                        {
                            game = new DayGame(sessions.Game_Name);
                            game.AddTime(session.End_Time - session.Start_Time);
                            day.Games.Add(game);
                        }
                    }
                    else
                    {
                        day = new PlayDay(session.Start_Time.Date);
                        DayGame game = new DayGame(sessions.Game_Name);
                        game.AddTime(session.End_Time - session.Start_Time);
                        day.Games.Add(game);
                        daysPlayed.Add(day);
                    }
                }
            }
            //
            List<CustomEvent> events = new List<CustomEvent>();
            foreach (PlayDay day in daysPlayed)
            {
                int i = 2;
                foreach (DayGame game in day.Games)
                {
                    CustomEvent cEvent = new CustomEvent();
                    cEvent.Date = day.Date;
                    cEvent.EventText = game.Game;
                    cEvent.TooltipText = "Played " + game.Game + " for " + GameDatabase.calculateTimeString(game.Total_Time, false);
                    cEvent.EventLengthInHours = (float)game.Total_Time.TotalHours;
                    cEvent.Rank = i++;
                    calendar1.AddEvent(cEvent);
                }
            }
        }

        private void ThreadWorker()
        {
            List<PlayDay> daysPlayed = new List<PlayDay>();
            foreach (CachedSessionsList sessions in GameDatabase.CachedSessions)
            {
                foreach (SessionData session in sessions.Sessions)
                {
                    PlayDay day = daysPlayed.Find(delegate(PlayDay x) { return x.Date == session.Start_Time.Date; });
                    if (day != null)
                    {
                        DayGame game = day.Games.Find(delegate(DayGame x) { return x.Game == sessions.Game_Name; });
                        if (game != null)
                        {
                            game.AddTime(session.End_Time - session.Start_Time);
                        }
                        else
                        {
                            game = new DayGame(sessions.Game_Name);
                            game.AddTime(session.End_Time - session.Start_Time);
                            day.Games.Add(game);
                        }
                    }
                    else
                    {
                        day = new PlayDay(session.Start_Time.Date);
                        DayGame game = new DayGame(sessions.Game_Name);
                        game.AddTime(session.End_Time - session.Start_Time);
                        day.Games.Add(game);
                        daysPlayed.Add(day);
                    }
                }
            }
            //
            List<CustomEvent> events = new List<CustomEvent>();
            foreach (PlayDay day in daysPlayed)
            {
                int i = 2;
                foreach (DayGame game in day.Games)
                {
                    CustomEvent cEvent = new CustomEvent();
                    cEvent.Date = day.Date;
                    cEvent.EventText = game.Game;
                    cEvent.TooltipText = "Played " + game.Game + " for " + GameDatabase.calculateTimeString(game.Total_Time, false);
                    cEvent.EventLengthInHours = (float)game.Total_Time.TotalHours;
                    cEvent.Rank = i++;
                    events.Add(cEvent);
                }
            }
            this.Invoke(new AddEventsD(AddEvents), new[] { events });
        }

        private delegate void AddEventsD(List<CustomEvent> events);

        private void AddEvents(List<CustomEvent> events)
        {
            foreach (CustomEvent cEvent in events) { calendar1.AddEvent(cEvent); }
        }

        private void CalendarReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }
    }

    class PlayDay
    {
        DateTime _date;
        public List<DayGame> Games;

        public PlayDay(DateTime setDate)
        {
            _date = setDate;
            Games = new List<DayGame>();
        }

        public DateTime Date
        {
            get { return _date; }
        }
    }

    class DayGame
    {
        string _game;
        TimeSpan _game_time;

        public DayGame(string game_name)
        {
            _game = game_name;
            _game_time = new TimeSpan();
        }

        public string Game { get { return _game; } }

        public TimeSpan Total_Time { get { return _game_time; } }

        public void AddTime(TimeSpan time)
        {
            _game_time = _game_time.Add(time);
        }
    }
}
