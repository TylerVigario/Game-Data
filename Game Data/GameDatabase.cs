using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Game_Data
{
    public delegate void GameD(GameData game);
    public delegate void EmptyD();

    public static class GameDatabase
    {
        private static string dataFolder;
        public static List<GameData> GamesData;
        public static List<CachedSessionsList> CachedSessions;
        private static Timer cacheTimer;
        public static event EmptyD Loaded;
        public static event GameD RefreshGame;
        public static event EmptyD FullRefresh;

        #region Load Sections

        public static void Load()
        {
            new Thread(new ThreadStart(LoadWorker)).Start();
        }

        private static void cacheTimer_Tick(object state)
        {
            for (int i = 0; i < CachedSessions.Count; i++)
            {
                if (DateTime.Now >= CachedSessions[i].Expire_Time) { CachedSessions.Remove(CachedSessions[i]); }
            }
            if (CachedSessions.Count == 0) { cacheTimer.Change(Timeout.Infinite, Timeout.Infinite); }
        }

        public static void LoadWorker()
        {
            GamesData = new List<GameData>();
            CachedSessions = new List<CachedSessionsList>();
            cacheTimer = new Timer(new TimerCallback(cacheTimer_Tick));
            dataFolder = Settings.Save_Path + "\\data";
            //
            if (!Directory.Exists(dataFolder)) { Directory.CreateDirectory(dataFolder); }
            string[] gameFolders = Directory.GetDirectories(dataFolder);
            if (gameFolders.Length > 0)
            {
                Exception error = null;
                foreach (string gameFolder in gameFolders)
                {
                    IniFile ini = new IniFile(gameFolder + "\\sessions.ini");
                    List<SessionData> ses = new List<SessionData>();
                    //
                    GameData data = new GameData();
                    data.Name = ini.GetString("General", "Name", "");
                    //
                    string[] sections = ini.GetSectionNames();
                    if (sections.Length > 1)
                    {
                        foreach (string section in sections)
                        {
                            if (section != "General")
                            {
                                SessionData s = new SessionData();
                                //
                                try
                                {
                                    string med = ini.GetString(section, "Start_Time", "");
                                    s.Start_Time = DateTime.Parse(med);
                                    med = ini.GetString(section, "End_Time", "");
                                    s.End_Time = DateTime.Parse(med);
                                    s.Average_CPU_Usage = ini.GetDouble(section, "Average_CPU_Usage", 0);
                                    s.Average_Working_Set = Convert.ToInt64(ini.GetString(section, "Average_Working_Set", "0"));
                                    //
                                    if (s.Start_Time > DateTime.MinValue && s.End_Time > DateTime.MinValue)
                                    {
                                        TimeSpan span = s.End_Time - s.Start_Time;
                                        data.Total_Time = data.Total_Time.Add(span);
                                        if (s.Start_Time < data.First_Played) { data.First_Played = s.Start_Time; }
                                        if (s.End_Time > data.Last_Played) { data.Last_Played = s.End_Time; data.Last_Session_Time = span; }
                                        if (span > data.Maximum_Session_Time) { data.Maximum_Session_Time = span; }
                                        if (span < data.Minimum_Session_Time) { data.Minimum_Session_Time = span; }
                                        data.Sessions++;
                                        ses.Add(s);
                                    }
                                }
                                catch (Exception ex) { error = ex; }
                            }
                        }
                        data.Average_Session_Time = TimeSpan.FromSeconds(data.Total_Time.TotalSeconds / data.Sessions);
                    }
                    //
                    GamesData.Add(data);
                    CachedSessions.Add(new CachedSessionsList(data.Name, ses));
                }
                if (error != null) { Game_Data.Program.ApplicationThreadException(null, new System.Threading.ThreadExceptionEventArgs(error)); }
                cacheTimer.Change(0, 20000);
            }
            Loaded();
        }

        public static List<SessionData> LoadSessions(string game_name)
        {
            string dataPath = getGameDataPath(game_name);
            //
            CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_Name == game_name; });
            if (cache != null) { cache.Reset();  return cache.Sessions; }
            //
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            IniFile ini = new IniFile(dataPath + "sessions.ini");
            game.Reset();
            //
            string[] sections = ini.GetSectionNames();
            if (sections.Length > 1)
            {
                SessionData session;
                List<SessionData> sessions = new List<SessionData>();
                Exception error = null;
                foreach (string section in sections)
                {
                    if (section != "General")
                    {
                        try
                        {
                            session = new SessionData();
                            string med = ini.GetString(section, "Start_Time", "");
                            session.Start_Time = DateTime.Parse(med);
                            med = ini.GetString(section, "End_Time", "");
                            session.End_Time = DateTime.Parse(med);
                            session.Average_CPU_Usage = ini.GetDouble(section, "Average_CPU_Usage", 0);
                            session.Average_Working_Set = Convert.ToInt64(ini.GetString(section, "Average_Working_Set", "0"));
                            //
                            if (session.Start_Time > DateTime.MinValue && session.End_Time > DateTime.MinValue)
                            {
                                TimeSpan span = session.End_Time - session.Start_Time;
                                game.Total_Time = game.Total_Time.Add(span);
                                if (session.Start_Time < game.First_Played) { game.First_Played = session.Start_Time; ; }
                                if (session.End_Time > game.Last_Played) { game.Last_Played = session.End_Time; game.Last_Session_Time = span; }
                                if (span > game.Maximum_Session_Time) { game.Maximum_Session_Time = span; }
                                if (span < game.Minimum_Session_Time) { game.Minimum_Session_Time = span; }
                                game.Sessions++;
                                //
                                sessions.Add(session);
                            }
                        }
                        catch (Exception ex) { error = ex; }
                    }
                }
                if (error != null) { Game_Data.Program.ApplicationThreadException(null, new System.Threading.ThreadExceptionEventArgs(error)); }
                game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
                //
                CachedSessions.Add(new CachedSessionsList(game_name, sessions));
                cacheTimer.Change(0, 20000);
                //
                return sessions;
            }
            else { return null; }
        }

        #endregion

        #region Add Sections

        #region Internal Recording Functions

        public static bool GameStarted(string game_name)
        {
            string dataPath = getGameDataPath(game_name);
            //
            if (!Directory.Exists(dataPath)) { Directory.CreateDirectory(dataPath); }
            if (!File.Exists(dataPath + "sessions.ini"))
            {
                File.WriteAllLines(dataPath + "sessions.ini", new string[] { "[General]", "Name=" + game_name });
                GameData game = new GameData();
                game.Name = game_name;
                game.Sessions = 1;
                game.Last_Played = DateTime.Now.AddMinutes(10);
                GamesData.Add(game);
                FullRefresh();
                return true;
            }
            else
            {
                GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
                game.Sessions++;
                game.Last_Played = DateTime.Now.AddMinutes(10);
                RefreshGame(game);
                return false;
            }
        }

        public static SessionData AddRecordedSession(string game_name, RunningSession session)
        {
            string dataPath = getGameDataPath(game_name);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            //
            TimeSpan span = session.Data.End_Time - session.Data.Start_Time;
            if (span.TotalSeconds >= Settings.Session_Threshold)
            { 
                IniFile ini = new IniFile(dataPath + "sessions.ini");
                string section = getSectionName(session.Data.End_Time);
                ini.WriteValue(section, "Start_Time", session.Data.Start_Time.ToString());
                ini.WriteValue(section, "End_Time", session.Data.End_Time.ToString());
                if (session.CollectionData.Count > 0)
                {
                    ini.WriteValue(section, "Average_Working_Set", session.Data.Average_Working_Set);
                    ini.WriteValue(section, "Average_CPU_Usage", session.Data.Average_CPU_Usage);
                    string saved_data = "";
                    foreach (CollectionData cData in session.CollectionData)
                    {
                        if (saved_data == "") { saved_data += cData.CPU_Usage + ":" + cData.Working_Set; }
                        else { saved_data += "|" + cData.CPU_Usage + ":" + cData.Working_Set; }
                    }
                    File.WriteAllText(dataPath + section + ".txt", saved_data);
                }
                //
                if (game.First_Played == DateTime.MinValue) { game.First_Played = session.Data.Start_Time; }
                game.Last_Played = session.Data.End_Time;
                game.Total_Time = game.Total_Time.Add(span);
                if (span > game.Maximum_Session_Time) { game.Maximum_Session_Time = span; }
                if (span < game.Minimum_Session_Time) { game.Minimum_Session_Time = span; }
                game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
                game.Last_Session_Time = span;
                //
                CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_Name == game_name; });
                if (cache != null) { cache.Reset(); cache.Sessions.Add(session.Data); }
                RefreshGame(game);
                return session.Data;
            }
            else
            {
                removeCachedSessions(game.Name);
                LoadSessions(game.Name);
                RefreshGame(game);
                return null;
            }
        }

        #endregion

        public static void AddSession(string game_name, SessionData session)
        {
            string dataPath = getGameDataPath(game_name);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            IniFile ini = new IniFile(dataPath + "sessions.ini");
            //
            TimeSpan span = session.End_Time - session.Start_Time;
            string section = getSectionName(session.End_Time);
            ini.WriteValue(section, "Start_Time", session.Start_Time.ToString());
            ini.WriteValue(section, "End_Time", session.End_Time.ToString());
            //
            if (game.First_Played == DateTime.MinValue) { game.First_Played = session.Start_Time; }
            game.Last_Played = session.End_Time;
            game.Total_Time = game.Total_Time.Add(span);
            if (span > game.Maximum_Session_Time) { game.Maximum_Session_Time = span; }
            if (span < game.Minimum_Session_Time) { game.Minimum_Session_Time = span; }
            game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
            game.Last_Session_Time = span;
            //
            CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_Name == game_name; });
            if (cache != null) { cache.Reset(); cache.Sessions.Add(session); }
            RefreshGame(game);
        }

        #endregion

        #region Remove Sections

        public static void RemoveGame(GameData game)
        {
            string dataPath = getGameDataPath(game.Name);
            //
            Directory.Delete(dataPath, true);
            removeCachedSessions(game.Name);
            GamesData.Remove(game);
            //
            FullRefresh();
        }

        public static void RemoveGames(List<GameData> games)
        {
            foreach (GameData game in games)
            {
                string dataPath = getGameDataPath(game.Name);
                //
                Directory.Delete(dataPath, true);
                removeCachedSessions(game.Name);
                GamesData.Remove(game);
            }
            //
            FullRefresh();
        }

        public static void RemoveSession(string game_name, SessionListItem session)
        {
            string dataPath = getGameDataPath(game_name);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            IniFile ini = new IniFile(dataPath + "sessions.ini");
            //
            ini.DeleteSection(getSectionName(session.Date.Add(session.Time)));
            //
            if (game.Minimum_Session_Time == session.Time || game.Minimum_Session_Time == session.Time || game.Last_Session_Time == session.Time || game.Last_Played == session.Date)
            {
                removeCachedSessions(game_name);
                LoadSessions(game_name);
            }
            else
            {
                game.Total_Time = game.Total_Time.Subtract(session.Time);
                game.Sessions--;
                game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
                //
                CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_Name == game_name; });
                if (cache != null) { cache.Reset(); cache.Sessions.Remove(cache.Sessions.Find(delegate(SessionData x) { return x.Start_Time == session.Date; })); }
                //
                RefreshGame(game);
            }
        }

        public static void RemoveSessions(string game_name, List<SessionListItem> sessions)
        {
            string dataPath = getGameDataPath(game_name);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            IniFile ini = new IniFile(dataPath + "sessions.ini");
            //
            bool fullRefresh = false;
            foreach (SessionListItem session in sessions)
            {
                ini.DeleteSection(getSectionName(session.Date.Add(session.Time)));
                //
                if (game.Minimum_Session_Time == session.Time || game.Minimum_Session_Time == session.Time || game.Last_Session_Time == session.Time || game.Last_Played == session.Date)
                {
                    fullRefresh = true;
                    break;
                }
                else
                {
                    game.Total_Time = game.Total_Time.Subtract(session.Time);
                    game.Sessions--;
                    //
                    CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_Name == game_name; });
                    if (cache != null) { cache.Reset(); cache.Sessions.Remove(cache.Sessions.Find(delegate(SessionData x) { return x.Start_Time == session.Date; })); }
                }
            }
            if (fullRefresh)
            {
                removeCachedSessions(game_name);
                LoadSessions(game_name);
            }
            else
            {
                game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
                RefreshGame(game);
            }
        }

        #endregion

        #region Edit Sections

        public static void RenameGame(string oldName, string newName)
        {
            string dataPath = getGameDataPath(oldName);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == oldName; });
            if (Directory.Exists(dataPath))
            {
                IniFile ini = new IniFile(dataPath + "sessions.ini");
                ini.WriteValue("General", "Name", newName);
                Directory.Move(dataPath, dataFolder + "\\" + gameNameSaferizer(newName));
            }
            if (game != null) { game.Name = newName; FullRefresh(); }
        }

        public static SessionListItem MergeSessions(string game_name, List<SessionListItem> sessions)
        {
            string dataPath = getGameDataPath(game_name);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            IniFile ini = new IniFile(dataPath + "sessions.ini");
            //
            SessionData session_data = new SessionData();
            foreach (SessionListItem session in sessions)
            {
                if (session.Date < session_data.Start_Time) { session_data.Start_Time = session.Date; }
                DateTime end = session.Date.Add(session.Time);
                if (end > session_data.End_Time) { session_data.End_Time = end; }
                //
                ini.DeleteSection(getSectionName(end));
            }
            //
            string new_section = getSectionName(session_data.End_Time);
            ini.WriteValue(new_section, "Start_Time", session_data.Start_Time.ToString());
            ini.WriteValue(new_section, "End_Time", session_data.End_Time.ToString());
            //
            removeCachedSessions(game_name);
            LoadSessions(game.Name);
            RefreshGame(game);
            return new SessionListItem(session_data);
        }

        #endregion

        #region Helper Functions

        public static string calculateTimeString(TimeSpan span, bool compact = true)
        {
            // TODO: Add support Time Display Level - Settings.Time_Display_Level
            //
            string d, h, m, s;
            if (compact) { d = "d"; h = "h"; m = "m"; s = "s"; }
            else
            {
                if (span.Days > 1) { d = " days"; } else { d = " day"; }
                if (span.Hours > 1) { h = " hours"; } else { h = " hour"; }
                if (span.Minutes > 1) { m = " minutes"; } else { m = " minute"; }
                if (span.Seconds > 1) { s = " seconds"; } else { s = " second"; }
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
            else { return span.Seconds + s; }
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
                else if (Settings.Last_Played_Display_Level == 2 && days >= 30)
                {
                    int months = (int)Math.Round(exact_days / 30.4375);
                    if (months == 1) { return "1 month ago"; }
                    else { return months.ToString() + " months ago"; }
                }
                else if (Settings.Last_Played_Display_Level == 1 && days >= 7)
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

        private static void removeCachedSessions(string game_name)
        {
            CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_Name == game_name; });
            if (cache != null) { CachedSessions.Remove(cache); }
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

        public void Reset()
        {
            _sessions = 0;
            _first_played = DateTime.MaxValue;
            _last_played = DateTime.MinValue;
            _total_time = TimeSpan.Zero;
            _maximum_session_time = TimeSpan.Zero;
            _minimum_session_time = TimeSpan.MaxValue;
            _average_session_time = TimeSpan.Zero;
            _last_session_time = TimeSpan.Zero;
        }

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

    #region CachedSessionsList

    public class CachedSessionsList
    {
        public string Game_Name;
        public List<SessionData> Sessions;
        public DateTime Expire_Time;

        public CachedSessionsList(string name, List<SessionData> s)
        {
            Game_Name = name;
            Sessions = s;
            Expire_Time = DateTime.Now.AddMinutes(2);
        }

        public void Reset()
        {
            Expire_Time = DateTime.Now.AddMinutes(2);
        }
    }

    #endregion
}
