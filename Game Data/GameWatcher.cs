using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace Game_Data
{
    public delegate void gameStartedD(SupportedGame game, DateTime time);
    public delegate void gameClosedD(SupportedGame game);

    public class GameWatcher
    {
        public static event gameStartedD gameStarted;
        public static event gameClosedD gameClosed;
        public static List<SupportedGame> supportedGames = new List<SupportedGame>();
        ManagementEventWatcher processWatcher;

        public GameWatcher()
        {
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(Settings.Save_Path + "\\games.ini");
            foreach (SectionData section in ini.Sections)
            {
                if (section.SectionName != "General")
                {
                    supportedGames.Add(new SupportedGame(section.SectionName, section.Keys["Game_Name"], section.Keys["Process_Name"]));
                }
            }
            //
            processWatcher = new ManagementEventWatcher(@"SELECT * FROM __InstanceOperationEvent WITHIN 1.5 WHERE TargetInstance ISA 'Win32_Process'");
            processWatcher.EventArrived += processWatcher_EventArrived;
        }

        #region Edit Loaded Supported Games

        public static void AddSupportedGame(SupportedGame nGame)
        {
            supportedGames.Add(nGame);
            Process[] procs = Process.GetProcessesByName(nGame.Process_Name);
            if (procs.Length > 0)
            {
                gameStarted(nGame, procs[0].StartTime);
            }
        }

        public static void RemoveSupportedGame(SupportedGame nGame)
        {
            supportedGames.Remove(nGame);
            Process[] procs = Process.GetProcessesByName(nGame.Process_Name);
            if (procs.Length > 0)
            {
                gameClosed(nGame);
            }
        }

        public static void EditSupportedGame(SupportedGame oGame, SupportedGame nGame)
        {
            supportedGames.Remove(oGame);
            supportedGames.Add(nGame);
            if (oGame.Process_Name != nGame.Process_Name)
            {
                Process[] procs = Process.GetProcessesByName(oGame.Process_Name);
                if (procs.Length > 0)
                {
                    gameClosed(oGame);
                }
                procs = Process.GetProcessesByName(nGame.Process_Name);
                if (procs.Length > 0)
                {
                    gameStarted(nGame, procs[0].StartTime);
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
                    gameStarted(sGame, proc.StartTime);
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
                        {
                            gameStarted(game, DateTime.Now);
                            break;
                        }
                    case "__InstanceDeletionEvent":
                        {
                            gameClosed(game);
                            break;
                        }
                }
            }
            obj.Dispose();
            mbo.Dispose();
        }
    }
}
