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

// NOTE: unit testing is "weak" and is useful mainly to allow debugging piecewise without/before installing


using ChunkFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dokan;
using System.IO;
using System.Collections;
using System.Globalization;
using System;

namespace ChunkFS_TestProject1
{
    
    /// <summary>
    ///This is a test class for ChunkFSTest and is intended
    ///to contain all ChunkFSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ChunkFSTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            CFSDriveConfig config = new CFSDriveConfig("test", args[0], args[1], args, 2);
            target = new ChunkFSDriver(config);
            //bool ok = target.Init(args[1], args[2]);
            bool ok = target.Init();
        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        static string[] args = {"m:\\","100M","c:\\hiberfil.sys" };
        static ChunkFSDriver target;

        /// <summary>
        ///A test for WriteFile
        ///</summary>
        [TestMethod()]
        public void InitTest()
        {
            CFSDriveConfig config = new CFSDriveConfig("test", args[0], args[1], args, 2);
            //bool ok = target.Init(args[1], args[2]);
            ChunkFSDriver target = new ChunkFSDriver(config);
            bool ok = target.Init();
            Assert.IsTrue(ok);
        }

        /// <summary>
        ///A test for WriteFile
        ///</summary>
        [TestMethod()]
        public void WriteFileTest()
        {
            string filename = string.Empty; 
            byte[] buffer = null; 
            uint writtenBytes = 0; 
            uint writtenBytesExpected = 0; 
            long offset = 0; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.WriteFile(filename, buffer, ref writtenBytes, offset, info);
            Assert.AreEqual(writtenBytesExpected, writtenBytes);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Unmount
        ///</summary>
        [TestMethod()]
        public void UnmountTest()
        {
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.Unmount(info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UnlockFile
        ///</summary>
        [TestMethod()]
        public void UnlockFileTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            long offset = 0; 
            long length = 0; 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.UnlockFile(filename, offset, length, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetFileTime
        ///</summary>
        [TestMethod()]
        public void SetFileTimeTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            DateTime ctime = new DateTime(); 
            DateTime atime = new DateTime(); 
            DateTime mtime = new DateTime(); 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.SetFileTime(filename, ctime, atime, mtime, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetFileAttributes
        ///</summary>
        [TestMethod()]
        public void SetFileAttributesTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            FileAttributes attr = new FileAttributes(); 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.SetFileAttributes(filename, attr, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetEndOfFile
        ///</summary>
        [TestMethod()]
        public void SetEndOfFileTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            long length = 0; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.SetEndOfFile(filename, length, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetAllocationSize
        ///</summary>
        [TestMethod()]
        public void SetAllocationSizeTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            long length = 0; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.SetAllocationSize(filename, length, info);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ReadFile
        ///</summary>
        [TestMethod()]
        public void ReadFileTest()
        {
            int kperchunk = 640;
            string infn = TestDataHelper.locateTestText(); //"D:\\testdata\\log.txt";
            string cs = "" + kperchunk + "K";
            CFSDriveConfig config = new CFSDriveConfig("readtest", "x:\\", cs, new string[] {infn}, 0 );
            ChunkFSDriver cfs = new ChunkFSDriver(config);
            //cfs.Init(infn, cs);

            uint readBytes = 64 * 1024;
            uint readBytesExpected = readBytes;
            byte[] buffer = new byte[readBytes]; 

            string filename = infn.Substring(infn.LastIndexOf("\\")+1);
            FileInfo fi = new FileInfo(infn);
            long actualsize = fi.Length;
            string outDir = fi.DirectoryName + "\\out\\";
            int bytesperchunk = kperchunk *1024;
            int chunksExpected = (int)((actualsize + (long)bytesperchunk - 1L) / (long)bytesperchunk);

            long offset = 0; 
            DokanFileInfo info = new DokanFileInfo(0);
            FileInformation finf = new FileInformation();
            int ok;

            for (int i = 0; i < chunksExpected; i++)
            {
                // get file info and note the size
                string uniq = fi.LastWriteTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
                string currfn = filename + "." + uniq + "."  + (i + 1) + "." + chunksExpected;
                ok = cfs.GetFileInformation(currfn, finf, info);
                long currcs = finf.Length;

                // open the file, create an output file
                ok = cfs.CreateFile(currfn, FileAccess.Read, FileShare.Read, FileMode.Open, FileOptions.None, info);
                FileStream ofs = new FileStream(outDir + currfn, FileMode.Create, FileAccess.Write);

                // read all the bytes and write to output
                offset = 0;
                do
                {
                    ok = cfs.ReadFile(currfn, buffer, ref readBytes, offset, info);
                    ofs.Write(buffer, 0, (int)readBytes);
                    offset += readBytes;
                } while (offset + 1 < currcs);

                // close the input and output
                ok = cfs.Cleanup(currfn, info);
                ok = cfs.CloseFile(currfn, info);
                string ofn = ofs.Name;
                ofs.Close();
                new FileInfo(ofn).LastWriteTime = fi.LastWriteTime;
            }
            // now you can compare the files TODO 

            //int expected = 0; 
            //int actual;
            //actual = cfs.ReadFile(filename, buffer, ref readBytes, offset, info);
            //Assert.AreEqual(readBytesExpected, readBytes);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ReadFile
        ///</summary>
        [TestMethod()]
        public void ReadFileTest2()
        {
            bool remember = Logger.UsuallyDebugging;
            Logger.UsuallyDebugging = true;
            Logger.setLogger(null);
            int kperchunk = 640;
            string infn = TestDataHelper.locateTestText(); 
            string mount = "k:\\";
            string csize = "640K";
            string[] files = { mount, csize, infn };
            CFSDriveConfig cfg = new CFSDriveConfig(null, mount, csize, files, 2);
            ChunkFSDriver cfs = new ChunkFSDriver(cfg);
            cfs.Init();
            Assert.IsTrue(cfs.Count() > 0);

            uint readBytes = 64 * 1024;
            uint readBytesExpected = readBytes;
            byte[] buffer = new byte[readBytes];

            string filename = infn.Substring(infn.LastIndexOf("\\") + 1);
            FileInfo fi = new FileInfo(infn);
            long actualsize = fi.Length;
            string outDir = fi.DirectoryName + "\\out\\";
            int bytesperchunk = kperchunk * 1024;
            int chunksExpected = (int)((actualsize + (long)bytesperchunk - 1L) / (long)bytesperchunk);

            long offset = 0;
            DokanFileInfo info = new DokanFileInfo(0);
            FileInformation finf = new FileInformation();
            int ok;

            for (int i = 0; i < chunksExpected; i++)
            {
                // get file info and note the size
                string uniq = fi.LastWriteTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
                string currfn = filename + "." + uniq + "." + (i + 1) + "." + chunksExpected;
                ok = cfs.GetFileInformation(currfn, finf, info);
                long currcs = finf.Length;

                // open the file, create an output file
                ok = cfs.CreateFile(currfn, FileAccess.Read, FileShare.Read, FileMode.Open, FileOptions.None, info);
                FileStream ofs = new FileStream(outDir + currfn, FileMode.Create, FileAccess.Write);

                // read all the bytes and write to output
                offset = 0;
                do
                {
                    ok = cfs.ReadFile(currfn, buffer, ref readBytes, offset, info);
                    ofs.Write(buffer, 0, (int)readBytes);
                    offset += readBytes;
                } while (offset + 1 < currcs);

                // close the input and output
                ok = cfs.Cleanup(currfn, info);
                ok = cfs.CloseFile(currfn, info);
                string ofn = ofs.Name;
                ofs.Close();
                new FileInfo(ofn).LastWriteTime = finf.LastWriteTime;
            }
            // now you can compare the files TODO 

            //int expected = 0; 
            //int actual;
            //actual = cfs.ReadFile(filename, buffer, ref readBytes, offset, info);
            //Assert.AreEqual(readBytesExpected, readBytes);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
            Logger.UsuallyDebugging = remember;
            Logger.setLogger(null);
        }


        /// <summary>
        ///A test for OpenDirectory
        ///</summary>
        [TestMethod()]
        public void OpenDirectoryTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = "somewhere"; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.OpenDirectory(filename, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MoveFile
        ///</summary>
        [TestMethod()]
        public void MoveFileTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            string newname = string.Empty; 
            bool replace = false; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.MoveFile(filename, newname, replace, info);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LockFile
        ///</summary>
        [TestMethod()]
        public void LockFileTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            long offset = 0; 
            long length = 0; 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.LockFile(filename, offset, length, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetFileInformation
        ///</summary>
        [TestMethod()]
        public void GetFileInformationTest()
        {
            // this needs to be a lot smarter and it doesnt seem worth the effort
            ////ChunkFSDriver target = new ChunkFSDriver(); 
            //string filename = args[2]+".1"; 
            //FileInformation fileinfo = new FileInformation(); 
            //DokanFileInfo info = null; 
            //int expected = 0; 
            //int actual;
            //actual = target.GetFileInformation(filename, fileinfo, info);
            //Assert.AreEqual(expected, actual);
            //Assert.IsTrue(fileinfo.Attributes == System.IO.FileAttributes.ReadOnly);
            //Assert.IsTrue(fileinfo.Length == 1024*1024*100);
        }

        /// <summary>
        ///A test for GetDiskFreeSpace
        ///</summary>
        [TestMethod()]
        public void GetDiskFreeSpaceTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            ulong freeBytesAvailable = 0; 
            ulong freeBytesAvailableExpected = 0; 
            ulong totalBytes = 0; 
            ulong totalBytesExpected = 536399872; 
            ulong totalFreeBytes = 0; 
            ulong totalFreeBytesExpected = 0; 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.GetDiskFreeSpace(ref freeBytesAvailable, ref totalBytes, ref totalFreeBytes, info);
            Assert.AreEqual(freeBytesAvailableExpected, freeBytesAvailable);
            Assert.AreEqual(totalBytesExpected, totalBytes);
            Assert.AreEqual(totalFreeBytesExpected, totalFreeBytes);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FlushFileBuffers
        ///</summary>
        [TestMethod()]
        public void FlushFileBuffersTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.FlushFileBuffers(filename, info);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FindFiles
        ///</summary>
        [TestMethod()]
        public void FindFilesTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = "\\"; 
            ArrayList files = new ArrayList(); 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.FindFiles(filename, files, info);
            Assert.AreEqual(expected, actual);
            int expectedCount = 6;
            Assert.AreEqual(expectedCount, files.Count);
        }

        /// <summary>
        ///A test for DeleteFile
        ///</summary>
        [TestMethod()]
        public void DeleteFileTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.DeleteFile(filename, info);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DeleteDirectory
        ///</summary>
        [TestMethod()]
        public void DeleteDirectoryTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.DeleteDirectory(filename, info);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateFile
        ///</summary>
        [TestMethod()]
        public void CreateFileTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            FileAccess access = new FileAccess(); 
            FileShare share = new FileShare(); 
            FileMode mode = new FileMode(); 
            FileOptions options = new FileOptions(); 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.CreateFile(filename, access, share, mode, options, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CreateDirectory
        ///</summary>
        [TestMethod()]
        public void CreateDirectoryTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            DokanFileInfo info = null; 
            int expected = -1; 
            int actual;
            actual = target.CreateDirectory(filename, info);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CloseFile
        ///</summary>
        [TestMethod()]
        public void CloseFileTest()
        {
            //ChunkFSDriver target = new ChunkFSDriver(); 
            string filename = string.Empty; 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.CloseFile(filename, info);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Cleanup
        ///</summary>
        [TestMethod()]
        public void CleanupTest()
        {
            string filename = string.Empty; 
            DokanFileInfo info = null; 
            int expected = 0; 
            int actual;
            actual = target.Cleanup(filename, info);
            Assert.AreEqual(expected, actual);
        }

    }
}
