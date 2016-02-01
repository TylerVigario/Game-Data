using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Game_Data
{
    public partial class SettingsForm : Form
    {
        private static bool open = false;

        public SettingsForm()
        {
            open = true;
            //
            InitializeComponent();
        }

        public static bool isOpen
        {
            get { return open; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Settings.Settings_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.Settings_Window_Geometry, this); }
            checkBox1.Checked = Util.IsAutoStartEnabled("Game_Data", Application.ExecutablePath);
            checkBox2.Checked = Settings.Start_Hidden;
            checkBox3.Checked = Settings.Exit_Confirmation;
            checkBox4.Checked = Settings.Minimize_To_Tray;
            TimeSpan temp = TimeSpan.FromSeconds(Settings.Session_Threshold);
            numericUpDown1.Value = temp.Seconds;
            numericUpDown2.Value = temp.Minutes;
            this.checkBox1.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Settings_Window_Geometry = WindowGeometry.GeometryToString(this);
            TimeSpan temp = TimeSpan.FromMinutes((double)numericUpDown2.Value).Add(TimeSpan.FromSeconds((double)numericUpDown1.Value));
            Settings.Session_Threshold = temp.TotalSeconds;
            Settings.Start_Hidden = checkBox2.Checked;
            Settings.Exit_Confirmation = checkBox3.Checked;
            Settings.Minimize_To_Tray = checkBox4.Checked;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { Util.SetAutoStart("Game_Data", Application.ExecutablePath); }
            else { Util.UnSetAutoStart("Game_Data"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.reset();
            //
            this.checkBox1.CheckStateChanged -= new System.EventHandler(this.checkBox1_CheckStateChanged);
            //
            SettingsForm_Load(null, null);
        }
    }

    #region Util (Registry/Start with Windows)

    public class Util
    {
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Sets the autostart value for the assembly.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public static void SetAutoStart(string keyName, string assemblyLocation)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.SetValue(keyName, assemblyLocation);
        }

        /// <summary>
        /// Returns whether auto start is enabled.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public static bool IsAutoStartEnabled(string keyName, string assemblyLocation)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
            if (key == null)
                return false;

            string value = (string)key.GetValue(keyName);
            if (value == null)
                return false;

            return (value == assemblyLocation);
        }

        /// <summary>
        /// Unsets the autostart value for the assembly.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        public static void UnSetAutoStart(string keyName)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.DeleteValue(keyName);
        }
    }

    #endregion
}
