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
 *  representations of a chunk, either way
 */ 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using Dokan;

namespace ChunkFS
{
    public class Chunk
    {
        public static string DateFormatString = "yyyyMMddHHmm";
        public string uniq;
        public string ActualPath;
        public string LogicalPath;
        public long BaseOffset;
        public long LogicalLength;
        public int n;
        public int of;

        private DateTime modified;

        // represent the given chunk of the given file
        public Chunk(string apath, long off, long len, int n, int of, bool useExtension)
        {
            this.ActualPath = apath;
            this.BaseOffset = off;
            this.LogicalLength = len;
            this.of = of;
            FileInfo fi = new FileInfo(this.ActualPath);
            modified = fi.LastWriteTime;
            uniq = modified.ToString(DateFormatString, CultureInfo.InvariantCulture);
            if (useExtension)
            {
                string ext = Path.GetExtension(apath);
                string root = Path.GetFileNameWithoutExtension(apath);
                string dir = Path.GetDirectoryName(apath);
                this.LogicalPath = Path.Combine(dir, root) + "." + uniq + "." + n + "." + of + ext;
            }
            else
            {
                this.LogicalPath = apath + "." + uniq + "." + n + "." + of;
            }
        }

        // fill in properties in a dokan FI 
        public void getFileInfo(FileInformation fi)
        {
            fi.Attributes = System.IO.FileAttributes.ReadOnly;
            fi.Length = this.LogicalLength;
            FileInfo realFi = new FileInfo(this.ActualPath);
            fi.LastAccessTime = realFi.LastAccessTime;
            fi.LastWriteTime = realFi.LastWriteTime;
            fi.CreationTime = realFi.CreationTime;
            fi.FileName = getFileName();
            return;
        }

        public string getFileName()
        {
            return Path.GetFileName(this.LogicalPath);
            //return (this.LogicalPath.LastIndexOf("\\") >= 0) ? this.LogicalPath.Substring(this.LogicalPath.LastIndexOf("\\") + 1) : this.LogicalPath;
        }
    }

    public class AntiChunk
    {
        public string ActualPath;
        public long ActualLength;
        public string LogicalName;
        public long BaseOffset;
        public string uniq;
        public int n;
        public int of;
        public bool IsValid = false;
        public bool originalExtension = false;

        protected AntiChunk() { }

        // represent a file that is a chunk
        public AntiChunk(FileInfo fi) 
        {
            try
            {
                ActualLength = fi.Length;
                ActualPath = fi.FullName;
                string fn = fi.Name;
                string ext = "";
                try
                {
                    // assume "use extension" is off
                    originalExtension = false;
                    of = int.Parse(fn.Substring(fn.LastIndexOf(".") + 1));
                    fn = fn.Substring(0, fn.LastIndexOf("."));
                    n = int.Parse(fn.Substring(fn.LastIndexOf(".") + 1));
                    fn = fn.Substring(0, fn.LastIndexOf("."));
                }
                catch (FormatException)
                {
                    // retry assuming "use extension" is on
                    originalExtension = true;
                    fn = fi.Name;
                    ext = Path.GetExtension(fn);
                    fn = Path.GetFileNameWithoutExtension(fn);
                    of = int.Parse(fn.Substring(fn.LastIndexOf(".") + 1));
                    fn = fn.Substring(0, fn.LastIndexOf("."));
                    n = int.Parse(fn.Substring(fn.LastIndexOf(".") + 1));
                    fn = fn.Substring(0, fn.LastIndexOf("."));
                }
                uniq = fn.Substring(fn.LastIndexOf(".") + 1);
                DateTime tester = DateTime.ParseExact(uniq, Chunk.DateFormatString, CultureInfo.InvariantCulture);
                fn = fn.Substring(0, fn.LastIndexOf("."));
                LogicalName = fn+ext;
                BaseOffset = 0;
                IsValid = true;
            }
            catch (Exception)
            {
                IsValid = false;
            }
        }

        // return a (approximate) representation of the chunk following this
        public AntiChunk next()
        {
            if (n >= of) return null;
            int nn = n + 1;
            AntiChunk rv = makeChunk(nn);
            return rv;
        }

        public AntiChunk makeChunk(int nn)
        {
            AntiChunk rv = new AntiChunk();
            // to make the path first we back up to the uniq...
            int ix = ActualPath.LastIndexOf(".");
            ix = ActualPath.LastIndexOf(".", ix - 1);
            if (originalExtension) ix = ActualPath.LastIndexOf(".", ix - 1);
            // ... then we string it back together
            string ext = (originalExtension) ? Path.GetExtension(ActualPath) : "";
            rv.ActualPath = ActualPath.Substring(0, ix) + "." + nn + "." + of + ext;
            //
            rv.ActualLength = ActualLength;
            rv.LogicalName = LogicalName;
            rv.BaseOffset = BaseOffset + ActualLength;
            rv.uniq = uniq;
            rv.n = nn;
            rv.of = of;
            rv.originalExtension = originalExtension;
            return rv;
        }

    }
}
