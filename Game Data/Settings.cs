using System;
using System.IO;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace Game_Data
{
    class Settings
    {
        private static IniData ini;
        private static Timer flush;

        public static void load()
        {
            if (!File.Exists(Application.StartupPath + "\\settings.ini"))
            {
                File.Create(Application.StartupPath + "\\settings.ini");
            }
            //
            var parser = new FileIniDataParser();
            ini = parser.ReadFile(Application.StartupPath + "\\settings.ini");
            //
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
            Session_Threshold = 60;
            Exit_Confrimation = true;
            Close_To_Tray = true;
        }

        public static string Save_Path
        {
            get { return Application.StartupPath; }
        }

        public static bool Start_Hidden
        {
            get
            {
                int sh = -1;
                string temp = ini["Settings"]["Start_Hidden"];
                if (!String.IsNullOrEmpty(temp)) { sh = int.Parse(temp); }
                if (sh < 0) { sh = 0; }
                return Convert.ToBoolean(sh);
            }
            set
            {
                ini["Settings"]["Start_Hidden"] = Convert.ToInt32(value).ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static bool Close_To_Tray
        {
            get
            {
                int ctt = -1;
                string temp = ini["Settings"]["Close_To_Tray"];
                if (!String.IsNullOrEmpty(temp)) { ctt = int.Parse(temp); }
                if (ctt < 0) { ctt = 1; }
                return Convert.ToBoolean(ctt);
            }
            set
            {
                ini["Settings"]["Close_To_Tray"] = Convert.ToInt32(value).ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static int Session_Threshold
        {
            get
            {
                int st = -1;
                string temp = ini["Settings"]["Session_Threshold"];
                if (!String.IsNullOrEmpty(temp)) { st = int.Parse(temp); }
                if (st < 0) { st = 60; }
                return st;
            }
            set
            {
                ini["Settings"]["Session_Threshold"] = value.ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static bool Exit_Confrimation
        {
            get
            {
                int ec = -1;
                string temp = ini["Settings"]["Exit_Confrimation"];
                if (!String.IsNullOrEmpty(temp)) { ec = int.Parse(temp); }
                if (ec < 0) { ec = 1; }
                return Convert.ToBoolean(ec);
            }
            set
            {
                ini["Settings"]["Exit_Confrimation"] = Convert.ToInt32(value).ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string Main_Window_Geometry
        {
            get
            {
                string temp = ini["Settings"]["Main_Window_Geometry"];
                if (String.IsNullOrEmpty(temp)) { temp = ""; }
                return temp;
            }
            set
            {
                ini["Settings"]["Main_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string GameStats_Window_Geometry
        {
            get
            {
                string temp = ini["Settings"]["GameStats_Window_Geometry"];
                if (String.IsNullOrEmpty(temp)) { temp = ""; }
                return temp;
            }
            set
            {
                ini["Settings"]["GameStats_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string SessionManager_Window_Geometry
        {
            get
            {
                string temp = ini["Settings"]["SessionManager_Window_Geometry"];
                if (String.IsNullOrEmpty(temp)) { temp = ""; }
                return temp;
            }
            set
            {
                ini["Settings"]["SessionManager_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string SupportedGames_Window_Geometry
        {
            get
            {
                string temp = ini["Settings"]["SupportedGames_Window_Geometry"];
                if (String.IsNullOrEmpty(temp)) { temp = ""; }
                return temp;
            }
            set
            {
                ini["Settings"]["SupportedGames_Window_Geometry"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static string GamesList_State
        {
            get
            {
                string temp = ini["Settings"]["GamesList_State"];
                if (String.IsNullOrEmpty(temp)) { temp = ""; }
                return temp;
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
                string temp = ini["Settings"]["SessionsList_State"];
                if (String.IsNullOrEmpty(temp)) { temp = ""; }
                return temp;
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
                string temp = ini["Settings"]["SupportedGamesList_State"];
                if (String.IsNullOrEmpty(temp)) { temp = ""; }
                return temp;
            }
            set
            {
                ini["Settings"]["SupportedGamesList_State"] = value;
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }

        public static int Time_Display_Level
        {
            get
            {
                int tdl = -1;
                string temp = ini["Settings"]["Time_Display_Level"];
                if (!String.IsNullOrEmpty(temp)) { tdl = int.Parse(temp); }
                if (tdl < 0) { tdl = 3; }
                return tdl;
            }
            set
            {
                ini["Settings"]["Time_Display_Level"] = value.ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }


        public static int Last_Played_Display_Level
        {
            get
            {
                int lpdl = -1;
                string temp = ini["Settings"]["Last_Played_Display_Level"];
                if (!String.IsNullOrEmpty(temp)) { lpdl = int.Parse(temp); }
                if (lpdl < 0) { lpdl = 2; }
                return lpdl;
            }
            set
            {
                ini["Settings"]["Last_Played_Display_Level"] = value.ToString();
                flush.Enabled = true;
                flush.Interval = 30000;
            }
        }
    }
}
