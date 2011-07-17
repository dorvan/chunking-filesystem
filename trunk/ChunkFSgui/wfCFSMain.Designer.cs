namespace ChunkFSgui
{
    partial class wfCFSMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wfCFSMain));
            this.scSubMain = new System.Windows.Forms.SplitContainer();
            this.fpDriveActions = new System.Windows.Forms.FlowLayoutPanel();
            this.pbMount = new System.Windows.Forms.Button();
            this.pbUnmount = new System.Windows.Forms.Button();
            this.pbMonitor = new System.Windows.Forms.Button();
            this.pbSave = new System.Windows.Forms.Button();
            this.pbLoad = new System.Windows.Forms.Button();
            this.pbRestore = new System.Windows.Forms.Button();
            this.pbAbout = new System.Windows.Forms.Button();
            this.dgConfigs = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mountPointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chunkSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UseExtension = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.runningChunksDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ShowSubdirs = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SmartChunk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.minimumChunkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mounted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.driveConfigBindingListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgFiles = new System.Windows.Forms.DataGridView();
            this.fileChoiceBindingListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fpFileActions = new System.Windows.Forms.FlowLayoutPanel();
            this.pbDown = new System.Windows.Forms.Button();
            this.pbUp = new System.Windows.Forms.Button();
            this.pbAdd = new System.Windows.Forms.Button();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scSubMain.Panel1.SuspendLayout();
            this.scSubMain.Panel2.SuspendLayout();
            this.scSubMain.SuspendLayout();
            this.fpDriveActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgConfigs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driveConfigBindingListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileChoiceBindingListBindingSource)).BeginInit();
            this.fpFileActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // scSubMain
            // 
            this.scSubMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSubMain.Location = new System.Drawing.Point(0, 0);
            this.scSubMain.Name = "scSubMain";
            // 
            // scSubMain.Panel1
            // 
            this.scSubMain.Panel1.Controls.Add(this.fpDriveActions);
            this.scSubMain.Panel1.Controls.Add(this.dgConfigs);
            this.scSubMain.Panel1MinSize = 530;
            // 
            // scSubMain.Panel2
            // 
            this.scSubMain.Panel2.Controls.Add(this.dgFiles);
            this.scSubMain.Panel2.Controls.Add(this.fpFileActions);
            this.scSubMain.Panel2MinSize = 120;
            this.scSubMain.Size = new System.Drawing.Size(960, 340);
            this.scSubMain.SplitterDistance = 644;
            this.scSubMain.TabIndex = 0;
            // 
            // fpDriveActions
            // 
            this.fpDriveActions.Controls.Add(this.pbMount);
            this.fpDriveActions.Controls.Add(this.pbUnmount);
            this.fpDriveActions.Controls.Add(this.pbMonitor);
            this.fpDriveActions.Controls.Add(this.pbSave);
            this.fpDriveActions.Controls.Add(this.pbLoad);
            this.fpDriveActions.Controls.Add(this.pbRestore);
            this.fpDriveActions.Controls.Add(this.pbAbout);
            this.fpDriveActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fpDriveActions.Location = new System.Drawing.Point(0, 309);
            this.fpDriveActions.MinimumSize = new System.Drawing.Size(530, 0);
            this.fpDriveActions.Name = "fpDriveActions";
            this.fpDriveActions.Size = new System.Drawing.Size(644, 31);
            this.fpDriveActions.TabIndex = 1;
            // 
            // pbMount
            // 
            this.pbMount.Location = new System.Drawing.Point(3, 3);
            this.pbMount.Name = "pbMount";
            this.pbMount.Size = new System.Drawing.Size(77, 23);
            this.pbMount.TabIndex = 1;
            this.pbMount.Text = "Mount";
            this.pbMount.UseVisualStyleBackColor = true;
            this.pbMount.Click += new System.EventHandler(this.pbMount_Click);
            // 
            // pbUnmount
            // 
            this.pbUnmount.Location = new System.Drawing.Point(86, 3);
            this.pbUnmount.Name = "pbUnmount";
            this.pbUnmount.Size = new System.Drawing.Size(74, 23);
            this.pbUnmount.TabIndex = 2;
            this.pbUnmount.Text = "UnMount";
            this.pbUnmount.UseVisualStyleBackColor = true;
            this.pbUnmount.Click += new System.EventHandler(this.pbUnmount_Click);
            // 
            // pbMonitor
            // 
            this.pbMonitor.Location = new System.Drawing.Point(166, 3);
            this.pbMonitor.Name = "pbMonitor";
            this.pbMonitor.Size = new System.Drawing.Size(75, 23);
            this.pbMonitor.TabIndex = 5;
            this.pbMonitor.Text = "Monitor";
            this.pbMonitor.UseVisualStyleBackColor = true;
            this.pbMonitor.Click += new System.EventHandler(this.pbMonitor_Click);
            // 
            // pbSave
            // 
            this.pbSave.Location = new System.Drawing.Point(247, 3);
            this.pbSave.Name = "pbSave";
            this.pbSave.Size = new System.Drawing.Size(75, 23);
            this.pbSave.TabIndex = 3;
            this.pbSave.Text = "Save Config";
            this.pbSave.UseVisualStyleBackColor = true;
            this.pbSave.Click += new System.EventHandler(this.pbSave_Click);
            // 
            // pbLoad
            // 
            this.pbLoad.Location = new System.Drawing.Point(328, 3);
            this.pbLoad.Name = "pbLoad";
            this.pbLoad.Size = new System.Drawing.Size(75, 23);
            this.pbLoad.TabIndex = 4;
            this.pbLoad.Text = "Load Config";
            this.pbLoad.UseVisualStyleBackColor = true;
            this.pbLoad.Click += new System.EventHandler(this.pbLoad_Click);
            // 
            // pbRestore
            // 
            this.pbRestore.Location = new System.Drawing.Point(446, 3);
            this.pbRestore.Margin = new System.Windows.Forms.Padding(40, 3, 3, 3);
            this.pbRestore.Name = "pbRestore";
            this.pbRestore.Size = new System.Drawing.Size(75, 23);
            this.pbRestore.TabIndex = 4;
            this.pbRestore.Text = "Restore";
            this.pbRestore.UseVisualStyleBackColor = true;
            this.pbRestore.Click += new System.EventHandler(this.pbRestore_Click);
            // 
            // pbAbout
            // 
            this.pbAbout.Location = new System.Drawing.Point(564, 3);
            this.pbAbout.Margin = new System.Windows.Forms.Padding(40, 3, 3, 3);
            this.pbAbout.Name = "pbAbout";
            this.pbAbout.Size = new System.Drawing.Size(75, 23);
            this.pbAbout.TabIndex = 6;
            this.pbAbout.Text = "About";
            this.pbAbout.UseVisualStyleBackColor = true;
            this.pbAbout.Click += new System.EventHandler(this.pbAbout_Click);
            // 
            // dgConfigs
            // 
            this.dgConfigs.AutoGenerateColumns = false;
            this.dgConfigs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgConfigs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.mountPointDataGridViewTextBoxColumn,
            this.chunkSizeDataGridViewTextBoxColumn,
            this.UseExtension,
            this.runningChunksDataGridViewCheckBoxColumn,
            this.ShowSubdirs,
            this.SmartChunk,
            this.minimumChunkDataGridViewTextBoxColumn,
            this.Mounted});
            this.dgConfigs.DataSource = this.driveConfigBindingListBindingSource;
            this.dgConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgConfigs.Location = new System.Drawing.Point(0, 0);
            this.dgConfigs.MultiSelect = false;
            this.dgConfigs.Name = "dgConfigs";
            this.dgConfigs.Size = new System.Drawing.Size(644, 340);
            this.dgConfigs.TabIndex = 0;
            this.dgConfigs.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgConfigs_RowEnter);
            this.dgConfigs.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgConfigs_CellEndEdit);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ToolTipText = "A short name for you to recognize; use only characters that allowed in a volume l" +
                "abel";
            this.nameDataGridViewTextBoxColumn.Width = 95;
            // 
            // mountPointDataGridViewTextBoxColumn
            // 
            this.mountPointDataGridViewTextBoxColumn.DataPropertyName = "MountPoint";
            this.mountPointDataGridViewTextBoxColumn.HeaderText = "Mount Point";
            this.mountPointDataGridViewTextBoxColumn.Name = "mountPointDataGridViewTextBoxColumn";
            this.mountPointDataGridViewTextBoxColumn.ToolTipText = "Volume root path e.g. M:\\, cannot be in use, cannot be A or B";
            this.mountPointDataGridViewTextBoxColumn.Width = 90;
            // 
            // chunkSizeDataGridViewTextBoxColumn
            // 
            this.chunkSizeDataGridViewTextBoxColumn.DataPropertyName = "ChunkSize";
            this.chunkSizeDataGridViewTextBoxColumn.HeaderText = "Chunk Size";
            this.chunkSizeDataGridViewTextBoxColumn.Name = "chunkSizeDataGridViewTextBoxColumn";
            this.chunkSizeDataGridViewTextBoxColumn.ToolTipText = "Size of chunks (or combined segments, if \'Fill\') e.g.  4.5G (use B, K, M, or G)";
            this.chunkSizeDataGridViewTextBoxColumn.Width = 90;
            // 
            // UseExtension
            // 
            this.UseExtension.DataPropertyName = "UseExtension";
            this.UseExtension.HeaderText = "Use Ext";
            this.UseExtension.Name = "UseExtension";
            this.UseExtension.ToolTipText = "Set to use the original file extension on each chunk";
            this.UseExtension.Width = 50;
            // 
            // runningChunksDataGridViewCheckBoxColumn
            // 
            this.runningChunksDataGridViewCheckBoxColumn.DataPropertyName = "RunningChunks";
            this.runningChunksDataGridViewCheckBoxColumn.HeaderText = "Fill";
            this.runningChunksDataGridViewCheckBoxColumn.Name = "runningChunksDataGridViewCheckBoxColumn";
            this.runningChunksDataGridViewCheckBoxColumn.ToolTipText = "Set to carry forward the remaining space in a chunk from file to file";
            this.runningChunksDataGridViewCheckBoxColumn.Width = 50;
            // 
            // ShowSubdirs
            // 
            this.ShowSubdirs.DataPropertyName = "ShowSubdirs";
            this.ShowSubdirs.HeaderText = "Subdirs";
            this.ShowSubdirs.Name = "ShowSubdirs";
            this.ShowSubdirs.ToolTipText = "Select to enable subdirectories for groups (only applies if also Fill)";
            this.ShowSubdirs.Width = 50;
            // 
            // SmartChunk
            // 
            this.SmartChunk.DataPropertyName = "SmartChunk";
            this.SmartChunk.HeaderText = "Smart";
            this.SmartChunk.Name = "SmartChunk";
            this.SmartChunk.ToolTipText = "Try to split supported file types at meaningful breakpoints";
            this.SmartChunk.Width = 50;
            // 
            // minimumChunkDataGridViewTextBoxColumn
            // 
            this.minimumChunkDataGridViewTextBoxColumn.DataPropertyName = "MinimumChunk";
            this.minimumChunkDataGridViewTextBoxColumn.HeaderText = "Minimum";
            this.minimumChunkDataGridViewTextBoxColumn.Name = "minimumChunkDataGridViewTextBoxColumn";
            this.minimumChunkDataGridViewTextBoxColumn.ToolTipText = "When \'Fill\'ing, do not make a part-1 segment smaller than this portion of a full " +
                "chunk (0 to disable)";
            this.minimumChunkDataGridViewTextBoxColumn.Width = 65;
            // 
            // Mounted
            // 
            this.Mounted.DataPropertyName = "Mounted";
            this.Mounted.HeaderText = "Mounted";
            this.Mounted.Name = "Mounted";
            this.Mounted.ReadOnly = true;
            this.Mounted.ToolTipText = "Indicates if this configuration is currently mounted";
            this.Mounted.Width = 60;
            // 
            // driveConfigBindingListBindingSource
            // 
            this.driveConfigBindingListBindingSource.DataSource = typeof(ChunkFSgui.DriveConfigBindingList);
            // 
            // dgFiles
            // 
            this.dgFiles.AutoGenerateColumns = false;
            this.dgFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.fileSizeDataGridViewTextBoxColumn});
            this.dgFiles.DataSource = this.fileChoiceBindingListBindingSource;
            this.dgFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgFiles.Location = new System.Drawing.Point(0, 0);
            this.dgFiles.Name = "dgFiles";
            this.dgFiles.Size = new System.Drawing.Size(312, 309);
            this.dgFiles.TabIndex = 0;
            this.dgFiles.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgFiles_CellEndEdit);
            // 
            // fileChoiceBindingListBindingSource
            // 
            this.fileChoiceBindingListBindingSource.DataSource = typeof(ChunkFSgui.FileChoiceBindingList);
            // 
            // fpFileActions
            // 
            this.fpFileActions.Controls.Add(this.pbDown);
            this.fpFileActions.Controls.Add(this.pbUp);
            this.fpFileActions.Controls.Add(this.pbAdd);
            this.fpFileActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fpFileActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.fpFileActions.Location = new System.Drawing.Point(0, 309);
            this.fpFileActions.Name = "fpFileActions";
            this.fpFileActions.Size = new System.Drawing.Size(312, 31);
            this.fpFileActions.TabIndex = 1;
            // 
            // pbDown
            // 
            this.pbDown.Location = new System.Drawing.Point(234, 3);
            this.pbDown.Name = "pbDown";
            this.pbDown.Size = new System.Drawing.Size(75, 23);
            this.pbDown.TabIndex = 1;
            this.pbDown.Text = "Down";
            this.pbDown.UseVisualStyleBackColor = true;
            this.pbDown.Click += new System.EventHandler(this.pbDown_Click);
            // 
            // pbUp
            // 
            this.pbUp.Location = new System.Drawing.Point(153, 3);
            this.pbUp.Name = "pbUp";
            this.pbUp.Size = new System.Drawing.Size(75, 23);
            this.pbUp.TabIndex = 0;
            this.pbUp.Text = "Up";
            this.pbUp.UseVisualStyleBackColor = true;
            this.pbUp.Click += new System.EventHandler(this.pbUp_Click);
            // 
            // pbAdd
            // 
            this.pbAdd.Location = new System.Drawing.Point(72, 3);
            this.pbAdd.Name = "pbAdd";
            this.pbAdd.Size = new System.Drawing.Size(75, 23);
            this.pbAdd.TabIndex = 2;
            this.pbAdd.Text = "Add";
            this.pbAdd.UseVisualStyleBackColor = true;
            this.pbAdd.Click += new System.EventHandler(this.pbAdd_Click);
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "File Name";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.ToolTipText = "Directory, or exact file name (full path) or matching expression with * or ? wild" +
                "cards";
            this.fileNameDataGridViewTextBoxColumn.Width = 180;
            // 
            // fileSizeDataGridViewTextBoxColumn
            // 
            this.fileSizeDataGridViewTextBoxColumn.DataPropertyName = "FileSizeM";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.fileSizeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.fileSizeDataGridViewTextBoxColumn.HeaderText = "Total Size";
            this.fileSizeDataGridViewTextBoxColumn.Name = "fileSizeDataGridViewTextBoxColumn";
            this.fileSizeDataGridViewTextBoxColumn.ReadOnly = true;
            this.fileSizeDataGridViewTextBoxColumn.ToolTipText = "Size of the file (or all matching files)";
            this.fileSizeDataGridViewTextBoxColumn.Width = 90;
            // 
            // wfCFSMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 340);
            this.Controls.Add(this.scSubMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "wfCFSMain";
            this.Text = "Chunking File System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.scSubMain.Panel1.ResumeLayout(false);
            this.scSubMain.Panel2.ResumeLayout(false);
            this.scSubMain.ResumeLayout(false);
            this.fpDriveActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgConfigs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driveConfigBindingListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileChoiceBindingListBindingSource)).EndInit();
            this.fpFileActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button pbMount;
        private System.Windows.Forms.Button pbUnmount;
        private System.Windows.Forms.Button pbSave;
        private System.Windows.Forms.Button pbLoad;
        private System.Windows.Forms.SplitContainer scSubMain;
        private System.Windows.Forms.DataGridView dgConfigs;
        private System.Windows.Forms.DataGridView dgFiles;
        private System.Windows.Forms.Button pbUp;
        private System.Windows.Forms.Button pbDown;
        private System.Windows.Forms.Button pbAdd;
        private System.Windows.Forms.BindingSource driveConfigBindingListBindingSource;
        private System.Windows.Forms.FlowLayoutPanel fpDriveActions;
        private System.Windows.Forms.FlowLayoutPanel fpFileActions;
        private System.Windows.Forms.BindingSource fileChoiceBindingListBindingSource;
        private System.Windows.Forms.Button pbMonitor;
        private System.Windows.Forms.Button pbRestore;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mountPointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chunkSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UseExtension;
        private System.Windows.Forms.DataGridViewCheckBoxColumn runningChunksDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ShowSubdirs;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SmartChunk;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimumChunkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Mounted;
        private System.Windows.Forms.Button pbAbout;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileSizeDataGridViewTextBoxColumn;
    }
}

