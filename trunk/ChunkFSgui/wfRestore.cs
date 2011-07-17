// Copyright (c) 2011 ChunkFS Contributors
//
// The contents of this file are subject to the terms of the
// Common Development and Distribution License, Version 1.0 only
// (the "License").  You may not use this file except in compliance
// with the License.
//
// A copy of the License is included in the file license.txt 
// A copy of the License can also be obtained from the Open Source 
// Initiative at http://www.opensource.org/licenses/cddl1.txt
// 
// See the License for the specific language governing permissions
// and limitations under the License.
//
// When distributing Covered Code, include this notice in each
// source code file and include the referenced License file, and  
// if applicable, add a description of any modifications to the 
// Summary of Contributions.
//
//
// Summary of Contributions
//
// Date		 	Author			Organization			Description of contribution
// ========== 	===============	=======================	=====================================================================
// 07/15/2011 	Tom Shanley		West Leitrim Software	Original contribution - entire file.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ChunkFS;

namespace ChunkFSgui
{
    public partial class wfRestore : Form
    {
        private GuiLogger myLogger;
        private LogHandler yourLogger = null;
        private CFSRestore restorer;
        private Thread restoThread;
        private bool busy = false;
        private IConcernedParent parnt;

        public delegate void LogUpdateDelegate(Object o, String txt);
        public delegate void GuiStateDelegate();

        #region Construction and Initialization

        private wfRestore()
        {
            InitializeComponent();
        }

        public wfRestore(IConcernedParent rent)
        {
            InitializeComponent();
            parnt = rent;
        }

        #endregion

        #region Button Clicks

        private void pbFindFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            DialogResult dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                this.tbFirstFile.Text=ofd.FileName;
            }
        }

        private void pbFindDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                this.tbOutPath.Text=fbd.SelectedPath;
            }
        }

        private void pbGo_Click(object sender, EventArgs e)
        {
            myLogger = new GuiLogger(false);
            myLogger.CFSLog += new CFSLogHandler(GuiLogger_CFSLog);
            this.tbLog.Text = "starting to restore from " + tbFirstFile.Text + " to " + tbOutPath.Text;
            yourLogger = Logger.setLogger(myLogger);
            restorer = new CFSRestore(tbFirstFile.Text, tbOutPath.Text);
            restorer.CFSRestoreCompletion += new CFSRestoreCompletionHandler(CFSRestore_CFSRestoreCompletion);
            restoThread = new Thread(new ThreadStart(restorer.Restore));
            busy = true;
            restoThread.Start();
            Thread.Sleep(500);
            manageGuiState();
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            if (this.restorer != null) this.restorer.CancelRequested = true;
        }

        #endregion

        #region GUI Events and State Management

        private void tbFirstFile_TextChanged(object sender, EventArgs e)
        {
            manageGuiState();
        }

        private void tbOutPath_TextChanged(object sender, EventArgs e)
        {
            manageGuiState();
        }

        private void manageGuiState()
        {
            if (this.pbGo.InvokeRequired)
            {
                GuiStateDelegate d = new GuiStateDelegate(manageGuiState);
                this.Invoke(d, new object[] {  });
            }
            if (busy)
            {
                pbGo.Enabled = false;
                pbCancel.Enabled = true;
                return;
            }
            pbCancel.Enabled = false;
            try
            {
                pbGo.Enabled = this.tbFirstFile.Text.Length > 0 && System.IO.File.Exists(this.tbFirstFile.Text)
                    && this.tbOutPath.Text.Length > 0 && System.IO.Directory.Exists(this.tbOutPath.Text);
                    //&& (this.restoThread == null || !this.restoThread.IsAlive);
            }
            catch (Exception)
            {
                pbGo.Enabled = false;
            }
        }

        private void wfRestore_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (restoThread != null)
            {
                if (busy || restoThread.IsAlive)
                {
                    DialogResult dr = MessageBox.Show("This will abort the restore", "Confirm action", MessageBoxButtons.OKCancel);
                    if (dr != DialogResult.OK)
                    {
                        e.Cancel = true;
                        return;
                    }
                    restoThread.Abort();
                }
            }
            if (myLogger != null && Logger.getLogger() == myLogger)
                Logger.setLogger(yourLogger);
            if (parnt != null) parnt.notifyClosing(this);
        }
 
        #endregion

        #region Logging and Misc
        void CFSRestore_CFSRestoreCompletion(object o, bool success)
        {
            if (o == restorer)
            {
                if (success) log("*** restore completed ***");
                else log("*** restore terminated ***");
                busy = false;
                manageGuiState();
            }
        }

        void GuiLogger_CFSLog(object o, string txt)
        {
            if (this.tbLog.InvokeRequired)
            {
                LogUpdateDelegate d = new LogUpdateDelegate(GuiLogger_CFSLog);
                this.Invoke(d, new object[] { o, txt });
            }
            else
            {
                log(txt);
            }
        }

        private void log(string txt)
        {
            this.tbLog.Text += ("\r\n" + DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss ") + txt);
        }

        #endregion

    }
}
