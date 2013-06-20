using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace Game_Data
{
    public delegate void gameChangedHandler(string game_name, string process_name);

    public class GameWatcher
    {
        public static event gameChangedHandler gameStarted;
        public static event gameChangedHandler gameClosed;
        //
        private static List<SupportedGame> supportedGames = new List<SupportedGame>();
        //
        ManagementEventWatcher processWatcher;

        public GameWatcher()
        {
            IniFile ini = new IniFile(Settings.Save_Path + "\\games.ini");
            string[] sections = ini.GetSectionNames();
            foreach (string section in sections)
            {
                supportedGames.Add(new SupportedGame(ini.GetString(section, "Game_Name", ""), ini.GetString(section, "Process_Name", "")));
            }
            //
            processWatcher = new ManagementEventWatcher(@"SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");
            processWatcher.EventArrived += processWatcher_EventArrived;
        }

        #region Edit Loaded Supported Games

        public static void AddSupportedGame(SupportedGame nGame)
        {
            supportedGames.Add(nGame);
            Process[] procs = Process.GetProcessesByName(nGame.Process_Name);
            if (procs.Length > 0)
            {
                if (gameStarted != null) { gameStarted(nGame.Game_Name, procs[0].ProcessName); }
            }
        }

        public static void RemoveSupportedGame(SupportedGame nGame)
        {
            supportedGames.Remove(nGame);
            Process[] procs = Process.GetProcessesByName(nGame.Process_Name);
            if (procs.Length > 0)
            {
                if (gameClosed != null) { gameClosed(nGame.Game_Name, nGame.Process_Name); }
            }
        }

        public static void EditSupportedGame(SupportedGame oGame, SupportedGame nGame)
        {
            SupportedGame sGame = supportedGames.Find(delegate(SupportedGame x) { return x.Process_Name.ToLower() == oGame.Process_Name; });
            sGame = nGame;
            if (oGame.Process_Name != nGame.Process_Name)
            {
                Process[] procs = Process.GetProcessesByName(nGame.Process_Name);
                if (procs.Length > 0)
                {
                    if (gameStarted != null) { gameStarted(nGame.Game_Name, procs[0].ProcessName); }
                }
            }
        }

        #endregion

        public void Start()
        {
            foreach (Process proc in Process.GetProcesses())
            {
                SupportedGame sGame = supportedGames.Find(delegate(SupportedGame x) { return x.Process_Name.ToLower() == proc.ProcessName.ToLower(); });
                if (sGame != null)
                {
                    if (gameStarted != null) { gameStarted(sGame.Game_Name, proc.ProcessName); }
                }
            }
            //
            processWatcher.Start();
        }

        public void Stop()
        {
            processWatcher.Stop();
        }

        public void Dispose()
        {
            processWatcher.Dispose();
        }

        void processWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject mbo, obj;
            mbo = e.NewEvent as ManagementBaseObject;
            obj = mbo["TargetInstance"] as ManagementBaseObject;
            string process_name = Path.GetFileNameWithoutExtension(obj["Caption"].ToString());
            SupportedGame game = supportedGames.Find(delegate(SupportedGame x) { return x.Process_Name.ToLower() == process_name.ToLower(); });
            if (game != null)
            {
                switch (mbo.ClassPath.ClassName)
                {
                    case "__InstanceCreationEvent":
                        if (gameStarted != null) { gameStarted(game.Game_Name, process_name); } break;
                    case "__InstanceDeletionEvent":
                        if (gameClosed != null) { gameClosed(game.Game_Name, process_name); } break;
                }
            }
            obj.Dispose();
            mbo.Dispose();
            game = null;
            process_name = null;
            System.GC.Collect();
        }
    }

    #region Supported Game

    public class SupportedGame
    {
        private string _game_name;
        private string _process_name;

        public SupportedGame(string g_n, string p_n)
        {
            _game_name = g_n;
            _process_name = p_n;
        }

        public string Game_Name { get { return _game_name; } }

        public string Process_Name { get { return _process_name; } }
    }

    #endregion
}
