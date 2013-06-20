using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Game_Data
{
    public static class GameDataCollector
    {
        private static Thread collectorThread = new Thread(new ThreadStart(dataCollectorThread));
        //
        private static List<RunningSession> runningSessions = new List<RunningSession>();

        private static int col_res = -1;

        public static int Collection_Res
        {
            get { return col_res; }
        }

        #region Imports

        [DllImport("Shell32")]
        public static extern int ExtractIconEx(
            string sFile,
            int iIndex,
            out IntPtr piLargeVersion,
            out IntPtr piSmallVersion,
            int amountIcons);

        #endregion

        public static void Collection_Change()
        {
            if (Settings.Collection_Resolution > 0)
            {
                col_res = Settings.Collection_Resolution * 1000;
                if (runningSessions.Count > 0 && !collectorThread.IsAlive)
                {
                    foreach (RunningSession session in runningSessions)
                    {
                        session.CPU_Usage_Timer.Restart();
                    }
                    collectorThread = new Thread(new ThreadStart(dataCollectorThread));
                    collectorThread.Start();
                }
            }
            else
            {
                if (collectorThread != null) { collectorThread.Abort(); col_res = 0; }
            }
        }

        public static bool isCurrentlyTracking(string process_name)
        {
            RunningSession session = runningSessions.Find(delegate(RunningSession x) { return x.Proc.ProcessName.ToLower() == process_name.ToLower(); });
            return (session != null) ? true : false;
        }

        public static void watchGame(string game_name, string process_name)
        {
            if (col_res == -1) { col_res = Settings.Collection_Resolution; }
            Process[] proc = Process.GetProcessesByName(process_name);
            if (proc.Length > 1) { System.Windows.Forms.MessageBox.Show("Data collection currently doesn't support multi-process games. This will be addressed in the future if needed."); }
            RunningSession temp = new RunningSession(proc[0]);
            temp.Data.Start_Time = proc[0].StartTime;
            runningSessions.Add(temp);
            if (col_res > 0)
            {
                if (!collectorThread.IsAlive) { collectorThread = new Thread(new ThreadStart(dataCollectorThread)); collectorThread.Start(); }
            }
            //
            // Unfinished icon grabbing
            /* string s = GameDatabase.getGameDataPath(game_name) + process_name + ".ico";
            if (!File.Exists(s))
            {
                IntPtr large;
                IntPtr small;
                ExtractIconEx(proc[0].MainModule.FileName, 0, out large, out small, 1);
                Icon ico = Icon.FromHandle(small);
                //
                //Icon ico = Icon.ExtractAssociatedIcon(proc[0].MainModule.FileName);
                using (FileStream fs = new FileStream(s, FileMode.Create))
                    ico.Save(fs);
            }*/
        }

        public static RunningSession stopWatchingGame(string process_name)
        {
            RunningSession session = runningSessions.Find(delegate(RunningSession x) { return x.Proc.ProcessName.ToLower() == process_name.ToLower(); });
            if (session != null)
            {
                session.Data.End_Time = DateTime.Now;
                //
                runningSessions.Remove(session);
                if (collectorThread.IsAlive)
                {
                    if (runningSessions.Count == 0) { collectorThread.Abort(); }
                    //
                    if (session.CollectionData.Count > 0)
                    {
                        long total_working_set = 0;
                        double total_cpu_usage = 0;
                        foreach (CollectionData data in session.CollectionData)
                        {
                            total_working_set += data.Working_Set;
                            total_cpu_usage += data.CPU_Usage;
                        }
                        session.Data.Average_CPU_Usage = total_cpu_usage / (session.CollectionData.Count - 1);
                        session.Data.Average_Working_Set = total_working_set / session.CollectionData.Count;
                    }
                }
                //
                return session;
            }
            return null;
        }

        private static void dataCollectorThread()
        {
            while (true)
            {
                foreach (RunningSession session in runningSessions)
                {
                    if (!session.Proc.HasExited)
                    {
                        session.Proc.Refresh();
                        //
                        CollectionData cData = new CollectionData();
                        if (session.CollectionData.Count > 0)
                        {
                            TimeSpan pCalc = session.Proc.TotalProcessorTime - session.CollectionData[session.CollectionData.Count - 1].Last_CPU_Time;
                            cData.CPU_Usage = Math.Round(pCalc.TotalMilliseconds / session.CPU_Usage_Timer.ElapsedMilliseconds, 3);
                        }
                        cData.Working_Set = session.Proc.WorkingSet64 / 1024;
                        cData.Last_CPU_Time = session.Proc.TotalProcessorTime;
                        session.CPU_Usage_Timer.Restart();
                        session.CollectionData.Add(cData);
                    }
                }
                Thread.Sleep(col_res);
            }
        }
    }

    #region RunningSession

    public class RunningSession
    {
        Process _proc;
        Stopwatch _cpu_usage_timer;
        SessionData _data;
        List<CollectionData> _collectionData;

        public RunningSession(Process proc)
        {
            _proc = proc;
            _cpu_usage_timer = new Stopwatch();
            _data = new SessionData();
            _collectionData = new List<CollectionData>();
        }

        public Process Proc
        {
            get { return _proc; }
            set { _proc = value; }
        }

        public Stopwatch CPU_Usage_Timer
        {
            get { return _cpu_usage_timer; }
            set { _cpu_usage_timer = value; }
        }

        public SessionData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public List<CollectionData> CollectionData
        {
            get { return _collectionData; }
            set { _collectionData = value; }
        }
    }

    #endregion

    #region CollectionData

    public class CollectionData
    {
        double _cpu_usage = 0;
        long _working_set = 0;
        TimeSpan _last_cpu_time = new TimeSpan();

        public double CPU_Usage
        {
            get { return _cpu_usage; }
            set { _cpu_usage = value; }
        }

        public long Working_Set
        {
            get { return _working_set; }
            set { _working_set = value; }
        }

        public TimeSpan Last_CPU_Time
        {
            get { return _last_cpu_time; }
            set { _last_cpu_time = value; }
        }
    }

    #endregion
}
