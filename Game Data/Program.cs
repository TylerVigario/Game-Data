using System;
using System.Threading;
using System.Windows.Forms;
using CrashReporterDotNET;

namespace Game_Data
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += ApplicationThreadException;
            //
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var reportCrash = new ReportCrash
            {
                FromEmail = "TylerVigario90@gmail.com",
                ToEmail = "logicpwn.crashes@gmail.com",
                SMTPHost = "smtp.gmail.com",
                Port = 587,
                UserName = "logicpwn.crashes@gmail.com",
                Password = "Px4XbJrPkssN",
                EnableSSL = true
            };
            reportCrash.Send(e.Exception);
        }
    }
}
