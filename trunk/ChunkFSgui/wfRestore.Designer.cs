namespace ChunkFSgui
{
    partial class wfRestore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wfRestore));
            this.label1 = new System.Windows.Forms.Label();
            this.tbFirstFile = new System.Windows.Forms.TextBox();
            this.pbFindFile = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.pbGo = new System.Windows.Forms.Button();
            this.pbCancel = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbOutPath = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pbFindDir = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First file:";
            // 
            // tbFirstFile
            // 
            this.tbFirstFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFirstFile.Location = new System.Drawing.Point(65, 3);
            this.tbFirstFile.Name = "tbFirstFile";
            this.tbFirstFile.Size = new System.Drawing.Size(243, 20);
            this.tbFirstFile.TabIndex = 1;
            this.tbFirstFile.TextChanged += new System.EventHandler(this.tbFirstFile_TextChanged);
            // 
            // pbFindFile
            // 
            this.pbFindFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbFindFile.Location = new System.Drawing.Point(314, 3);
            this.pbFindFile.Name = "pbFindFile";
            this.pbFindFile.Size = new System.Drawing.Size(28, 20);
            this.pbFindFile.TabIndex = 2;
            this.pbFindFile.Text = "...";
            this.pbFindFile.UseVisualStyleBackColor = true;
            this.pbFindFile.Click += new System.EventHandler(this.pbFindFile_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(10, 273);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.pbGo);
            this.flowLayoutPanel3.Controls.Add(this.pbCancel);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(10, 242);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(345, 31);
            this.flowLayoutPanel3.TabIndex = 2;
            // 
            // pbGo
            // 
            this.pbGo.Enabled = false;
            this.pbGo.Location = new System.Drawing.Point(3, 3);
            this.pbGo.Name = "pbGo";
            this.pbGo.Size = new System.Drawing.Size(93, 28);
            this.pbGo.TabIndex = 0;
            this.pbGo.Text = "Go";
            this.pbGo.UseVisualStyleBackColor = true;
            this.pbGo.Click += new System.EventHandler(this.pbGo_Click);
            // 
            // pbCancel
            // 
            this.pbCancel.Enabled = false;
            this.pbCancel.Location = new System.Drawing.Point(102, 3);
            this.pbCancel.Name = "pbCancel";
            this.pbCancel.Size = new System.Drawing.Size(83, 28);
            this.pbCancel.TabIndex = 1;
            this.pbCancel.Text = "Cancel";
            this.pbCancel.UseVisualStyleBackColor = true;
            this.pbCancel.Click += new System.EventHandler(this.pbCancel_Click);
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(0, 0);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(345, 187);
            this.tbLog.TabIndex = 3;
            this.tbLog.WordWrap = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output dir:";
            // 
            // tbOutPath
            // 
            this.tbOutPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutPath.Location = new System.Drawing.Point(65, 31);
            this.tbOutPath.Name = "tbOutPath";
            this.tbOutPath.Size = new System.Drawing.Size(243, 20);
            this.tbOutPath.TabIndex = 4;
            this.tbOutPath.TextChanged += new System.EventHandler(this.tbOutPath_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tbOutPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbFirstFile, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbFindFile, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbFindDir, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(345, 55);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // pbFindDir
            // 
            this.pbFindDir.Location = new System.Drawing.Point(314, 30);
            this.pbFindDir.Name = "pbFindDir";
            this.pbFindDir.Size = new System.Drawing.Size(28, 22);
            this.pbFindDir.TabIndex = 5;
            this.pbFindDir.Text = "...";
            this.pbFindDir.UseVisualStyleBackColor = true;
            this.pbFindDir.Click += new System.EventHandler(this.pbFindDir_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 187);
            this.panel1.TabIndex = 5;
            // 
            // wfRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 273);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "wfRestore";
            this.Text = "Restore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wfRestore_FormClosing);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFirstFile;
        private System.Windows.Forms.Button pbFindFile;
        private System.Windows.Forms.Button pbGo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbOutPath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button pbFindDir;
        private System.Windows.Forms.Button pbCancel;
    }
}