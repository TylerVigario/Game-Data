using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using IniParser;
using IniParser.Model;

namespace Game_Data
{
    public delegate void LoadedD(List<GameData> games);
    public delegate void GameStartedD(GameData game, DateTime time);
    public delegate void GameRemovedD(GameData game);
    public delegate void GameRenamedD(GameData old_, GameData new_);
    public delegate void SessionD(GameData game, SessionData session);

    public static class GameDatabase
    {
        private static GameWatcher gameWatch;
        private static string dataFolder;
        public static List<GameData> GamesData;
        private static List<CachedSessionsList> CachedSessions;
        private static Timer cacheTimer;
        //
        public static event LoadedD Loaded;
        public static event GameStartedD GameStarted;
        public static event SessionD GameClosed;
        public static event GameRemovedD GameRemoved;
        public static event GameRenamedD GameRenamed;
        public static event SessionD SessionAdded;
        public static event SessionD SessionRemoved;

        #region Load Sections

        public static void Load()
        {
            new Thread(new ThreadStart(LoadWorker)).Start();
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
                foreach (string gameFolder in gameFolders)
                {
                    var parser = new FileIniDataParser();
                    IniData ini = parser.ReadFile(gameFolder + "\\sessions.ini");
                    GameData data = new GameData();
                    data.Name = ini["General"]["Name"];
                    CalculateGameStats(data, LoadGameSessions(ini["General"]["Name"]));
                    GamesData.Add(data);
                }
            }
            Loaded(GamesData);
            //
            gameWatch = new GameWatcher();
            GameWatcher.gameStarted += Game_Started;
            GameWatcher.gameClosed += GameWatcher_gameClosed;
            gameWatch.Start();
        }

        private static void GameWatcher_gameClosed(SupportedGame game)
        {
            AddSession(game);
        }

