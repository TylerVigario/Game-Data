using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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
        private static System.Timers.Timer cacheTimer;
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
            cacheTimer = new System.Timers.Timer();
            cacheTimer.Elapsed += CacheTimer_Elapsed;
            cacheTimer.Interval = 15000;
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
                    GameData data = new GameData(Path.GetFileName(gameFolder), ini["General"]["Name"]);
                    CalculateGameStats(data, LoadGameSessions(data.ID));
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

        public static List<SessionData> LoadGameSessions(string game_id)
        {
            CachedSessionsList cache = CachedSessions.Find(delegate(CachedSessionsList x) { return x.Game_ID == game_id; });
            if (cache != null) { cache.Reset();  return cache.Sessions; }
            //
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(dataFolder + "\\" + game_id + "\\sessions.ini");
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
                        sessions.Add(session);
                    }
                }
                //
                CachedSessions.Add(new CachedSessionsList(game_id, sessions));
                cacheTimer.Enabled = true;
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

        private static void CacheTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<CachedSessionsList> removeList = new List<CachedSessionsList>();
            foreach (CachedSessionsList session in CachedSessions)
            {
                if (DateTime.Now >= session.Expire_Time) { removeList.Add(session); }
            }
            foreach (CachedSessionsList session in removeList)
            {
                CachedSessions.Remove(session);
            }
            if (CachedSessions.Count == 0) { cacheTimer.Enabled = false; }
        }

        public static void Dispose()
        {
            gameWatch.Stop();
        }

        #endregion

        #region Add Sections

        private static void Game_Started(SupportedGame sGame, DateTime time)
        {
            string dataPath = dataFolder + "\\" + sGame.ID + "\\";
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
                GameData game = new GameData(sGame.ID, sGame.Game_Name);
                game.Sessions = 1;
                game.Last_Played = time;
                GamesData.Add(game);
                GameStarted(game, time);
            }
            else
            {
                GameData game = GamesData.Find(delegate(GameData x) { return x.ID == sGame.ID; });
                game.Sessions++;
                game.Last_Played = time;
                GameStarted(game, time);
            }
        }

        public static void AddSession(SupportedGame sGame)
        {
            GameData game = GamesData.Find(delegate (GameData x) { return x.ID == sGame.ID; });
            SessionData session = new SessionData(game.Last_Played, DateTime.Now);
            AddSession(game, session);
            GameClosed(game, session);
        }

        public static void AddSession(string game_id, SessionData session)
        {
            GameData game = GamesData.Find(delegate (GameData x) { return x.ID == game_id; });
            game.Sessions++;
            AddSession(game, session);
        }

        public static bool AddSession(GameData game, SessionData session)
        {
            if (session.Time_Span.TotalSeconds < Settings.Session_Threshold)
            {
                CalculateGameStats(game, LoadGameSessions(game.ID));
                SessionAdded(game, session);
                return false;
            }
            string dataPath = dataFolder + "\\" + game.ID + "\\";
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
            if (session.End_Time > game.Last_Played)
            {
                game.Last_Played = session.End_Time;
                game.Last_Session_Time = span;
            }
            game.Total_Time = game.Total_Time.Add(span);
            if (span > game.Maximum_Session_Time) { game.Maximum_Session_Time = span; }
            if (span < game.Minimum_Session_Time) { game.Minimum_Session_Time = span; }
            game.Average_Session_Time = TimeSpan.FromSeconds(game.Total_Time.TotalSeconds / game.Sessions);
            //
            SessionAdded(game, session);
            //
            CachedSessionsList cache = CachedSessions.Find(delegate (CachedSessionsList x) { return x.Game_ID == game.ID; });
            if (cache != null) { cache.Reset(); cache.Sessions.Add(session); }
            else
            {
                List<SessionData> sessions = new List<SessionData>();
                sessions.Add(session);
                CachedSessions.Add(new CachedSessionsList(game.Name, sessions));
            }
            return true;
        }

        #endregion

        #region Remove Sections

        public static void RemoveGame(GameData game)
        {
            string dataPath = dataFolder + "\\" + game.ID + "\\";
            //
            Directory.Delete(dataPath, true);
            GamesData.Remove(game);
            CachedSessionsList cache = CachedSessions.Find(delegate (CachedSessionsList x) { return x.Game_ID == game.ID; });
            if (cache != null) { CachedSessions.Remove(cache); }
            //
            GameRemoved(game);
        }

        public static void RemoveGames(List<GameData> games)
        {
            foreach (GameData game in games)
            {
                RemoveGame(game);
            }
        }

        public static void RemoveSession(string game_id, SessionData session)
        {
            string dataPath = dataFolder + "\\" + game_id + "\\";
            GameData game = GamesData.Find(delegate(GameData x) { return x.ID == game_id; });
            //
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(dataPath + "sessions.ini");
            ini.Sections.RemoveSection(getSectionName(session.End_Time));
            parser.WriteFile(dataPath + "sessions.ini", ini);
            //
            CachedSessionsList cache = CachedSessions.Find(delegate (CachedSessionsList x) { return x.Game_ID == game_id; });
            if (cache != null) { cache.Reset(); cache.Sessions.Remove(cache.Sessions.Find(delegate (SessionData x) { return x.Start_Time == session.Start_Time; })); }
            //
            if (game.Minimum_Session_Time == session.Time_Span || game.Minimum_Session_Time == session.Time_Span || game.Last_Session_Time == session.Time_Span || game.Last_Played == session.End_Time)
            {
                CalculateGameStats(game, LoadGameSessions(game_id));
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
            GameData game = GamesData.Find(delegate (GameData x) { return x.ID == old_.ID; });
            GameData nGame = game;
            nGame.Name = new_.Game_Name;
            GameRenamed(game, nGame);
            game.Name = new_.Game_Name;
            //
            string dataPath = dataFolder + "\\" + game.ID + "\\";
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(dataPath + "sessions.ini");
            ini["General"]["Name"] = new_.Game_Name;
            parser.WriteFile(dataPath + "sessions.ini", ini);
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

        public static string getSectionName(DateTime end_date)
        {
            return (end_date.Ticks / 600000000).ToString();
        }

        #endregion
    }

    #region GameData

    public class GameData
    {
        private string _id;
        private string _name;
        private int _sessions = 0;
        private DateTime _first_played = DateTime.MaxValue;
        private DateTime _last_played = DateTime.MinValue;
        private TimeSpan _total_time = TimeSpan.Zero;
        private TimeSpan _maximum_session_time = TimeSpan.Zero;
        private TimeSpan _minimum_session_time = TimeSpan.MaxValue;
        private TimeSpan _average_session_time = TimeSpan.Zero;
        private TimeSpan _last_session_time = TimeSpan.Zero;

        public GameData(string id, string name)
        {
            _id = id;
            _name = name;
        }

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

        public string ID
        {
            get { return _id; }
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
        DateTime _start_time = DateTime.MaxValue;
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
        private string gid;
        public List<SessionData> Sessions;
        private DateTime expire;

        public CachedSessionsList(string _id, List<SessionData> s)
        {
            gid = _id.ToString();
            Sessions = s;
            expire = DateTime.Now.AddMinutes(1);
        }

        public string Game_ID { get { return gid; } }

        public DateTime Expire_Time {  get { return expire; } }

        public void Reset()
        {
            expire = DateTime.Now.AddMinutes(1);
        }
    }

    #endregion
}
