namespace Game_Data
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.gameMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sessionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weeklyAveragesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionsLengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusClear = new System.Windows.Forms.Timer(this.components);
            this.gamesList = new BrightIdeasSoftware.ObjectListView();
            this.GameName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Last_Played = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Sessions = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Total_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Last_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Maximum_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Minimum_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Average_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.listUpdate = new System.Windows.Forms.Timer(this.components);
            this.gameMenu.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.topMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gamesList)).BeginInit();
            this.SuspendLayout();
            // 
            // gameMenu
            // 
            this.gameMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sessionsToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.gameMenu.Name = "gameMenu";
            this.gameMenu.Size = new System.Drawing.Size(119, 48);
            this.gameMenu.Opening += new System.ComponentModel.CancelEventHandler(this.gameMenu_Opening);
            // 
            // sessionsToolStripMenuItem
            // 
            this.sessionsToolStripMenuItem.Enabled = false;
            this.sessionsToolStripMenuItem.Name = "sessionsToolStripMenuItem";
            this.sessionsToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.sessionsToolStripMenuItem.Text = "Sessions";
            this.sessionsToolStripMenuItem.Click += new System.EventHandler(this.sessionsToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Enabled = false;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Game Data";
            this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.settingsToolStripMenuItem1,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(117, 76);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem1.Text = "Settings";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.settingsToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 200);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(536, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(536, 24);
            this.topMenu.TabIndex = 2;
            this.topMenu.Text = "menuStrip1";
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.weeklyAveragesToolStripMenuItem,
            this.sessionsLengthToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Enabled = false;
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.infoToolStripMenuItem.Text = "Game Info";
            this.infoToolStripMenuItem.Visible = false;
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // weeklyAveragesToolStripMenuItem
            // 
            this.weeklyAveragesToolStripMenuItem.Enabled = false;
            this.weeklyAveragesToolStripMenuItem.Name = "weeklyAveragesToolStripMenuItem";
            this.weeklyAveragesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.weeklyAveragesToolStripMenuItem.Text = "Weekly Averages";
            this.weeklyAveragesToolStripMenuItem.Click += new System.EventHandler(this.statsToolStripMenuItem_Click);
            // 
            // sessionsLengthToolStripMenuItem
            // 
            this.sessionsLengthToolStripMenuItem.Enabled = false;
            this.sessionsLengthToolStripMenuItem.Name = "sessionsLengthToolStripMenuItem";
            this.sessionsLengthToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.sessionsLengthToolStripMenuItem.Text = "Sessions Length";
            this.sessionsLengthToolStripMenuItem.Click += new System.EventHandler(this.sessionsLengthToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.toolStripMenuItem1.Text = "Supported Games";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.supportedGamesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // settingsToolStripMenuItem2
            // 
            this.settingsToolStripMenuItem2.Name = "settingsToolStripMenuItem2";
            this.settingsToolStripMenuItem2.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem2.Text = "Settings";
            this.settingsToolStripMenuItem2.Click += new System.EventHandler(this.allSettingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusClear
            // 
            this.statusClear.Tick += new System.EventHandler(this.statusClear_Tick);
            // 
            // gamesList
            // 
            this.gamesList.AllColumns.Add(this.GameName);
            this.gamesList.AllColumns.Add(this.Last_Played);
            this.gamesList.AllColumns.Add(this.Sessions);
            this.gamesList.AllColumns.Add(this.Total_Time);
            this.gamesList.AllColumns.Add(this.Last_Session_Time);
            this.gamesList.AllColumns.Add(this.Maximum_Session_Time);
            this.gamesList.AllColumns.Add(this.Minimum_Session_Time);
            this.gamesList.AllColumns.Add(this.Average_Session_Time);
            this.gamesList.AllowColumnReorder = true;
            this.gamesList.CellEditUseWholeCell = false;
            this.gamesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GameName,
            this.Last_Played,
            this.Sessions,
            this.Total_Time,
            this.Last_Session_Time});
            this.gamesList.ContextMenuStrip = this.gameMenu;
            this.gamesList.Cursor = System.Windows.Forms.Cursors.Default;
            this.gamesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamesList.EmptyListMsg = "";
            this.gamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gamesList.FullRowSelect = true;
            this.gamesList.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.gamesList.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.gamesList.Location = new System.Drawing.Point(0, 24);
            this.gamesList.MultiSelect = false;
            this.gamesList.Name = "gamesList";
            this.gamesList.ShowGroups = false;
            this.gamesList.Size = new System.Drawing.Size(536, 176);
            this.gamesList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.gamesList.TabIndex = 3;
            this.gamesList.UseCompatibleStateImageBehavior = false;
            this.gamesList.View = System.Windows.Forms.View.Details;
            this.gamesList.SelectionChanged += new System.EventHandler(this.gamesList_SelectionChanged);
            this.gamesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gamesList_MouseDoubleClick);
            // 
            // GameName
            // 
            this.GameName.AspectName = "Name";
            this.GameName.FillsFreeSpace = true;
            this.GameName.Hideable = false;
            this.GameName.MinimumWidth = 30;
            this.GameName.Text = "Name";
            this.GameName.ToolTipText = "Name";
            this.GameName.Width = 225;
            // 
            // Last_Played
            // 
            this.Last_Played.AspectName = "Last_Played";
            this.Last_Played.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Last_Played.MinimumWidth = 30;
            this.Last_Played.Sortable = false;
            this.Last_Played.Text = "Last Played";
            this.Last_Played.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Last_Played.ToolTipText = "Last Time Played";
            this.Last_Played.Width = 117;
            // 
            // Sessions
            // 
            this.Sessions.AspectName = "Sessions";
            this.Sessions.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Sessions.MinimumWidth = 30;
            this.Sessions.Text = "Sessions";
            this.Sessions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Sessions.ToolTipText = "Number of Sessions";
            this.Sessions.Width = 40;
            // 
            // Total_Time
            // 
            this.Total_Time.AspectName = "Total_Time";
            this.Total_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Total_Time.MinimumWidth = 30;
            this.Total_Time.Text = "Total";
            this.Total_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Total_Time.ToolTipText = "Total Total";
            this.Total_Time.Width = 75;
            // 
            // Last_Session_Time
            // 
            this.Last_Session_Time.AspectName = "Last_Session_Time";
            this.Last_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Last_Session_Time.MinimumWidth = 30;
            this.Last_Session_Time.Text = "Last Session";
            this.Last_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Last_Session_Time.ToolTipText = "Last Session Time";
            this.Last_Session_Time.Width = 75;
            // 
            // Maximum_Session_Time
            // 
            this.Maximum_Session_Time.AspectName = "Maximum_Session_Time";
            this.Maximum_Session_Time.DisplayIndex = 5;
            this.Maximum_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Maximum_Session_Time.IsVisible = false;
            this.Maximum_Session_Time.MinimumWidth = 30;
            this.Maximum_Session_Time.Text = "Maximum Session";
            this.Maximum_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Maximum_Session_Time.ToolTipText = "Maximum Session Time";
            this.Maximum_Session_Time.Width = 75;
            // 
            // Minimum_Session_Time
            // 
            this.Minimum_Session_Time.AspectName = "Minimum_Session_Time";
            this.Minimum_Session_Time.DisplayIndex = 6;
            this.Minimum_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Minimum_Session_Time.IsVisible = false;
            this.Minimum_Session_Time.MinimumWidth = 30;
            this.Minimum_Session_Time.Text = "Minimum Session";
            this.Minimum_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Minimum_Session_Time.ToolTipText = "Minimum Session Time";
            this.Minimum_Session_Time.Width = 75;
            // 
            // Average_Session_Time
            // 
            this.Average_Session_Time.AspectName = "Average_Session_Time";
            this.Average_Session_Time.DisplayIndex = 7;
            this.Average_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Average_Session_Time.IsVisible = false;
            this.Average_Session_Time.MinimumWidth = 30;
            this.Average_Session_Time.Text = "Average Session";
            this.Average_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Average_Session_Time.ToolTipText = "Average Session Time";
            this.Average_Session_Time.Width = 75;
            // 
            // listUpdate
            // 
            this.listUpdate.Interval = 60000;
            this.listUpdate.Tick += new System.EventHandler(this.listUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 222);
            this.Controls.Add(this.gamesList);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.topMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.topMenu;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 220);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Game Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.gameMenu.ResumeLayout(false);
            this.trayMenu.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gamesList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Timer statusClear;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip gameMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private BrightIdeasSoftware.ObjectListView gamesList;
        private BrightIdeasSoftware.OLVColumn GameName;
        private BrightIdeasSoftware.OLVColumn Last_Played;
        private BrightIdeasSoftware.OLVColumn Sessions;
        private BrightIdeasSoftware.OLVColumn Average_Session_Time;
        private BrightIdeasSoftware.OLVColumn Total_Time;
        private BrightIdeasSoftware.OLVColumn Maximum_Session_Time;
        private BrightIdeasSoftware.OLVColumn Minimum_Session_Time;
        private BrightIdeasSoftware.OLVColumn Last_Session_Time;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem sessionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem weeklyAveragesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionsLengthToolStripMenuItem;
        private System.Windows.Forms.Timer listUpdate;
    }
}

