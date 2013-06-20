using System;
using System.IO;
using System.Windows.Forms;

namespace Game_Data
{
    class Settings
    {
        private static IniFile ini;

        public static void load()
        {
            string path = Application.StartupPath + "\\settings.ini";
            if (!File.Exists(path)) { File.Create(path); }
            //
            ini = new IniFile(path);
        }

        public static void reset()
        {
            Start_Hidden = false;
            Session_Threshold = 60;
            Collection_Resolution = 30;
            Exit_Confrimation = true;
            Update_Check_Interval = 60;
            Close_To_Tray = true;
        }

        #region Save_Path

        public static string Save_Path
        {
            get { return Application.StartupPath; }
        }

        #endregion

        #region Start_Hidden

        private static int sh = -1;

        public static bool Start_Hidden
        {
            get
            {
                if (sh < 0)
                {
                    sh = ini.GetInt32("Settings", "Start_Hidden", -1);
                    if (sh < 0) { sh = 0; }
                }
                return Convert.ToBoolean(sh);
            }
            set
            {
                int t_sh = Convert.ToInt32(value);
                if (t_sh != sh)
                {
                    sh = t_sh;
                    ini.WriteValue("Settings", "Start_Hidden", t_sh);
                }
            }
        }

        #endregion

        #region Close_To_Tray

        private static int ctt = -1;

        public static bool Close_To_Tray
        {
            get
            {
                if (ctt < 0)
                {
                    sh = ini.GetInt32("Settings", "Close_To_Tray", -1);
                    if (ctt < 0) { ctt = 1; }
                }
                return Convert.ToBoolean(ctt);
            }
            set
            {
                int t_ctt = Convert.ToInt32(value);
                if (t_ctt != ctt)
                {
                    ctt = t_ctt;
                    ini.WriteValue("Settings", "Close_To_Tray", t_ctt);
                }
            }
        }

        #endregion

        #region Session_Threshold

        private static int st = -1;

        public static int Session_Threshold
        {
            get
            {
                if (st < 0)
                {
                    st = ini.GetInt32("Settings", "Session_Threshold", -1);
                    if (st < 0) { st = 60; }
                }
                return st;
            }
            set
            {
                if (value != st)
                {
                    st = value;
                    ini.WriteValue("Settings", "Session_Threshold", value);
                }
            }
        }

        #endregion

        #region Collection_Resolution

        private static int cr = -1;

        public static int Collection_Resolution
        {
            get
            {
                if (cr < 0)
                {
                    cr = ini.GetInt32("Settings", "Collection_Resolution", -1);
                    if (cr < 0) { cr = 30; }
                }
                return cr;
            }
            set
            {
                if (value != cr)
                {
                    cr = value;
                    ini.WriteValue("Settings", "Collection_Resolution", value);
                }
            }
        }

        #endregion

        #region Exit_Confrimation

        private static int ec = -1;

        public static bool Exit_Confrimation
        {
            get
            {
                if (ec < 0)
                {
                    ec = ini.GetInt32("Settings", "Exit_Confrimation", -1);
                    if (ec < 0) { ec = 1; }
                }
                return Convert.ToBoolean(ec);
            }
            set
            {
                int t_ec = Convert.ToInt32(value);
                if (t_ec != ec)
                {
                    ec = t_ec;
                    ini.WriteValue("Settings", "Exit_Confrimation", t_ec);
                }
            }
        }

        #endregion

        #region Update_Check_On_Startup

        private static int upos = -1;

        public static bool Update_Check_On_Startup
        {
            get
            {
                if (upos < 0)
                {
                    upos = ini.GetInt32("Settings", "Update_Check_On_Startup", -1);
                    if (upos < 0) { upos = 1; }
                }
                return Convert.ToBoolean(upos);
            }
            set
            {
                int t_upos = Convert.ToInt32(value);
                if (t_upos != upos)
                {
                    ec = t_upos;
                    ini.WriteValue("Settings", "Update_Check_On_Startup", t_upos);
                }
            }
        }

        #endregion

        #region Main_Window_Geometry

