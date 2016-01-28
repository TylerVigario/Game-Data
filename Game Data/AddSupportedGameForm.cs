using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace Game_Data
{
    public delegate void addGameHandler(SupportedGame nGame, SupportedGame oGame);

    public partial class AddSupportedGameForm : Form
    {
        private static bool open = false;
        private string _id;
        private SupportedGame sItem;
        public event addGameHandler addGame;
        List<string> supportedProcs = new List<string>();

        public AddSupportedGameForm(int id)
        {
            InitializeComponent();
            _id = id.ToString();
        }

        public AddSupportedGameForm(SupportedGame s)
        {
            open = true;
            //
            InitializeComponent();
            //
            sItem = s;
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
            addGame(nGame, sItem);
            this.Close();
        }

        private void AddSupportedGameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter) { button1_Click(null, null); }
        }

        private void AddSupportedGameForm_Load(object sender, EventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData ini = parser.ReadFile(Application.StartupPath + "\\games.ini");
            foreach (SectionData section in ini.Sections)
            {
                supportedProcs.Add(section.Keys["Process_Name"]);
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
                if (supportedProcs.Find(delegate(string p) { return p == proc.ToLower(); }) == null)
                {
                    if (comboBox1.FindString(proc.ToLower()) == -1) { comboBox1.Items.Add(proc); }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox1.Text;
        }
    }
}
