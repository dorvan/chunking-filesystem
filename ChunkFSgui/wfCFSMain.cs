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
    public partial class wfCFSMain : Form, IConcernedParent
    {
        private CFSConfig myConfig;
        private DriveConfigBindingList myDrives;
        private FileChoiceBindingList currentFiles;
        private wfRestore resto = null;

        #region Construction and Initialization
        public wfCFSMain()
        {
            InitializeComponent();
            loadConfig();
        }

        private void loadConfig()
        {
            myConfig = CFSConfig.Load(null);
            if (myConfig == null) myConfig = new CFSConfig();
            myDrives = new DriveConfigBindingList();
            myDrives.AddAll(myConfig.Drives);
            this.dgConfigs.DataSource = myDrives;
            currentFiles = new FileChoiceBindingList();
            if (myDrives.Count > 0) currentFiles.SwitchTo(myDrives[0]);
            this.dgFiles.DataSource = currentFiles;
        }

        #endregion

        #region Button Clicks

        private void pbNew_Click(object sender, EventArgs e)
        {
            myDrives.Add(new CFSDriveConfig());
        }

        private void pbMount_Click(object sender, EventArgs e)
        {
            try
            {
                int ix = this.dgConfigs.CurrentCell.RowIndex;
                CFSDriveConfig dc = this.myDrives[ix];
                if (!dc.isValid())
                {
                    MessageBox.Show(this, "Drive config is not valid", "Error", MessageBoxButtons.OK);
                    return;
                }
                ChunkFSDriver theDrive = new ChunkFSDriver(dc);
                if (theDrive.Init())
                {
                    //MessageBox.Show("drive will be mounted now, to unmount hit <CTRL>C");
                    Thread t = new Thread(new ThreadStart(theDrive.exec2));
                    t.Start();
                    dc.Mounted = true;
                    dc.RunningDriver = theDrive;
                    myDrives.ResetBindings();
                    manageGuiState(dc);
                }
                else
                {
                    MessageBox.Show("Init() failed, drive not mounted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message + ex.StackTrace, "Mount Failed", MessageBoxButtons.OK);
            }
            finally
            {
                //manageGuiState();
            }

        }

        private void pbUnmount_Click(object sender, EventArgs e)
        {
            try
            {
                int ix = this.dgConfigs.CurrentCell.RowIndex;
                CFSDriveConfig dc = this.myDrives[ix];
                if (!dc.Mounted || dc.RunningDriver==null )
                {
                    MessageBox.Show(this, "Drive config is not mounted", "Internal Error", MessageBoxButtons.OK);
                    return;
                }
                unMountDrive(dc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK);
            }
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            this.saveMyConfig();
        }

        private void pbLoad_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "Reload configuration?", "Confirm Action", MessageBoxButtons.OKCancel);
            switch (dr)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.OK:
                    loadConfig();
                    break;
            }
        }

        private void pbUp_Click(object sender, EventArgs e)
        {
            this.currentFiles.up(this.dgFiles.SelectedCells[0].RowIndex);
        }

        private void pbDown_Click(object sender, EventArgs e)
        {
            this.currentFiles.down(this.dgFiles.SelectedCells[0].RowIndex);
        }

        private void pbAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            DialogResult dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (string fn in ofd.FileNames)
                {
                    currentFiles.Add(new FileChoice(fn));
                }
                currentFiles.ResetBindings();
            }
        }

        private void pbMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                int ix = this.dgConfigs.CurrentCell.RowIndex;
                CFSDriveConfig dc = this.myDrives[ix];
                if (!dc.Mounted)
                {
                    MessageBox.Show(this, "Drive is not mounted", "Error", MessageBoxButtons.OK);
                    return;
                }
                wfCFSMonitor fmon = new wfCFSMonitor(dc);
                fmon.Show();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(this, ex.Message + ex.StackTrace, "Monitor Failed", MessageBoxButtons.OK);
            }

        }
        
        private void pbRestore_Click(object sender, EventArgs e)
        {
            if (resto != null)
            {
                MessageBox.Show("Only one restore window can be open at a time.");
                return;
            }
            resto = new wfRestore(this);
            resto.Show();
        }

        private void pbAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog(this);
        }
        #endregion

        #region GUI Events and State Management

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            List<CFSDriveConfig> mounted = new List<CFSDriveConfig>();
            foreach (CFSDriveConfig dc in this.myDrives) if (dc.Mounted) mounted.Add(dc);
            if (mounted.Count > 0)
            {
                dr = MessageBox.Show(this, "This will dismount all CFS drives", "Confirm Action on Close", MessageBoxButtons.OKCancel);
                switch (dr)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    case DialogResult.OK:
                        foreach (CFSDriveConfig dc in mounted)
                        {
                            unMountDrive(dc);
                        }
                        break;
                }
            }

            dr =  MessageBox.Show(this, "Save configuration?", "Confirm Action on Close", MessageBoxButtons.YesNoCancel);
            switch (dr)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
                case DialogResult.Yes:
                    saveMyConfig();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void dgConfigs_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < myDrives.Count)
            {
                CFSDriveConfig dc = myDrives[e.RowIndex];
                manageGuiState(dc);

            }
        }

        private void manageGuiState(CFSDriveConfig dc)
        {
            currentFiles.SwitchTo(dc);
            this.pbUnmount.Enabled = dc.Mounted;
            this.pbMount.Enabled = !dc.Mounted && dc.isValid();
            this.pbMonitor.Enabled = dc.Mounted;
        }

        private void dgConfigs_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                manageGuiState(this.myDrives[e.RowIndex]);
            }
            catch (Exception) { }
        }

        private void dgFiles_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                CFSDriveConfig dc = this.myDrives[this.dgConfigs.CurrentCell.RowIndex];
                currentFiles.Apply(dc);
                manageGuiState(dc);
            }
            catch (Exception) { }
        }
 
        #endregion

        #region Logging and Misc
        private void unMountDrive(CFSDriveConfig dc)
        {
            ChunkFSDriver theDrive = dc.RunningDriver;
            int rc = Dokan.DokanNet.DokanUnmount(dc.MountPoint.ToCharArray()[0]);
            dc.RunningDriver = null;
            dc.Mounted = false;
            myDrives.ResetBindings();
            manageGuiState(dc);
        }

        private void saveMyConfig()
        {
            currentFiles.Apply();
            myDrives.Apply(myConfig.Drives);
            this.myConfig.Save(null);
        }

        public void notifyClosing(Form form)
        {
            if (form==resto)
                resto = null;
        }

        #endregion

    }
}
