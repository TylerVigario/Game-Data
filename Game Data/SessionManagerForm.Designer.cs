namespace Game_Data
{
    partial class SessionManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionManagerForm));
            this.sessionsList = new BrightIdeasSoftware.ObjectListView();
            this.Start_Time = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Time_Span = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.sessionMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.sessionsList)).BeginInit();
            this.sessionMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // sessionsList
            // 
            this.sessionsList.AllColumns.Add(this.Start_Time);
            this.sessionsList.AllColumns.Add(this.Time_Span);
            this.sessionsList.CellEditUseWholeCell = false;
            this.sessionsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Start_Time,
            this.Time_Span});
            this.sessionsList.ContextMenuStrip = this.sessionMenu;
            this.sessionsList.Cursor = System.Windows.Forms.Cursors.Default;
            this.sessionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionsList.FullRowSelect = true;
            this.sessionsList.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.sessionsList.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.sessionsList.Location = new System.Drawing.Point(0, 0);
            this.sessionsList.Name = "sessionsList";
            this.sessionsList.ShowItemCountOnGroups = true;
            this.sessionsList.Size = new System.Drawing.Size(242, 299);
            this.sessionsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.sessionsList.TabIndex = 0;
            this.sessionsList.UseCompatibleStateImageBehavior = false;
            this.sessionsList.View = System.Windows.Forms.View.Details;
            this.sessionsList.SelectionChanged += new System.EventHandler(this.sessionsList_SelectionChanged);
            // 
            // Start_Time
            // 
            this.Start_Time.AspectName = "Start_Time";
            this.Start_Time.MinimumWidth = 30;
            this.Start_Time.Text = "Date";
            this.Start_Time.Width = 117;
            // 
            // Time_Span
            // 
            this.Time_Span.AspectName = "Time_Span";
            this.Time_Span.Groupable = false;
            this.Time_Span.MinimumWidth = 30;
            this.Time_Span.Text = "Time";
            this.Time_Span.Width = 119;
            // 
            // sessionMenu
            // 
            this.sessionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.mergToolStripMenuItem});
            this.sessionMenu.Name = "sessionsMenu";
            this.sessionMenu.Size = new System.Drawing.Size(153, 114);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Enabled = false;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // mergToolStripMenuItem
            // 
            this.mergToolStripMenuItem.Enabled = false;
            this.mergToolStripMenuItem.Name = "mergToolStripMenuItem";
            this.mergToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mergToolStripMenuItem.Text = "Merge";
            this.mergToolStripMenuItem.Click += new System.EventHandler(this.mergToolStripMenuItem_Click);
            // 
            // SessionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 299);
            this.Controls.Add(this.sessionsList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SessionManagerForm";
            this.ShowIcon = false;
            this.Text = "Session Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SessionManagerForm_FormClosing);
            this.Load += new System.EventHandler(this.SessionManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sessionsList)).EndInit();
            this.sessionMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView sessionsList;
        private BrightIdeasSoftware.OLVColumn Start_Time;
        private BrightIdeasSoftware.OLVColumn Time_Span;
        private System.Windows.Forms.ContextMenuStrip sessionMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergToolStripMenuItem;
    }
}