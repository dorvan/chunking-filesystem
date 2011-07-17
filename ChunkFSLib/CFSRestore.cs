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
// 07/15/2011 	T Shanley		West Leitrim Software	Original contribution - entire file.
//

/**
 *  class with Restore logic, able to decode either format of chunk file name, insist on starting at chunk 1, 
 *  ask for chunks when not available. note: the logger is what does the prompting when needed.
 */ 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ChunkFS
{
    public delegate void CFSRestoreCompletionHandler(object o, bool success);

    public class CFSRestore
    {
        public event CFSRestoreCompletionHandler CFSRestoreCompletion;

        public bool CancelRequested { get; set; }

        private string infile, outpath;

        public CFSRestore() { }

        public CFSRestore(string infile, string outpath)
        {
            this.infile = infile;
            this.outpath = outpath;
        }

        public void Restore()
        {
            Restore(this.infile, this.outpath);
        }

        public void Restore(string infile, string outpath)
        {
            CancelRequested = false;
            bool success = false;
            FileStream opf = null;
            FileStream inf = null;
            DateTime fileModTime = new DateTime(); ;
            try
            {
                FileInfo fi = new FileInfo(infile);
                if (!fi.Exists)
                {
                    Logger.getLogger().log("input file does not exist");
                    return;
                }

                DirectoryInfo di = new DirectoryInfo(outpath);
                if (!di.Exists)
                {
                    Logger.getLogger().log("output directory does not exist");
                    return;
                }

                fileModTime = fi.LastWriteTime;

                AntiChunk c = new AntiChunk(fi);
                if (c == null || !c.IsValid)
                {
                    Logger.getLogger().log("invalid input file");
                    return;
                }
                string ofn = di.FullName;
                if (!ofn.EndsWith("\\")) ofn += "\\";
                ofn += c.LogicalName;

                byte[] buffer = new byte[1024 * 64];
                opf = new FileStream(ofn, FileMode.Create, FileAccess.Write);
                Logger.getLogger().log("total matching files needed: " + c.of);
                if (c.n != 1)
                {
                    Logger.getLogger().log("restore must start from file #1, not file #" + c.n);
                    c = c.makeChunk(1);
                }
                do
                {
                    while (!File.Exists(c.ActualPath))
                    {
                        if (!Logger.getLogger().logAndWait("press enter when the file is available: " + c.ActualPath))
                        {
                            break;
                        }
                    }

                    if (File.Exists(c.ActualPath))
                    {
                        inf = new FileStream(c.ActualPath, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length * 2, FileOptions.SequentialScan);

                        Logger.getLogger().log("processing input file: " + c.ActualPath + " (" + inf.Length + " bytes)");
                             
                        while (inf.Position < inf.Length)
                        {
                            int readbytes = buffer.Length;
                            if (inf.Position + readbytes > inf.Length) readbytes = (int)(inf.Length - inf.Position);
                            readbytes = inf.Read(buffer, 0, readbytes);
                            opf.Write(buffer, 0, readbytes);
                            if (CancelRequested) throw new OperationCanceledException();
                        }

                        inf.Close();
                        inf = null;
                        c = c.next();
                        success = true; // so far at least
                    }
                    else
                    {
                        c = null;
                        Logger.getLogger().log("Giving up");
                        success = false; 
                    }
                } while (c != null);

            }
            catch (Exception ex)
            {
                Logger.getLogger().log("failed:" + ex.Message + ex.StackTrace);
                success = false;
            }
            finally
            {
                try
                {
                    if (inf != null) inf.Close();
                    if (opf != null)
                    {
                        String fp = opf.Name;
                        opf.Close();
                        if (fileModTime.Ticks>0) new FileInfo(fp).LastWriteTime = fileModTime;
                    }
                }
                catch (Exception ex) 
                {
                    Logger.getLogger().log("exception on close after apparent success:" + ex.Message + ex.StackTrace);
                }
                CFSRestoreCompletionHandler handler = this.CFSRestoreCompletion;
                if (null != handler) { handler(this, success); }
            }
        }
    }
}
