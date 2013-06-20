using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Game_Data
{
    public partial class CounterForm : Form
    {
        string _counterText = "";
        TimeSpan _counter;
        DateTime _date_started;
        bool _counter_started = false;
        string _mode = "";

        public CounterForm()
        {
            InitializeComponent();
        }

        private void CounterForm_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            modeComboBox.SelectedIndex = 0;
        }

        #region Counter Resize

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_counter_started)
            {
                using (Font f = new Font("Tahoma", 15))
                {
                    SizeF size;
                    using (Font f2 = AppropriateFont(e.Graphics, ClientRectangle.Size, _counterText, f, out size))
                    {
                        PointF p = new PointF((ClientRectangle.Width - size.Width) / 2, (ClientRectangle.Height - size.Height) / 2);
                        e.Graphics.DrawString(_counterText, f2, Brushes.Black, p);
                    }
                }
            }
            //
            base.OnPaint(e);
        }

        public static Font AppropriateFont(Graphics g, Size layoutSize, string s, Font f, out SizeF extent)
        {
            extent = g.MeasureString(s, f);
            //
            float hRatio = layoutSize.Height / extent.Height;
            float wRatio = layoutSize.Width / extent.Width;
            float ratio = (hRatio < wRatio) ? hRatio : wRatio;
            //
            float newSize = f.Size * ratio;
            //
            f = new Font(f.FontFamily, newSize, f.Style);
            extent = g.MeasureString(s, f);
            //
            return f;
        }

        #endregion

        private void startButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(modeComboBox.Text)) { return; }
            _mode = modeComboBox.Text;
            //
            if (_mode == "Countdown")
            {
                this.Text = "Countdown";
                try
                {
                    _counter = new TimeSpan(Convert.ToInt32(hoursBox.Text), Convert.ToInt32(minutesBox.Text), Convert.ToInt32(secondsBox.Text));
                }
                catch (FormatException) { return; }
                if (_counter.TotalSeconds < 1) { return; }
                _counterText = _counter.ToString(@"hh\:mm\:ss");
            }
            else if (_mode == "Elapsed Time")
            {
                this.Text = "Elapsed Time";
                _counterText = "00:00:00";
            }
            //
            _date_started = DateTime.Now;
            //
            modeComboBox.Visible = false;
            divider1.Visible = false;
            divider2.Visible = false;
            hoursBox.Visible = false;
            minutesBox.Visible = false;
            secondsBox.Visible = false;
            startButton.Visible = false;
            //
            _counter_started = true;
            this.Refresh();
            //
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_mode == "Countdown")
            {
                TimeSpan remainingTime = _counter - (DateTime.Now - _date_started);
                if (remainingTime.TotalSeconds > 0)
                {
                    _counterText = remainingTime.ToString(@"hh\:mm\:ss");
                    this.Refresh();
                    if (TaskbarManager.IsPlatformSupported) { TaskbarManager.Instance.SetProgressValue((int)(DateTime.Now - _date_started).TotalSeconds, (int)_counter.TotalSeconds); }
                }
                else
                {
                    timer.Enabled = false;
                    _counterText = "Times Up";
                    this.Refresh();
                    this.TopLevel = true;
                    this.BringToFront();
                    this.Focus();
                }
            }
            else if (_mode == "Elapsed Time")
            {
                _counterText = (DateTime.Now - _date_started).ToString(@"hh\:mm\:ss");
                this.Refresh();
            }
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (alwaysOnTopToolStripMenuItem.Checked)
            {
                this.TopMost = true;
            }
            else { this.TopMost = false; }
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modeComboBox.Text == "Countdown")
            {
                divider1.Enabled = true;
                divider2.Enabled = true;
                hoursBox.Enabled = true;
                minutesBox.Enabled = true;
                secondsBox.Enabled = true;
            }
            else if (modeComboBox.Text == "Elapsed Time")
            {
                divider1.Enabled = false;
                divider2.Enabled = false;
                hoursBox.Enabled = false;
                minutesBox.Enabled = false;
                secondsBox.Enabled = false;
            }
        }
    }
}
