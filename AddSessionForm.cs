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
            //
            if (!String.IsNullOrEmpty(Settings.AddSession_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.AddSession_Window_Geometry, this); }
        }

        public AddSessionForm(SessionData s)
        {
            open = true;
            _session = s;
            //
            InitializeComponent();
            //
            if (!String.IsNullOrEmpty(Settings.AddSession_Window_Geometry)) { WindowGeometry.GeometryFromString(Settings.AddSession_Window_Geometry, this); }
            this.Text = "Edit Session";
            button1.Text = "Save";
            dateTimePicker1.Value = s.Start_Time;
            dateTimePicker2.Value = s.Start_Time;
            TimeSpan temp = TimeSpan.FromTicks(s.End_Time.Ticks - s.Start_Time.Ticks);
            numericUpDown3.Value = Convert.ToDecimal(temp.Hours);
            numericUpDown1.Value = Convert.ToDecimal(temp.Minutes);
            numericUpDown2.Value = Convert.ToDecimal(temp.Seconds);
        }

        public static bool isOpen
        {
            get { return open; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SessionData newSession = new SessionData();
            newSession.Start_Time = dateTimePicker1.Value.Date.Add(dateTimePicker2.Value.TimeOfDay);
            newSession.End_Time = newSession.Start_Time.AddHours(Convert.ToDouble(numericUpDown3.Value)).AddMinutes(Convert.ToDouble(numericUpDown1.Value)).AddSeconds(Convert.ToDouble(numericUpDown2.Value));
            if (newSession.Time_Span.TotalSeconds >= Settings.Session_Threshold)
            {
                sessionAdded(newSession, _session);
                //
                this.Close();
            }
            else
            {
                MessageBox.Show("Session is shorter than threshold. Increase time or decrease threshold.");
            }
        }

        private void AddSessionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            open = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value > 0 || numericUpDown1.Value > 0 || numericUpDown2.Value > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void AddSessionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.AddSession_Window_Geometry = WindowGeometry.GeometryToString(this);
        }
    }
}
