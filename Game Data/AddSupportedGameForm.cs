using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace Game_Data
{
    public delegate void addGameHandler(SupportedGame nGame, SupportedGame oGame);

    public partial class AddSupportedGameForm : Form
    {
        private static bool open = false;
        private string _id;
        private SupportedGame sItem;
        public event addGameHandler addGame;
        List<string> blacklist = new List<string>();

        public AddSupportedGameForm(int id)
        {
            open = true;
            //
            InitializeComponent();
            //
            _id = id.ToString();
            if (!String.IsNullOrEmpty(Settings.AddSupportedGame_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.AddSupportedGame_Window_Geometry, this); }
        }

        public AddSupportedGameForm(SupportedGame s)
        {
            open = true;
            //
            InitializeComponent();
            //
            sItem = s;
            if (!String.IsNullOrEmpty(Settings.AddSupportedGame_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.AddSupportedGame_Window_Geometry, this); }
            this.Text = "Edit Supported Game";
            this.addButton.Text = "Save";
            this.textBox2.Text = s.Game_Name;
            this.comboBox1.Text = s.Process_Name;
        }

        public static bool isOpen
        {
            get { return open; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(comboBox1.Text)) { return; }
            SupportedGame nGame;
            if (sItem != null)
            {
                nGame = new SupportedGame(sItem.ID, textBox2.Text.Trim(), comboBox1.Text.Trim().ToLower());
                if (sItem.Game_Name == nGame.Game_Name && sItem.Process_Name == nGame.Process_Name) { this.Close(); }
            }
            else { nGame = new SupportedGame(_id, textBox2.Text.Trim(), comboBox1.Text.Trim().ToLower()); }
            //
            foreach (SupportedGame game in GameWatcher.supportedGames)
            {
                if (game.ID != nGame.ID)
                {
                    if (game.Game_Name == nGame.Game_Name)
                    {
                        MessageBox.Show("A game exists with the name " + '"' + game.Game_Name + '"' + " already.");
                        return;
                    }
                    if (game.Process_Name == nGame.Process_Name)
                    {
                        MessageBox.Show("A game exists with the game process " + '"' + game.Process_Name + '"' + " already.");
                        return;
                    }
                }

            }
            //
            addGame(nGame, sItem);
            this.Close();
        }

        private void AddSupportedGameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter) { button1_Click(null, null); }
        }

        private void AddSupportedGameForm_Load(object sender, EventArgs e)
        {
            if (Settings.Hide_Common_Processes)
            {
                if (!File.Exists(Settings.Save_Path + "\\blacklist.txt"))
                {
                    takeSnapshot();
                }
                string contents = File.ReadAllText(Settings.Save_Path + "\\blacklist.txt");
                string[] items =  contents.Split('|');
                foreach (string item in items) { blacklist.Add(item); }
            }
        }

        private void AddSupportedGameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            Process[] running_procs = Process.GetProcesses();
            string[] procs_names = new string[running_procs.Length];
            int i = 0;
            foreach (Process proc in running_procs)
            {
                procs_names[i++] = proc.ProcessName;
            }
            Array.Sort(procs_names);
            foreach (string proc in procs_names)
            {
                if (GameWatcher.supportedGames.Find(delegate(SupportedGame p) { return p.Process_Name == proc.ToLower(); }) == null)
                {
                    if (checkBox1.Checked)
                    {
                        if (blacklist.Find(delegate (string p) { return p == proc.ToLower(); }) == null)
                        {
                            if (comboBox1.FindString(proc.ToLower()) == -1) { comboBox1.Items.Add(proc); }
                        }
                    }
                    else { if (comboBox1.FindString(proc.ToLower()) == -1) { comboBox1.Items.Add(proc); } }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox1.Text.ToUpper();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Hide_Common_Processes = checkBox1.Checked;
            //
            if (checkBox1.Checked)
            {
                takeSnapshot();
            }
        }

        private void takeSnapshot()
        {
            string contents = "";
            Process[] running_procs = Process.GetProcesses();
            blacklist.Clear();
            foreach (Process proc in running_procs)
            {
                if (contents == "") { contents += proc.ProcessName.ToLower(); }
                else { contents += "|" + proc.ProcessName.ToLower(); }
                blacklist.Add(proc.ProcessName.ToLower());
            }
            File.WriteAllText(Settings.Save_Path + "\\blacklist.txt", contents);
        }

        private void AddSupportedGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.AddSupportedGame_Window_Geometry = WindowGeometry.GeometryToString(this);
        }
    }
}
