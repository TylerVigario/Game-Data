using IniParser;
using IniParser.Model;
using System;
using System.IO;
using System.Windows.Forms;

namespace Game_Data
{
    class Settings
    {
        private static IniData ini;
        private static Timer flush;

        public static void load()
        {
            var parser = new FileIniDataParser();
            if (!File.Exists(Application.StartupPath + "\\settings.ini"))
            {
                ini = new IniData();
                ini.Sections.AddSection("Settings");
                ini["Settings"].AddKey("Version", "1");
                ini["Settings"].AddKey("Start_Hidden", "0");
                ini["Settings"].AddKey("Minimize_To_Tray", "1");
                ini["Settings"].AddKey("Session_Threshold", "180");
                ini["Settings"].AddKey("Exit_Confirmation", "1");
                ini["Settings"].AddKey("Hide_Common_Processes", "1");
                parser.WriteFile(Application.StartupPath + "\\settings.ini", ini);
            }
            else { ini = parser.ReadFile(Application.StartupPath + "\\settings.ini"); }
            //
            if (ini["Settings"]["Version"] != "1") { File.Delete(Application.StartupPath + "\\settings.ini"); load(); return; }
            flush = new Timer();
            flush.Interval = 30000;
            flush.Enabled = false;
            flush.Tick += Flush_Tick;
        }

        public static void Dispose()
        {
            Flush_Tick(null, null);
        }

        private static void Flush_Tick(object sender, EventArgs e)
        {
            var parser = new FileIniDataParser();
            parser.WriteFile(Application.StartupPath + "\\settings.ini", ini);
            flush.Enabled = false;
        }

        public static void reset()
        {
            Start_Hidden = false;
            Minimize_To_Tray = true;
            Session_Threshold = 180;
            Exit_Confirmation = true;
            Hide_Common_Processes = true;
        }

        public static string Save_Path
        {
            get { return Application.StartupPath; }
        }

        public static bool Start_Hidden
        {
            get
            {
                return Convert.ToBoolean(Convert.ToInt32(ini["Settings"]["Start_Hidden"]));
            }
            set
            {
                ini["Settings"]["Start_Hidden"] = Convert.ToInt32(value).ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static bool Minimize_To_Tray
        {
            get
            {
                return Convert.ToBoolean(Convert.ToInt32(ini["Settings"]["Minimize_To_Tray"]));
            }
            set
            {
                ini["Settings"]["Minimize_To_Tray"] = Convert.ToInt32(value).ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static double Session_Threshold
        {
            get
            {
                return double.Parse(ini["Settings"]["Session_Threshold"]);
            }
            set
            {
                ini["Settings"]["Session_Threshold"] = value.ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static bool Exit_Confirmation
        {
            get
            {
                return Convert.ToBoolean(Convert.ToInt32(ini["Settings"]["Exit_Confirmation"]));
            }
            set
            {
                ini["Settings"]["Exit_Confirmation"] = Convert.ToInt32(value).ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static bool Hide_Common_Processes
        {
            get
            {
                return Convert.ToBoolean(Convert.ToInt32(ini["Settings"]["Hide_Common_Processes"]));
            }
            set
            {
                ini["Settings"]["Hide_Common_Processes"] = Convert.ToInt32(value).ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        #region Window Geometry

        public static string Main_Window_Geometry
        {
            get
            {
                return ini["Settings"]["Main_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["Main_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string Settings_Window_Geometry
        {
            get
            {
                return ini["Settings"]["Settings_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["Settings_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string WeeklyAverages_Window_Geometry
        {
            get
            {
                return ini["Settings"]["WeeklyAverages_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["WeeklyAverages_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string SessionManager_Window_Geometry
        {
            get
            {
                return ini["Settings"]["SessionManager_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["SessionManager_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string AddSession_Window_Geometry
        {
            get
            {
                return ini["Settings"]["AddSession_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["AddSession_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string SessionsLength_Window_Geometry
        {
            get
            {
                return ini["Settings"]["SessionsLength_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["SessionsLength_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string SupportedGames_Window_Geometry
        {
            get
            {
                return ini["Settings"]["SupportedGames_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["SupportedGames_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string AddSupportedGame_Window_Geometry
        {
            get
            {
                return ini["Settings"]["AddSupportedGame_Window_Geometry"];
            }
            set
            {
                ini["Settings"]["AddSupportedGame_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        #endregion

        #region List State

        public static string GamesList_State
        {
            get
            {
                return ini["Settings"]["GamesList_State"];
            }
            set
            {
                ini["Settings"]["GamesList_State"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string SessionsList_State
        {
            get
            {
                return ini["Settings"]["SessionsList_State"];
            }
            set
            {
                ini["Settings"]["SessionsList_State"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string SupportedGamesList_State
        {
            get
            {
                return ini["Settings"]["SupportedGamesList_State"];
            }
            set
            {
                ini["Settings"]["SupportedGamesList_State"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        #endregion
    }
}
