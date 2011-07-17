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
using System.IO;
using System.Text;
using ChunkFS;

/**
 * 
 * we don't use a full-scale ViewModel in this WinForm app, but there are a few bits 
 * that either require or benefit greatly from adapters at that logical level.  those 
 * adaptations are implemented in the classes in this file.
 * 
 */

namespace ChunkFSgui
{
    /*
     * DriveConfigBindingList 
     *  
     * Makes the list of drive configs available for easy binding to columns in a WF GridView
     * 
     */
    class DriveConfigBindingList : BindingList<CFSDriveConfig>  
    {
        public void AddAll(List<CFSDriveConfig> drives)
        {
            foreach (CFSDriveConfig drive in drives) Add(drive);
        }

        public void Apply(List<CFSDriveConfig> drives)
        {
            drives.Clear();
            foreach (CFSDriveConfig drive in this) drives.Add(drive);
        }
    }

    /*
     * FileChoice 
     *  
     * Holds a string that (is supposed to) represent a file, directory, or file
     * match string, for convenience in the gui.
     * 
     */
    class FileChoice
    {
        private const Double mega_byte = 1024 * 1024;

        private string fileName;
        public string FileName {
            get 
            { 
                return fileName; 
            }
            set 
            {
                fileName = value;
                calcFileSize();
            }
        }

        private long fileSize;
        public long FileSize
        {
            get
            {
                return fileSize;
            }
        }

        public string FileSizeM
        {
            get
            {
                double megs = ((double)FileSize / mega_byte);
                if (megs < 1024) return megs.ToString("#.00M");
                megs /= 1024;
                return megs.ToString("#.00G");
            }
        }

        private void calcFileSize()
        { //&& File.Exists(FileName))
            if (FileName != null)            
            {
                try
                {
                    fileSize = FileHelper.findTotalLength(FileName);
                }
                catch (ArgumentException)
                {
                    fileSize = 0;
                }
            }
        }
        public FileChoice() { }
        public FileChoice(string fn) { this.FileName = fn; }
    }

    /*
     * FileChoiceBindingList 
     *  
     * Makes a list of FileChoice available for easy binding to columns in a WF GridView. 
     * Supports the manipulations wanted in the gui e.g. up, down.
     * 
     */
    class FileChoiceBindingList : BindingList<FileChoice>
    {
        CFSDriveConfig currentOwner = null;
        public void SwitchTo(CFSDriveConfig drive)
        {
            if (drive == currentOwner) return;
            if (currentOwner != null) Apply(currentOwner);
            this.Clear();
            currentOwner = drive;
            if (drive.Files != null) 
                foreach (string fn in drive.Files) Add(new FileChoice(fn));
        }

        public void Apply(CFSDriveConfig drive)
        {
            drive.Files.Clear();
            foreach (FileChoice fc in this) drive.Files.Add(fc.FileName);
        }

        public void Apply()
        {
            if (currentOwner == null) return;
            Apply(currentOwner);
        }

        public void up(int ix)
        {
            if (ix < 1 || ix+1 > this.Count) return;
            FileChoice fc = this[ix];
            this.RemoveAt(ix);
            this.Insert(ix - 1, fc);
            this.ResetBindings();
        }

        public void down(int ix)
        {
            if (ix < 0 || ix+1 >= this.Count) return;
            FileChoice fc = this[ix];
            this.RemoveAt(ix);
            this.Insert(ix + 1, fc);
            this.ResetBindings();
        }
    }

    /*
     * StatSource 
     *  
     * Encapsulates the calculation of interval stats etc. and exposes
     * total, interval, average, and peak values for easy binding to 
     * r/o text boxes on the gui.
     * 
     */
    public class StatSource 
    {
        CFSStats start, latest, prior, peak;

        public StatSource(CFSStats start)
        {
            reset(start);
        }

        public void reset(CFSStats start)
        {
            this.start = start;
            this.latest = start;
            this.prior = start;
            peak = new CFSStats();
        }

        public void update(CFSStats latest) 
        {
            this.prior = this.latest;
            this.latest = latest;
            if (this.IntervalOtherCount > peak.OtherCalls) peak.OtherCalls = this.IntervalOtherCount;
            if (this.IntervalReadCount > peak.ReadCalls) peak.ReadCalls = this.IntervalReadCount;
            if (this.IntervalReadBytes > peak.ReadBytes) peak.ReadBytes = this.IntervalReadBytes;
        }

        public long TotalReadCount
        {
            get
            {
                return latest.ReadCalls;
            }
        }
        public long TotalReadBytes
        {
            get
            {
                return latest.ReadBytes;
            }
        }
        public long TotalOtherCount
        {
            get
            {
                return latest.OtherCalls;
            }
        }
        public long IntervalReadCount
        {
            get
            {
                return latest.ReadCalls - prior.ReadCalls;
            }
        }
        public long IntervalReadBytes
        {
            get
            {
                return latest.ReadBytes - prior.ReadBytes;
            }
        }
        public long IntervalOtherCount
        {
            get
            {
                return latest.OtherCalls - prior.OtherCalls;
            }
        }
        public long AvgReadCount
        {
            get
            {
                if (latest.Starting > start.Starting) return latest.ReadCalls / (long)(latest.Starting.Subtract(start.Starting)).TotalSeconds;
                else return 0;
            }
        }
        public long AvgReadBytes
        {
            get
            {
                if (latest.Starting > start.Starting) return latest.ReadBytes / (long)(latest.Starting.Subtract(start.Starting)).TotalSeconds;
                else return 0;
            }
        }
        public long AvgOtherCount
        {
            get
            {
                if (latest.Starting > start.Starting) return latest.OtherCalls / (long)(latest.Starting.Subtract(start.Starting)).TotalSeconds;
                else return 0;
            }
        }

        public long PeakReadCount
        {
            get
            {
                return peak.ReadCalls;
            }
        }
        public long PeakReadBytes
        {
            get
            {
                return peak.ReadBytes;
            }
        }
        public long PeakOtherCount
        {
            get
            {
                return peak.OtherCalls;
            }
        }
    }

}
