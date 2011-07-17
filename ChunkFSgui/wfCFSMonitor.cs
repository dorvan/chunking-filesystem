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
using System.Windows.Forms;
using ChunkFS;

namespace ChunkFSgui
{
    public partial class wfCFSMonitor : Form
    {
        private CFSDriveConfig theDrive;
        private Timer myTimer;
        private StatSource myStats;

        #region Construction and Initialization

        protected wfCFSMonitor()
        {
            InitializeComponent();
        }

        public wfCFSMonitor(CFSDriveConfig drive)
        {
            InitializeComponent();
            theDrive = drive;
            myStats = new StatSource(new CFSStats());
            this.statBindingSource.DataSource = myStats;
            this.lbDrive.Text = "[" + drive.Name + "] on "+ drive.MountPoint.ToUpper() ;
            run();
        }

        #endregion

        #region Button Clicks

        private void pbReset_Click(object sender, EventArgs e)
        {
            if (myTimer == null || !myTimer.Enabled) return;
            myTimer.Stop();
            theDrive.RunningDriver.Stats.reset();
            myStats.reset(theDrive.RunningDriver.Stats.snapshot());
            this.statBindingSource.ResetBindings(false);
            myTimer.Start();
            //stop();
            //run();
        }

        private void pbStartStop_Click(object sender, EventArgs e)
        {
            if (this.pbStartStop.Text.Equals("Stop")) stop();
            else run();
        }

        #endregion

        #region GUI Events and State Management

        void myTimer_Tick(object sender, EventArgs e)
        {
            update();
        }
        private void fMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop();
        }

        #endregion

        #region Logging and Misc

        public void run()
        {
            if (theDrive == null || !theDrive.Mounted) return;
            if (myTimer != null) myTimer.Stop();
            theDrive.RunningDriver.Stats.reset();
            theDrive.RunningDriver.KeepStats = true;
            this.pbStartStop.Text = "Stop";
            myTimer=new Timer(); 
            myTimer.Tick += new EventHandler(myTimer_Tick);
            myTimer.Interval = 1000;
            myTimer.Start();
        }

        public void stop()
        {
            if (myTimer != null) myTimer.Stop();
            myTimer = null;
            this.pbStartStop.Text = "Start";
            try
            {
                theDrive.RunningDriver.KeepStats = false;
            }
            catch { }
        }

        private void update()
        {
            if (theDrive.RunningDriver == null)
            {
                stop();
                return;
            }
            myStats.update(theDrive.RunningDriver.Stats.snapshot());
            this.statBindingSource.ResetBindings(false);
        }

        #endregion

    }
}
