using System;
using System.Collections.Generic;
using System.IO;

namespace Game_Data
{
    public delegate void GameD(GameData game);
    public delegate void EmptyD();

    public static class GameDatabase
    {
        public static List<GameData> GamesData;
        //
        private static string dataFolder;
        //
        public static event GameD RefreshGame;
        public static event EmptyD FullRefresh;

        #region Load Sections

        public static void Load()
        {
            GamesData = new List<GameData>();
            dataFolder = Settings.Save_Path + "\\data";
            if (!Directory.Exists(dataFolder)) { Directory.CreateDirectory(dataFolder); }
            //
            string[] gameFolders = Directory.GetDirectories(dataFolder);
            if (gameFolders.Length > 0)
            {
                foreach (string gameFolder in gameFolders)
                {
                    IniFile ini = new IniFile(gameFolder + "\\sessions.ini");
                    //
                    GameData data = new GameData();
                    data.Name = ini.GetString("General", "Name", "");
                    //
                    string[] sections = ini.GetSectionNames();
                    if (sections.Length > 1)
                    {
                        DateTime start;
                        DateTime end;
                        //
                        foreach (string section in sections)
                        {
                            if (section != "General")
                            {
                                start = DateTime.MinValue;
                                end = DateTime.MinValue;
                                //
                                try
                                {
                                    string med = ini.GetString(section, "Start_Time", "");
                                    start = DateTime.Parse(med);
                                    med = ini.GetString(section, "End_Time", "");
                                    end = DateTime.Parse(med);
                                }
                                catch { }
                                //
                                if (start > DateTime.MinValue && end > DateTime.MinValue)
                                {
                                    TimeSpan span = end - start;
                                    data.Total_Time = data.Total_Time.Add(span);
                                    if (start < data.First_Played) { data.First_Played = start; }
                                    if (end > data.Last_Played) { data.Last_Played = end; }
                                    if (span > data.Maximum_Session_Time) { data.Maximum_Session_Time = span; }
                                    if (span < data.Minimum_Session_Time) { data.Minimum_Session_Time = span; }
                                    //
                                    data.Sessions++;
                                }
                            }
                        }
                    }
                    //
                    GamesData.Add(data);
                }
            }
        }

        public static List<SessionData> LoadSessions(GameData game)
        {
            string dataPath = getGameDataPath(game.Name);
            IniFile ini = new IniFile(dataPath + "sessions.ini");
            //
            string[] sections = ini.GetSectionNames();
            if (sections.Length > 1)
            {
                SessionData session;
                List<SessionData> sessions = new List<SessionData>();
                foreach (string section in sections)
                {
                    if (section != "General")
                    {
                        session = new SessionData();
                        //
                        try
                        {
                            string med = ini.GetString(section, "Start_Time", "");
                            session.Start_Time = DateTime.Parse(med);
                            med = ini.GetString(section, "End_Time", "");
                            session.End_Time = DateTime.Parse(med);
                            session.Average_CPU_Usage = Convert.ToInt32(ini.GetString(section, "Average_CPU_Usage", "0"));
                            session.Average_Working_Set = Convert.ToInt32(ini.GetString(section, "Average_Working_Set", "0"));
                        }
                        catch { }
                        //
                        if (session.Start_Time > DateTime.MinValue && session.End_Time > DateTime.MinValue)
                        {
                            sessions.Add(session);
                        }
                    }
                }
                return sessions;
            }
            else { return null; }
        }

        #endregion

        #region Add Sections

        #region Internal Recording Functions

        public static void GameStarted(string game_name)
        {
            string dataPath = getGameDataPath(game_name);
            //
            CheckDataFolder(dataPath, game_name, true);
        }

        public static void AddRecordedSession(GameData game, RunningSession session)
        {
            
            string dataPath = getGameDataPath(game.Name);
            //
            CheckDataFolder(dataPath, game.Name);
            //
            UpdateGame(game);
        }

        #endregion

        public static void AddSession(GameData game, SessionData session)
        {
            string dataPath = getGameDataPath(game.Name);

            //
            UpdateGame(game);
        }

        private static GameData CheckDataFolder(string game_data_path, string game_name, bool push_update = false)
        {
            if (!Directory.Exists(game_data_path)) { Directory.CreateDirectory(game_data_path); }
            if (!File.Exists(game_data_path + "sessions.ini"))
            {
                File.WriteAllLines(game_data_path + "sessions.ini", new string[] { "[General]", "Name=" + game_name });
                GameData game = new GameData();
                game.Name = game_name;
                GamesData.Add(game);
            }
        }

        #endregion

        #region Remove Sections

        public static void RemoveGame(GameData game)
        {
            string dataPath = getGameDataPath(game.Name);

            //
            UpdateList();
        }

        public static void RemoveSession(GameData game, SessionData session)
        {
            string dataPath = getGameDataPath(game.Name);

            //
            UpdateList();
        }

        #endregion

        #region Edit Sections

        public static SessionData MergeSessions()
        {
            string dataPath = getGameDataPath(game.Name);

            //
            UpdateList();
        }

