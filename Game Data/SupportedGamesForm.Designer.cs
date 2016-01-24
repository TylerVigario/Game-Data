namespace Game_Data
{
    partial class SupportedGamesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SupportedGamesForm));
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.supportedGamesList = new BrightIdeasSoftware.FastObjectListView();
            this.Game_Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Process_Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.supportedGamesMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.supportedGamesList)).BeginInit();
            this.supportedGamesMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(12, 347);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(174, 347);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 3;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editButton.Location = new System.Drawing.Point(93, 347);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(75, 23);
            this.editButton.TabIndex = 4;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // supportedGamesList
            // 
            this.supportedGamesList.AllColumns.Add(this.Game_Name);
            this.supportedGamesList.AllColumns.Add(this.Process_Name);
            this.supportedGamesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.supportedGamesList.CellEditUseWholeCell = false;
            this.supportedGamesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Game_Name,
            this.Process_Name});
            this.supportedGamesList.ContextMenuStrip = this.supportedGamesMenu;
            this.supportedGamesList.Cursor = System.Windows.Forms.Cursors.Default;
            this.supportedGamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.supportedGamesList.FullRowSelect = true;
            this.supportedGamesList.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.supportedGamesList.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.supportedGamesList.Location = new System.Drawing.Point(12, 12);
            this.supportedGamesList.Name = "supportedGamesList";
            this.supportedGamesList.ShowGroups = false;
            this.supportedGamesList.Size = new System.Drawing.Size(356, 329);
            this.supportedGamesList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.supportedGamesList.TabIndex = 5;
            this.supportedGamesList.UseCompatibleStateImageBehavior = false;
            this.supportedGamesList.View = System.Windows.Forms.View.Details;
            this.supportedGamesList.VirtualMode = true;
            this.supportedGamesList.SelectionChanged += new System.EventHandler(this.supportedGamesList_SelectionChanged);
            // 
            // Game_Name
            // 
            this.Game_Name.AspectName = "Game_Name";
            this.Game_Name.Hideable = false;
            this.Game_Name.MinimumWidth = 30;
            this.Game_Name.Text = "Game Name";
            this.Game_Name.Width = 200;
            // 
            // Process_Name
            // 
            this.Process_Name.AspectName = "Process_Name";
            this.Process_Name.Hideable = false;
            this.Process_Name.MinimumWidth = 30;
            this.Process_Name.Text = "Process Name";
            this.Process_Name.Width = 150;
            // 
            // supportedGamesMenu
            // 
            this.supportedGamesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.supportedGamesMenu.Name = "supportedGamesMenu";
            this.supportedGamesMenu.Size = new System.Drawing.Size(118, 70);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Enabled = false;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // SupportedGamesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 379);
            this.Controls.Add(this.supportedGamesList);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SupportedGamesForm";
            this.Text = "Supported Games Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SupportedGamesForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SupportedGamesForm_FormClosed);
            this.Load += new System.EventHandler(this.SupportedGamesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.supportedGamesList)).EndInit();
            this.supportedGamesMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button editButton;
        private BrightIdeasSoftware.FastObjectListView supportedGamesList;
        private BrightIdeasSoftware.OLVColumn Game_Name;
        private BrightIdeasSoftware.OLVColumn Process_Name;
        private System.Windows.Forms.ContextMenuStrip supportedGamesMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
    }
}