        public static List<SessionData> LoadGameSessions(string game_name)
        {
            string dataPath = getGameDataPath(game_name);
            //
            CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_Name == game_name; });
            if (cache != null) { cache.Reset();  return cache.Sessions; }
            //
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(dataPath + "sessions.ini");
            //
            if (data.Sections.Count > 0)
            {
                SessionData session;
                List<SessionData> sessions = new List<SessionData>();
                foreach (SectionData section in data.Sections)
                {
                    if (section.SectionName != "General")
                    {
                        session = new SessionData();
                        session.Start_Time = DateTime.Parse(section.Keys["Start_Time"]);
                        session.End_Time = DateTime.Parse(section.Keys["End_Time"]);
                        //
                        if (session.Start_Time > DateTime.MinValue && session.End_Time > DateTime.MinValue)
                        {
                            sessions.Add(session);
                        }
                    }
                }
                //
                CachedSessions.Add(new CachedSessionsList(game_name, sessions));
                cacheTimer.Change(0, 20000);
                //
                return sessions;
            }
            else { return null; }
        }

        public static void CalculateGameStats(GameData game, List<SessionData> sessions)
        {
            game.Reset();
            foreach (SessionData session in sessions)
            {
                if (session.Start_Time > DateTime.MinValue && session.End_Time > DateTime.MinValue)
                {
                    TimeSpan span = session.End_Time - session.Start_Time;
                    game.Total_Time = game.Total_Time.Add(span);
                    if (session.Start_Time < game.First_Played) { game.First_Played = session.Start_Time; ; }
                    if (session.End_Time > game.Last_Played) { game.Last_Played = session.End_Time; game.Last_Session_Time = span; }
                    if (span > game.Maximum_Session_Time) { game.Maximum_Session_Time = span; }
                    if (span < game.Minimum_Session_Time) { game.Minimum_Session_Time = span; }
                    game.Sessions++;
                }
            }
            if (game.Sessions > 0)
            {
                game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
            }
        }

        private static void cacheTimer_Tick(object state)
        {
            for (int i = 0; i < CachedSessions.Count; i++)
            {
                if (DateTime.Now >= CachedSessions[i].Expire_Time) { CachedSessions.Remove(CachedSessions[i]); }
            }
            if (CachedSessions.Count == 0) { cacheTimer.Change(Timeout.Infinite, Timeout.Infinite); }
        }

        public static void Dispose()
        {
            gameWatch.Stop();
        }

        #endregion

        #region Add Sections

        private static void Game_Started(SupportedGame sGame, DateTime time)
        {
            string dataPath = getGameDataPath(sGame.Game_Name);
            //
            if (!Directory.Exists(dataPath)) { Directory.CreateDirectory(dataPath); }
            if (!File.Exists(dataPath + "sessions.ini"))
            {
                IniData ini = new IniData();
                ini.Sections.AddSection("General");
                ini["General"].AddKey("Name", sGame.Game_Name);
                var parser = new FileIniDataParser();
                parser.WriteFile(dataPath + "sessions.ini", ini);
                //
                GameData game = new GameData();
                game.Name = sGame.Game_Name;
                game.Sessions = 1;
                game.Last_Played = time;
                GamesData.Add(game);
                GameStarted(game, time);
            }
            else
            {
                GameData game = GamesData.Find(delegate(GameData x) { return x.Name == sGame.Game_Name; });
                game.Sessions++;
                game.Last_Played = time;
                GameStarted(game, time);
            }
        }

        public static void AddSession(SupportedGame sGame)
        {
            GameData game = GamesData.Find(delegate (GameData x) { return x.Name == sGame.Game_Name; });
            SessionData session = new SessionData(game.Last_Played, DateTime.Now);
            AddSession(game, session);
            GameClosed(game, session);
        }

        public static void AddSession(string game_name, SessionData session)
        {
            GameData game = GamesData.Find(delegate (GameData x) { return x.Name == game_name; });
            AddSession(game, session);
        }

        public static void AddSession(GameData game, SessionData session)
        {
            if (session.Time_Span.TotalSeconds < Settings.Session_Threshold) return;
            string dataPath = getGameDataPath(game.Name);
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(dataPath + "sessions.ini");
            //
            TimeSpan span = session.End_Time - session.Start_Time;
            string section = getSectionName(session.End_Time);
            ini.Sections.AddSection(section);
            ini[section].AddKey("Start_Time", session.Start_Time.ToString());
            ini[section].AddKey("End_Time", session.End_Time.ToString());
            parser.WriteFile(dataPath + "sessions.ini", ini);
            //
            if (game.First_Played == DateTime.MinValue) { game.First_Played = session.Start_Time; }
            game.Last_Played = session.End_Time;
            game.Total_Time = game.Total_Time.Add(span);
            if (span > game.Maximum_Session_Time) { game.Maximum_Session_Time = span; }
            if (span < game.Minimum_Session_Time) { game.Minimum_Session_Time = span; }
            game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
            game.Last_Session_Time = span;
            //
            SessionAdded(game, session);
            //
            CachedSessionsList cache = CachedSessions.Find(delegate (CachedSessionsList x) { return x.Game_Name == game.Name; });
            if (cache != null) { cache.Reset(); cache.Sessions.Add(session); }
            else
            {
                List<SessionData> sessions = new List<SessionData>();
                sessions.Add(session);
                CachedSessions.Add(new CachedSessionsList(game.Name, sessions));
            }
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
            GameRemoved(game);
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
                //
                GameRemoved(game);
            }
        }

        public static void RemoveSession(string game_name, SessionData session)
        {
            string dataPath = getGameDataPath(game_name);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            //
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(dataPath + "sessions.ini");
            ini.Sections.RemoveSection(getSectionName(session.End_Time));
            parser.WriteFile(dataPath + "sessions.ini", ini);
            //
            CachedSessionsList cache = CachedSessions.Find(delegate (CachedSessionsList x) { return x.Game_Name == game_name; });
            if (cache != null) { cache.Reset(); cache.Sessions.Remove(cache.Sessions.Find(delegate (SessionData x) { return x.Start_Time == session.Start_Time; })); }
            //
            if (game.Minimum_Session_Time == session.Time_Span || game.Minimum_Session_Time == session.Time_Span || game.Last_Session_Time == session.Time_Span || game.Last_Played == session.End_Time)
            {
                CalculateGameStats(game, LoadGameSessions(game_name));
            }
            else
            {
                game.Total_Time = game.Total_Time.Subtract(session.Time_Span);
                game.Sessions--;
                game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
            }
            //
            SessionRemoved(game, session);
        }

        #endregion

        #region Edit Sections

        public static void RenameGame(SupportedGame old_, SupportedGame new_)
        {
            GameData game = GamesData.Find(delegate (GameData x) { return x.Name == old_.Game_Name; });
            GameData nGame = game;
            nGame.Name = new_.Game_Name;
            GameRenamed(game, nGame);
            game.Name = new_.Game_Name;
            //
            string dataPath = getGameDataPath(old_.Game_Name);
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(dataPath + "sessions.ini");
            ini["General"]["Name"] = new_.Game_Name;
            parser.WriteFile(dataPath + "sessions.ini", ini);
            Directory.Move(dataPath, dataFolder + "\\" + gameNameSaferizer(new_.Game_Name));
        }

        /*public static SessionListItem MergeSessions(string game_name, List<SessionListItem> sessions)
        {
            string dataPath = getGameDataPath(game_name);
            GameData game = GamesData.Find(delegate(GameData x) { return x.Name == game_name; });
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(dataPath + "sessions.ini");
            //
            SessionData session_data = new SessionData();
            foreach (SessionListItem session in sessions)
            {
                if (session.Date < session_data.Start_Time) { session_data.Start_Time = session.Date; }
                DateTime end = session.Date.Add(session.Time);
                if (end > session_data.End_Time) { session_data.End_Time = end; }
                //
                ini.Sections.RemoveSection(getSectionName(end));
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
        }*/

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

        public TimeSpan Time_Span
        {
            get
            {
                if (_start_time == DateTime.MinValue || _end_time == DateTime.MinValue) { return TimeSpan.Zero; }
                return _end_time.Subtract(_start_time);
            }
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
