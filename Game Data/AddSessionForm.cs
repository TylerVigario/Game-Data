using System;
using System.Windows.Forms;

namespace Game_Data
{
    public delegate void addSessionD(SessionData session);

    public partial class AddSessionForm : Form
    {
        private static bool open = false;
        //
        public event addSessionD sessionAdded;

        public AddSessionForm()
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
            if (numericUpDown1.Value > 0)
            {
                SessionData newSession = new SessionData();
                newSession.Start_Time = dateTimePicker1.Value.Date.Add(dateTimePicker2.Value.TimeOfDay);
                newSession.End_Time = newSession.Start_Time.AddMinutes(Convert.ToDouble(numericUpDown1.Value));
                //
                if (sessionAdded != null) { sessionAdded(newSession); }
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
