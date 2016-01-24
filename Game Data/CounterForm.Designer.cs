namespace Game_Data
{
    partial class CounterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CounterForm));
            this.startButton = new System.Windows.Forms.Button();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.hoursBox = new System.Windows.Forms.TextBox();
            this.minutesBox = new System.Windows.Forms.TextBox();
            this.secondsBox = new System.Windows.Forms.TextBox();
            this.divider1 = new System.Windows.Forms.Label();
            this.divider2 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(180, 76);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(66, 21);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // modeComboBox
            // 
            this.modeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Items.AddRange(new object[] {
            "Countdown",
            "Elapsed Time"});
            this.modeComboBox.Location = new System.Drawing.Point(23, 76);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(146, 21);
            this.modeComboBox.TabIndex = 2;
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // hoursBox
            // 
            this.hoursBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hoursBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hoursBox.Location = new System.Drawing.Point(25, 25);
            this.hoursBox.Name = "hoursBox";
            this.hoursBox.Size = new System.Drawing.Size(47, 35);
            this.hoursBox.TabIndex = 3;
            this.hoursBox.Text = "00";
            this.hoursBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // minutesBox
            // 
            this.minutesBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.minutesBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minutesBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minutesBox.Location = new System.Drawing.Point(116, 25);
            this.minutesBox.Name = "minutesBox";
            this.minutesBox.Size = new System.Drawing.Size(47, 35);
            this.minutesBox.TabIndex = 4;
            this.minutesBox.Text = "00";
            this.minutesBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // secondsBox
            // 
            this.secondsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.secondsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.secondsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondsBox.Location = new System.Drawing.Point(201, 25);
            this.secondsBox.Name = "secondsBox";
            this.secondsBox.Size = new System.Drawing.Size(47, 35);
            this.secondsBox.TabIndex = 5;
            this.secondsBox.Text = "00";
            this.secondsBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // divider1
            // 
            this.divider1.AutoSize = true;
            this.divider1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.divider1.Location = new System.Drawing.Point(87, 32);
            this.divider1.Name = "divider1";
            this.divider1.Size = new System.Drawing.Size(16, 24);
            this.divider1.TabIndex = 6;
            this.divider1.Text = ":";
            // 
            // divider2
            // 
            this.divider2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.divider2.AutoSize = true;
            this.divider2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.divider2.Location = new System.Drawing.Point(176, 32);
            this.divider2.Name = "divider2";
            this.divider2.Size = new System.Drawing.Size(16, 24);
            this.divider2.TabIndex = 7;
            this.divider2.Text = ":";
            // 
            // timer
            // 
            this.timer.Interval = 900;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysOnTopToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 26);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.alwaysOnTopToolStripMenuItem.Text = "Always on top";
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem_Click);
            // 
            // CounterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 114);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.divider2);
            this.Controls.Add(this.divider1);
            this.Controls.Add(this.secondsBox);
            this.Controls.Add(this.minutesBox);
            this.Controls.Add(this.hoursBox);
            this.Controls.Add(this.modeComboBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(275, 141);
            this.Name = "CounterForm";
            this.Text = "Counter";
            this.Load += new System.EventHandler(this.CounterForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox modeComboBox;
        private System.Windows.Forms.TextBox hoursBox;
        private System.Windows.Forms.TextBox minutesBox;
        private System.Windows.Forms.TextBox secondsBox;
        private System.Windows.Forms.Label divider1;
        private System.Windows.Forms.Label divider2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
    }
}