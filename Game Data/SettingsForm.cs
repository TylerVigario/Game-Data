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
            checkBox1.Checked = Util.IsAutoStartEnabled("Game_Data", Application.ExecutablePath);
            checkBox2.Checked = Settings.Start_Hidden;
            checkBox3.Checked = Settings.Exit_Confrimation;
            checkBox4.Checked = Settings.Close_To_Tray;
            checkBox6.Checked = Settings.Update_Check_On_Startup;
            numericUpDown1.Value = (decimal)Settings.Session_Threshold;
            numericUpDown2.Value = (decimal)Settings.Collection_Resolution;
            numericUpDown3.Value = (decimal)Settings.Update_Check_Interval;
            //
            switch (Settings.Time_Display_Level)
            {
                case 3:
                    radioButton1.Checked = true;
                    break;
                case 2:
                    radioButton2.Checked = true;
                    break;
                case 1:
                    radioButton3.Checked = true;
                    break;
                case 0:
                    radioButton4.Checked = true;
                    break;
            }
            //
            switch (Settings.Last_Played_Display_Level)
            {
                case 2:
                    radioButton8.Checked = true;
                    break;
                case 1:
                    radioButton7.Checked = true;
                    break;
                case 0:
                    radioButton6.Checked = true;
                    break;
            }
            //
            this.checkBox1.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Session_Threshold = (int)numericUpDown1.Value;
            Settings.Collection_Resolution = (int)numericUpDown2.Value;
            Settings.Update_Check_Interval = (int)numericUpDown3.Value;
            Settings.Start_Hidden = checkBox2.Checked;
            Settings.Exit_Confrimation = checkBox3.Checked;
            Settings.Close_To_Tray = checkBox4.Checked;
            Settings.Update_Check_On_Startup = checkBox6.Checked;
            //
            if (radioButton1.Checked) { Settings.Time_Display_Level = 3; }
            else if (radioButton2.Checked) { Settings.Time_Display_Level = 2; }
            else if (radioButton3.Checked) { Settings.Time_Display_Level = 1; }
            else if (radioButton4.Checked) { Settings.Time_Display_Level = 0; }
            //
            if (radioButton8.Checked) { Settings.Last_Played_Display_Level = 2; }
            else if (radioButton7.Checked) { Settings.Last_Played_Display_Level = 1; }
            else if (radioButton6.Checked) { Settings.Last_Played_Display_Level = 0; }
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