        #endregion

        #region Helper Functions

        public static string calculateTimeString(TimeSpan span, bool compact = true)
        {
            string d, h, m, s;
            if (compact)
            {
                d = "d";
                h = "h";
                m = "m";
                s = "s";
            }
            else
            {
                if (span.Days > 1) { d = " days"; }
                else { d = " day"; }
                if (span.Hours > 1) { h = " hours"; }
                else { h = " hour"; }
                if (span.Minutes > 1) { m = " minutes"; }
                else { m = " minute"; }
                if (span.Seconds > 1) { s = " seconds"; }
                else { s = " second"; }
            }
            //
            if (span.Days > 0)
            {
                return span.Days + d + " " + ((span.Hours > 0) ? span.Hours + h : (span.Minutes > 0) ? span.Minutes + m : (span.Seconds > 0) ? span.Seconds + s : "");
            }
            else if (span.Hours > 0)
            {
                return span.Hours + h + " " + ((span.Minutes > 0) ? span.Minutes + m : (span.Seconds > 0) ? span.Seconds + s : "");
            }
            else if (span.Minutes > 0)
            {
                return span.Minutes + m + " " + ((span.Seconds > 0) ? span.Seconds + s : "");
            }
            else { return span.TotalSeconds + s; }
        }

        public static string calculateLastPlayedString(DateTime date, bool exact_date = false)
        {
            if (date == DateTime.MinValue) { return "Never"; }
            else if (date.Date == DateTime.Today) { return date.ToShortTimeString(); }
            else if (exact_date) { return date.ToShortDateString(); }
            else
            {
                double exact_days = (DateTime.Today - date.Date).TotalDays;
                int days = (int)Math.Floor(exact_days);
                if (days == 1) { return "Yesterday"; }
                else if (days >= 30)
                {
                    int months = (int)Math.Round(exact_days / 30.4375);
                    if (months == 1) { return "1 month ago"; }
                    else { return months.ToString() + " months ago"; }
                }
                else if (days >= 7)
                {
                    int weeks = (int)Math.Round(exact_days / 7);
                    if (weeks == 1) { return "1 week ago"; }
                    else { return weeks.ToString() + " weeks ago"; }
                }
                else { return days.ToString() + " days ago"; }
            }
        }

        public static string getGameDataPath(string game_name)
        {
            return dataFolder + "\\" + gameNameSaferizer(game_name) + "\\";
        }

        public static string gameNameSaferizer(string game_name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                game_name = game_name.Replace(c.ToString(), "");
            }
            return game_name.ToLower().Replace(" ", "_");
        }

        public static string getSectionName(DateTime end_date)
        {
            return (end_date.Ticks / 600000000).ToString();
        }

        #endregion
    }

    #region GameData

    public class GameData
    {
        private string _name = "";
        private int _sessions = 0;
        private DateTime _first_played = DateTime.MaxValue;
        private DateTime _last_played = DateTime.MinValue;
        private TimeSpan _total_time = TimeSpan.Zero;
        private TimeSpan _maximum_session_time = TimeSpan.Zero;
        private TimeSpan _minimum_session_time = TimeSpan.MaxValue;
        private TimeSpan _average_session_time = TimeSpan.Zero;
        private TimeSpan _last_session_time = TimeSpan.Zero;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Sessions
        {
            get { return _sessions; }
            set { _sessions = value; }
        }

        public DateTime First_Played
        {
            get { return _first_played; }
            set { _first_played = value; }
        }

        public DateTime Last_Played
        {
            get { return _last_played; }
            set { _last_played = value; }
        }

        public TimeSpan Total_Time
        {
            get { return _total_time; }
            set { _total_time = value; }
        }

        public TimeSpan Maximum_Session_Time
        {
            get { return _maximum_session_time; }
            set { _maximum_session_time = value; }
        }

        public TimeSpan Minimum_Session_Time
        {
            get { return _minimum_session_time; }
            set { _minimum_session_time = value; }
        }

        public TimeSpan Average_Session_Time
        {
            get { return _average_session_time; }
            set { _average_session_time = value; }
        }

        public TimeSpan Last_Session_Time
        {
            get { return _last_session_time; }
            set { _last_session_time = value; }
        }
    }

    #endregion

    #region SessionData

    public class SessionData
    {
        DateTime _start_time = DateTime.MinValue;
        DateTime _end_time = DateTime.MinValue;
        long _average_working_set = 0;
        double _average_cpu_usage = 0;

        public SessionData() { }

        public SessionData(DateTime start, DateTime end)
        {
            _start_time = start;
            _end_time = end;
        }

        public DateTime Start_Time
        {
            get { return _start_time; }
            set { _start_time = value; }
        }

        public DateTime End_Time
        {
            get { return _end_time; }
            set { _end_time = value; }
        }

        public double Average_CPU_Usage
        {
            get { return _average_cpu_usage; }
            set { _average_cpu_usage = value; }
        }

        public long Average_Working_Set
        {
            get { return _average_working_set; }
            set { _average_working_set = value; }
        }
    }

    #endregion
}