        public static string Main_Window_Geometry
        {
            get
            {
                return ini.GetString("Settings", "Main_Window_Geometry", "");
            }
            set
            {
                ini.WriteValue("Settings", "Main_Window_Geometry", value);
            }
        }

        #endregion

        #region GameStats_Window_Geometry

        private static string gswg = null;

        public static string GameStats_Window_Geometry
        {
            get
            {
                if (gswg == null)
                {
                    gswg = ini.GetString("Settings", "GameStats_Window_Geometry", "");
                }
                return gswg;
            }
            set
            {
                gswg = value;
                ini.WriteValue("Settings", "GameStats_Window_Geometry", value);
            }
        }

        #endregion

        #region SessionManager_Window_Geometry

        private static string smwg = null;

        public static string SessionManager_Window_Geometry
        {
            get
            {
                if (smwg == null)
                {
                    smwg = ini.GetString("Settings", "SessionManager_Window_Geometry", "");
                }
                return smwg;
            }
            set
            {
                smwg = value;
                ini.WriteValue("Settings", "SessionManager_Window_Geometry", value);
            }
        }

        #endregion

        #region SupportedGames_Window_Geometry

        private static string sgwg = null;

        public static string SupportedGames_Window_Geometry
        {
            get
            {
                if (sgwg == null)
                {
                    sgwg = ini.GetString("Settings", "SupportedGames_Window_Geometry", "");
                }
                return sgwg;
            }
            set
            {
                sgwg = value;
                ini.WriteValue("Settings", "SupportedGames_Window_Geometry", value);
            }
        }

        #endregion

        #region GamesList_State

        public static string GamesList_State
        {
            get
            {
                return ini.GetString("Settings", "GamesList_State", "");
            }
            set
            {
                ini.WriteValue("Settings", "GamesList_State", value);
            }
        }

        #endregion

        #region SessionsList_State

        private static string sls = null;

        public static string SessionsList_State
        {
            get
            {
                if (sls == null)
                {
                    sls = ini.GetString("Settings", "SessionsList_State", "");
                }
                return sls;
            }
            set
            {
                sls = value;
                ini.WriteValue("Settings", "SessionsList_State", value);
            }
        }

        #endregion

        #region SupportedGamesList_State

        private static string sgls = null;

        public static string SupportedGamesList_State
        {
            get
            {
                if (sgls == null)
                {
                    sgls = ini.GetString("Settings", "SupportedGamesList_State", "");
                }
                return sgls;
            }
            set
            {
                sgls = value;
                ini.WriteValue("Settings", "SupportedGamesList_State", value);
            }
        }

        #endregion

        #region Update_Check_Interval

        private static int uci = -1;

        public static int Update_Check_Interval
        {
            get
            {
                if (uci < 0)
                {
                    uci = ini.GetInt32("Settings", "Update_Check_Interval", -1);
                    if (uci < 0) { uci = 60; }
                }
                return uci;
            }
            set
            {
                if (value != uci)
                {
                    uci = value;
                    ini.WriteValue("Settings", "Update_Check_Interval", value);
                }
            }
        }

        #endregion

        #region Time_Display_Level

        private static int tdl = -1;

        public static int Time_Display_Level
        {
            get
            {
                if (tdl < 0)
                {
                    tdl = ini.GetInt32("Settings", "Time_Display_Level", -1);
                    if (tdl < 0) { tdl = 3; }
                }
                return tdl;
            }
            set
            {
                if (value != tdl)
                {
                    tdl = value;
                    ini.WriteValue("Settings", "Time_Display_Level", value);
                }
            }
        }

        #endregion

        #region Last_Played_Display_Level

        private static int lpdl = -1;

        public static int Last_Played_Display_Level
        {
            get
            {
                if (lpdl < 0)
                {
                    lpdl = ini.GetInt32("Settings", "Last_Played_Display_Level", -1);
                    if (lpdl < 0) { lpdl = 2; }
                }
                return lpdl;
            }
            set
            {
                if (value != lpdl)
                {
                    lpdl = value;
                    ini.WriteValue("Settings", "Last_Played_Display_Level", value);
                }
            }
        }

        #endregion
    }
}
