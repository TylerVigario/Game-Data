using System;
using System.Windows.Forms;

namespace Game_Data
{
    public delegate void addSessionD(SessionData nSes, SessionData oSes = null);

    public partial class AddSessionForm : Form
    {
        private static bool open = false;
        private SessionData _session;
        public event addSessionD sessionAdded;

        public AddSessionForm()
        {
            open = true;
            //
            InitializeComponent();
        }

        public AddSessionForm(SessionData s)
        {
            open = true;
            _session = s;
            //
            InitializeComponent();
            //
            this.Text = "Edit Session";
            button1.Text = "Save";
            dateTimePicker1.Value = s.Start_Time;
            dateTimePicker2.Value = s.Start_Time;
            numericUpDown1.Value = Convert.ToDecimal(TimeSpan.FromTicks(s.End_Time.Ticks - s.Start_Time.Ticks).TotalMinutes);
        }

        public static bool isOpen
        {
            get { return open; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > 0)
            {
                SessionData newSession = new SessionData();
                newSession.Start_Time = dateTimePicker1.Value.Date.Add(dateTimePicker2.Value.TimeOfDay);
                newSession.End_Time = newSession.Start_Time.AddMinutes(Convert.ToDouble(numericUpDown1.Value));
                sessionAdded(newSession, _session);
                //
                this.Close();
            }
        }

        private void AddSessionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }
    }
}
