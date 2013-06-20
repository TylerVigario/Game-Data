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
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.averagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weeklyAveragesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calendarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.counterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supportedGamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusClear = new System.Windows.Forms.Timer(this.components);
            this.nextDay = new System.Windows.Forms.Timer(this.components);
            this.gamesList = new BrightIdeasSoftware.ObjectListView();
            this.GameName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Last_Played = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Sessions = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Total_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Last_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Maximum_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Minimum_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Average_Session_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.gamesListPrinter = new BrightIdeasSoftware.ListViewPrinter();
            this.updateChecker = new System.Windows.Forms.Timer(this.components);
            this.monthlyAveragesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.infoToolStripMenuItem,
            this.sessionsToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.gameMenu.Name = "gameMenu";
            this.gameMenu.Size = new System.Drawing.Size(119, 70);
            this.gameMenu.Opening += new System.ComponentModel.CancelEventHandler(this.gameMenu_Opening);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // sessionsToolStripMenuItem
            // 
            this.sessionsToolStripMenuItem.Name = "sessionsToolStripMenuItem";
            this.sessionsToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.sessionsToolStripMenuItem.Text = "Sessions";
            this.sessionsToolStripMenuItem.Click += new System.EventHandler(this.sessionsToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
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
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 275);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(526, 22);
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
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(526, 24);
            this.topMenu.TabIndex = 2;
            this.topMenu.Text = "menuStrip1";
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripMenuItem,
            this.averagesToolStripMenuItem,
            this.calendarToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // averagesToolStripMenuItem
            // 
            this.averagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monthlyAveragesToolStripMenuItem,
            this.weeklyAveragesToolStripMenuItem});
            this.averagesToolStripMenuItem.Name = "averagesToolStripMenuItem";
            this.averagesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.averagesToolStripMenuItem.Text = "Averages";
            // 
            // weeklyAveragesToolStripMenuItem
            // 
            this.weeklyAveragesToolStripMenuItem.Name = "weeklyAveragesToolStripMenuItem";
            this.weeklyAveragesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.weeklyAveragesToolStripMenuItem.Text = "Weekly Averages";
            this.weeklyAveragesToolStripMenuItem.Click += new System.EventHandler(this.statsToolStripMenuItem_Click);
            // 
            // calendarToolStripMenuItem
            // 
            this.calendarToolStripMenuItem.Name = "calendarToolStripMenuItem";
            this.calendarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.calendarToolStripMenuItem.Text = "Calendar";
            this.calendarToolStripMenuItem.Click += new System.EventHandler(this.calendarToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.counterToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // counterToolStripMenuItem
            // 
            this.counterToolStripMenuItem.Name = "counterToolStripMenuItem";
            this.counterToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.counterToolStripMenuItem.Text = "Counter";
            this.counterToolStripMenuItem.Click += new System.EventHandler(this.counterToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allSettingsToolStripMenuItem,
            this.supportedGamesToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // allSettingsToolStripMenuItem
            // 
            this.allSettingsToolStripMenuItem.Name = "allSettingsToolStripMenuItem";
            this.allSettingsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.allSettingsToolStripMenuItem.Text = "Settings";
            this.allSettingsToolStripMenuItem.Click += new System.EventHandler(this.allSettingsToolStripMenuItem_Click);
            // 
            // supportedGamesToolStripMenuItem
            // 
            this.supportedGamesToolStripMenuItem.Name = "supportedGamesToolStripMenuItem";
            this.supportedGamesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.supportedGamesToolStripMenuItem.Text = "Supported Games";
            this.supportedGamesToolStripMenuItem.Click += new System.EventHandler(this.supportedGamesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusClear
            // 
            this.statusClear.Tick += new System.EventHandler(this.statusClear_Tick);
            // 
            // nextDay
            // 
            this.nextDay.Tick += new System.EventHandler(this.nextDay_Tick);
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
            this.gamesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GameName,
            this.Last_Played,
            this.Sessions,
            this.Total_Time,
            this.Last_Session_Time});
            this.gamesList.ContextMenuStrip = this.gameMenu;
            this.gamesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamesList.EmptyListMsg = "";
            this.gamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gamesList.FullRowSelect = true;
            this.gamesList.Location = new System.Drawing.Point(0, 24);
            this.gamesList.MultiSelect = false;
            this.gamesList.Name = "gamesList";
            this.gamesList.ShowGroups = false;
            this.gamesList.Size = new System.Drawing.Size(526, 251);
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
            this.GameName.Width = 150;
            // 
            // Last_Played
            // 
            this.Last_Played.AspectName = "Last_Played";
            this.Last_Played.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Last_Played.MinimumWidth = 30;
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
            this.Sessions.Width = 44;
            // 
            // Total_Time
            // 
            this.Total_Time.AspectName = "Total_Time";
            this.Total_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Total_Time.MinimumWidth = 30;
            this.Total_Time.Text = "Total Time";
            this.Total_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Total_Time.ToolTipText = "Total Total";
            this.Total_Time.Width = 110;
            // 
            // Last_Session_Time
            // 
            this.Last_Session_Time.AspectName = "Last_Session_Time";
            this.Last_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Last_Session_Time.MinimumWidth = 30;
            this.Last_Session_Time.Text = "Last Session Time";
            this.Last_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Last_Session_Time.ToolTipText = "Last Session Time";
            this.Last_Session_Time.Width = 89;
            // 
            // Maximum_Session_Time
            // 
            this.Maximum_Session_Time.AspectName = "Maximum_Session_Time";
            this.Maximum_Session_Time.DisplayIndex = 5;
            this.Maximum_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Maximum_Session_Time.IsVisible = false;
            this.Maximum_Session_Time.MinimumWidth = 30;
            this.Maximum_Session_Time.Text = "Maximum Session Time";
            this.Maximum_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Maximum_Session_Time.ToolTipText = "Maximum Session Time";
            this.Maximum_Session_Time.Width = 110;
            // 
            // Minimum_Session_Time
            // 
            this.Minimum_Session_Time.AspectName = "Minimum_Session_Time";
            this.Minimum_Session_Time.DisplayIndex = 6;
            this.Minimum_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Minimum_Session_Time.IsVisible = false;
            this.Minimum_Session_Time.MinimumWidth = 30;
            this.Minimum_Session_Time.Text = "Minimum Session Time";
            this.Minimum_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Minimum_Session_Time.ToolTipText = "Minimum Session Time";
            this.Minimum_Session_Time.Width = 110;
            // 
            // Average_Session_Time
            // 
            this.Average_Session_Time.AspectName = "Average_Session_Time";
            this.Average_Session_Time.DisplayIndex = 7;
            this.Average_Session_Time.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Average_Session_Time.IsVisible = false;
            this.Average_Session_Time.MinimumWidth = 30;
            this.Average_Session_Time.Text = "Average Session Time";
            this.Average_Session_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Average_Session_Time.ToolTipText = "Average Session Time";
            this.Average_Session_Time.Width = 110;
            // 
            // gamesListPrinter
            // 
            // 
            // 
            // 
            this.gamesListPrinter.CellFormat.CanWrap = true;
            this.gamesListPrinter.CellFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.gamesListPrinter.DocumentName = "Game Data";
            // 
            // 
            // 
            this.gamesListPrinter.FooterFormat.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Italic);
            // 
            // 
            // 
            this.gamesListPrinter.GroupHeaderFormat.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            // 
            // 
            // 
            this.gamesListPrinter.HeaderFormat.Font = new System.Drawing.Font("Verdana", 24F);
            // 
            // 
            // 
            this.gamesListPrinter.ListHeaderFormat.CanWrap = true;
            this.gamesListPrinter.ListHeaderFormat.Font = new System.Drawing.Font("Verdana", 12F);
            this.gamesListPrinter.ListView = this.gamesList;
            // 
            // updateChecker
            // 
            this.updateChecker.Tick += new System.EventHandler(this.updateChecker_Tick);
            // 
            // monthlyAveragesToolStripMenuItem
            // 
            this.monthlyAveragesToolStripMenuItem.Name = "monthlyAveragesToolStripMenuItem";
            this.monthlyAveragesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.monthlyAveragesToolStripMenuItem.Text = "Monthly Averages";
            this.monthlyAveragesToolStripMenuItem.Visible = false;
            this.monthlyAveragesToolStripMenuItem.Click += new System.EventHandler(this.monthlyAveragesToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 297);
            this.Controls.Add(this.gamesList);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.topMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.topMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Game Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
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
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Timer statusClear;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supportedGamesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip gameMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer nextDay;
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
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private BrightIdeasSoftware.ListViewPrinter gamesListPrinter;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem counterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.Timer updateChecker;
        private System.Windows.Forms.ToolStripMenuItem calendarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem averagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem weeklyAveragesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monthlyAveragesToolStripMenuItem;
    }
}

