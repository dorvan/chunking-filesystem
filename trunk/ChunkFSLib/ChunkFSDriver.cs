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

/**
 *  the actual user-mode filesystem driver
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using Dokan;

namespace ChunkFS
{

    public class ChunkFSDriver : DokanOperations
    {
        public const string ROOT_PATH = "\\";
        public const int BufferSize = 128 * 1024;

        public CFSDriveConfig Config { get; set; }
        public CFSStats Stats { get; set; }
        public Boolean KeepStats { get; set; }

        private Object myLock = new Object();
        private long chunkSize;
        private int chunks = 0;
        private Dictionary<string, Chunk> MyDirectory;
        private Dictionary<string, List<Chunk>> MySubDirs;
        private List<Chunk> orderedChunks;
        private long totalBytes = 0;
        private long fakeFreeBytes = 0; // claim we have free space to dodge the "low disk space" noise 
        private long deductedBytes = 0;
        private int _count = 0;
        private Dictionary<ulong, OpenChunk> openChunks;
        private string currentSubdirName = null;
        private List<Chunk> currentSubdir = null;

        #region Construct and Initialize

        public ChunkFSDriver() 
        {
            init();
        }

        public ChunkFSDriver(CFSDriveConfig config)
        {
            Config = config;
            init();
        }

        // internal
        private void init()
        {
            MyDirectory = new Dictionary<string, Chunk>();
            MySubDirs = new Dictionary<string, List<Chunk>>();
            orderedChunks = new List<Chunk>();
            MySubDirs.Add(ROOT_PATH, orderedChunks);
            openChunks = new Dictionary<ulong, OpenChunk>();
            Stats = new CFSStats();
            KeepStats = false;
        }

        // init based on the config previously set
        public bool Init()
        {
            debug("enter Init() ");
            if (this.Config == null) throw new ArgumentException("Config is null");
            decodeChunksize(this.Config.ChunkSize);
            if (this.Config.RunningChunks && this.Config.ShowSubdirs)
            {
                if (this.currentSubdir == null)
                {
                    newSubdir();
                }
            }
            for (int i = 0; i < Config.Files.Count; i++)
            {
                string fn = Config.Files[i];
                List<string> files = FileHelper.findFileNames(fn);
                foreach (string file in files)
                {
                    debug("adding file " + file);
                    add(file);
                }

            }
            // pretend to have 20% free space
            fakeFreeBytes = totalBytes >> 2; 
            totalBytes += fakeFreeBytes;
            return MyDirectory.Count > 0;
        }

        // common method to convert string chunksize to a long value
        private void decodeChunksize(string chunksize)
        {
            string scale = chunksize.Substring(chunksize.Length - 1);
            double dScale = double.Parse(chunksize.Substring(0, chunksize.Length - 1));
            switch (scale)
            {
                case "B":
                    chunkSize = (long)(1 * dScale);
                    break;
                case "K":
                    chunkSize = (long)(1024 * dScale);
                    break;
                case "M":
                    chunkSize = (long)((1024 * 1024) * dScale);
                    break;
                case "G":
                    chunkSize = (long)((1024 * 1024 * 1024) * dScale);
                    break;
                default:
                    throw new ArgumentException("scale must end in B, K, M, or G");
            }
        }
        #endregion

        #region Chunk Calculations

        // add a file to the configuration, with running chunk sizes if set
        public bool add(string fileName)
        {
            lock (myLock)
            {
                FileInfo fi = new FileInfo(fileName);
                long fsize = fi.Length;
                long deductible = ((totalBytes+deductedBytes) % chunkSize);
                // don't make a too-small first chunk 
                if (deductible > ((1.0-this.Config.MinimumChunk) * chunkSize))
                {
                    deductedBytes += (chunkSize-deductible);
                    deductible = 0;
                }
                //// dont make a too-small last chunk ??????? [the calc is wrong, but so is the concept... doing this leads to possible very-short fills]
                //else if ((fsize + deductible) % chunkSize < (this.Config.MinimumChunk * chunkSize))
                //{
                //    deductedBytes += deductible;
                //    deductible = 0;
                //}
                chunks = (int)((deductible+fsize) / chunkSize);
                if (((chunks * chunkSize)-deductible) < fsize) chunks += 1;
                debug("exposing file " + fileName + " in chunks of " + chunkSize + "(total of " + chunks + ")");
                for (int i = 0; i < chunks; i++)
                {
                    long offset = (i * chunkSize) - deductible;
                    long length;
                    if (i == 0)
                    {
                        offset = 0;
                        length = chunkSize - deductible;
                        if (length > fsize)
                        {
                            length = fsize;
                        }
                    }
                    else if (i + 1 == chunks)
                    {
                        length = (fsize + deductible) - (i * chunkSize);
                    }
                    else
                    {
                        length = chunkSize;
                    }
                    if (Config.UseExtension && Config.SmartChunk && i + 1 < chunks)
                    {
                        IFileChunkHelper ifh = FileChunkHelperFactory.GetInstance(fileName);
                        if (ifh != null)
                        {
                            long suggested = (ifh.LocateChunkEndPoint(fileName, offset+length));
                            if (suggested < (offset+length))
                            {
                                long delta = (offset + length) - suggested;
                                length -= delta;
                                deductible += delta;
                                deductedBytes += delta;
                            }
                        }
                        

                    }
                    Chunk c = new Chunk(fileName, offset, length, i + 1, chunks, Config.UseExtension);
                    MyDirectory.Add(c.getFileName(), c);
                    orderedChunks.Add(c);
                    //if (currentSubdir != null) currentSubdir.Add(c);
                    addToSubdir(c);
                    debug("chunk added: " + c.getFileName());
                    totalBytes += c.LogicalLength;
                }
                return (chunks >= 1);
            }
        }

        /**
         * it seemed to be getting too complicated trying to work "is full" into 
         * the logic of add(), so instead that is taken care of here.  this only
         * happens once, before mount time, so the cost is trivial
         */ 
        private void addToSubdir(Chunk c)
        {
            if (currentSubdir == null) return;
            long total = c.LogicalLength;
            foreach (Chunk already in currentSubdir) total += already.LogicalLength;
            if (total > chunkSize) newSubdir();
            currentSubdir.Add(c);
        }

        private void newSubdir()
        {
            currentSubdirName = (this.MySubDirs.Count).ToString("000");
            currentSubdir = new List<Chunk>();
            MySubDirs.Add(currentSubdirName, currentSubdir);
        }

        #endregion

        #region Execution
        // exec entry point for a generic Thread proc
        public void exec2()
        {
            bool rv = exec();
            return;
        }

        // runs itself.  for a GUI program this needs to be invoked on a thread
        // other than the gui's main one!
        public bool exec()
        {
            Stats = new CFSStats();
            DokanOptions opt = new DokanOptions();
            opt.MountPoint = Config.MountPoint;
            opt.DebugMode = Logger.UsuallyDebugging && Logger.getLogger().isConsole();
            opt.UseStdErr = Logger.UsuallyDebugging && Logger.getLogger().isConsole();
            opt.VolumeLabel = "ChunkFS-"+Config.Name;
            opt.FileSystemName = "CFS";
            int status = DokanNet.DokanMain(opt, this);
            switch (status)
            {
                case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
                    Logger.getLogger().log("Drive letter error");
                    break;
                case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
                    Logger.getLogger().log("Driver install error");
                    break;
                case DokanNet.DOKAN_MOUNT_ERROR:
                    Logger.getLogger().log("Mount error");
                    break;
                case DokanNet.DOKAN_START_ERROR:
                    Logger.getLogger().log("Start error");
                    break;
                case DokanNet.DOKAN_ERROR:
                    Logger.getLogger().log("Unknown error");
                    break;
                case DokanNet.DOKAN_SUCCESS:
                    Logger.getLogger().log("Success");
                    break;
                default:
                    Logger.getLogger().log("Unknown status: " + status);
                    break;
            }
            return true;
        }
        #endregion

        #region DokanOperations Members

        public int Cleanup(string filename, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            ulong ctx = (info == null || info.Context==null) ? 0 : (ulong)info.Context; 
            if (ctx>0) debug("...Cleanup request for " + filename + " ctx is " + ctx);
            if (filename.Equals("\\")) return 0;
            else if (filename.StartsWith("\\autorun", StringComparison.CurrentCultureIgnoreCase)) return 0;
            OpenChunk oc;
            this.openChunks.TryGetValue(ctx, out oc);
            if (oc == null) return 0;
            if (!oc.isDirectory) oc.file.Close();
            openChunks.Remove(ctx);
            return 0;
        }

        public int CloseFile(string filename, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            // per doc this is handled in cleanup, not here in close()
            return 0;
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int CreateFile(string filename, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            debug("...create file request for " + filename + " with access " + access + " with mode " + mode );
            if (filename.Equals(ROOT_PATH)) return 0;
            else if (filename.StartsWith("\\autorun", StringComparison.CurrentCultureIgnoreCase)) return 0;
            else if (filename.StartsWith(ROOT_PATH) && MySubDirs.ContainsKey(filename.Substring(1)))
            {
                List<Chunk> lst;
                MySubDirs.TryGetValue(filename.Substring(1), out lst);
                info.Context = (ulong)this.getNextId();
                openChunks.Add((ulong)info.Context, new OpenChunk(filename.Substring(1), lst));
                return 0;
            }
            string fn = (filename.StartsWith("\\")) ? filename.Substring(filename.LastIndexOf("\\") + 1) : filename;
            if (this.MyDirectory.ContainsKey(fn))
            {
                if (mode != FileMode.Open) return -1;
                if (access != FileAccess.Read) return -1;
                Chunk c;
                MyDirectory.TryGetValue(fn, out c);

                info.Context = (ulong)this.getNextId();
                debug("...create file request ****CTX*** is " + (ulong)info.Context);
                //FileStream fs = File.Open(c.ActualPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                FileStream fs = new FileStream(c.ActualPath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.SequentialScan);
                openChunks.Add((ulong)info.Context, new OpenChunk(c, fs));
                return 0;
            }
            else
            {
                return 0;
            }
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int FindFiles(string filename, System.Collections.ArrayList files, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            ulong ctx = (info == null || info.Context == null) ? 0 : (ulong)info.Context;
            debug("...FindFiles file request ctx is " + ctx + " for " + filename);
            string fn = (ROOT_PATH != filename && filename.StartsWith("\\")) ? filename.Substring(1) : filename;
            if (fn == ROOT_PATH)
            {
                // cheap special handling for subdirs; we are not doing the whole "tree thing" we have at most one level of subdirs
                foreach (string key in MySubDirs.Keys)
                {
                    if (key == ROOT_PATH) continue;
                    FileInformation fi = new FileInformation();
                    fi.Attributes = System.IO.FileAttributes.Directory;
                    fi.LastAccessTime = DateTime.Now;
                    fi.LastWriteTime = DateTime.Now;
                    fi.CreationTime = DateTime.Now;
                    fi.FileName = key;
                    files.Add(fi);
                }
            }
            if (MySubDirs.ContainsKey(fn))
            {
                MySubDirs.TryGetValue(fn, out currentSubdir);
                foreach (Chunk chunk in currentSubdir)
                {
                    FileInformation fi = new FileInformation();
                    chunk.getFileInfo(fi);
                    files.Add(fi);
                }
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            return 0;
        }

        public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            totalBytes = (ulong)this.totalBytes;
            totalFreeBytes = (ulong)this.fakeFreeBytes;
            freeBytesAvailable = (ulong)this.fakeFreeBytes;
            return 0;
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            string fn = Path.GetFileName(filename);
            string pn = Path.GetDirectoryName(filename);
            if (filename == ROOT_PATH) pn = ROOT_PATH;
            debug("...GetFileInformation for " + filename + "(fn=" + fn + ", pn=" + pn + ")");
            if (fn == null && MySubDirs.ContainsKey(pn)) // (pn == ROOT_PATH)
            {
                fileinfo.Attributes = System.IO.FileAttributes.Directory; // | FileAttributes.ReadOnly;
                fileinfo.LastAccessTime = DateTime.Now;
                fileinfo.LastWriteTime = DateTime.Now;
                fileinfo.CreationTime = DateTime.Now;
                info.IsDirectory = true;
                return 0;
            }

            Chunk chunk;
            MyDirectory.TryGetValue(fn, out chunk);
            if (chunk == null)
                return -1;
            else
                chunk.getFileInfo(fileinfo);

            return 0;
        }

        public int LockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            return 0;
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            ulong ctx = (info == null || info.Context == null) ? 0 : (ulong)info.Context;
            debug("...OpenDirectory file request ctx is " + ctx + " for " + filename);
            string fn = (ROOT_PATH != filename && filename.StartsWith("\\")) ? filename.Substring(1) : filename;
            if (MySubDirs.ContainsKey(fn))
            {
                MySubDirs.TryGetValue(fn, out currentSubdir);
                return 0;
            }

            return -1;
        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            ulong ctx = (info == null || info.Context == null) ? 0 : (ulong)info.Context;
            debug("...read file request ctx is " + ctx);
            readBytes = 0;
            OpenChunk oc;
            this.openChunks.TryGetValue(ctx, out oc);
            if (oc == null) return -1;
            if (oc.isDirectory) return 0;
            int toread = buffer.Length;
            if (offset + (long)buffer.Length > oc.chunk.LogicalLength)
                toread = (int)(oc.chunk.LogicalLength - offset); 
            if (toread <= 0) return 0;
            long fromOffset = offset + oc.chunk.BaseOffset;
            if (fromOffset != oc.file.Position) {
                debug("...seek needed for read file to / at " + fromOffset + "/" + oc.file.Position);
                long newOffset = oc.file.Seek(fromOffset,SeekOrigin.Begin);
                if (newOffset != fromOffset) {
                    Console.WriteLine("...seek problem, now at " + newOffset);
                }
            }
            readBytes = (uint)oc.file.Read(buffer, 0, toread);
            if (readBytes != toread)
            {
                Console.WriteLine("...read problem, got / wanted " + readBytes + "/" + toread);
            }
            if (KeepStats) Stats.count(readBytes);
            return 0;
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int SetFileAttributes(string filename, System.IO.FileAttributes attr, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        public int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            return 0;
        }

        public int Unmount(DokanFileInfo info)
        {
            if (KeepStats) Stats.count();
            return 0;
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            if (KeepStats) Stats.count(true);
            return -1;
        }

        #endregion

        #region Logging and Misc Members

        // get an id for a unique Dokan request context.  
        private int getNextId()
        {
            lock (myLock)
            {
                return ++_count; // start at 1, 0 is nada
            }
        }

        // how many files (chunks) have we
        public int Count()
        {
            return MyDirectory.Count;
        }

        private void debug(string txt)
        {
            Logger.getLogger().debug(txt);
        }

        private void log(string txt)
        {
            Logger.getLogger().log(txt);
        }
        #endregion
    }
}
