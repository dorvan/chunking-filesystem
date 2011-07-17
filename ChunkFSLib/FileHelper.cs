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
 *  static convenience methods to find lists of files and count their size
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChunkFS
{
    public class FileHelper
    {
        public static List<string> findFileNames(string fn)
        {
            List<string> files = new List<string>();
            if ((fn.Contains("*") || fn.Contains("?")) && fn.IndexOf(Path.DirectorySeparatorChar) > -1)
            {
                string wc = fn.Substring(fn.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                string dir = fn.Substring(0, fn.LastIndexOf(Path.DirectorySeparatorChar));
                string[] filenames = Directory.GetFiles(dir, wc);
                foreach (string filename in filenames)
                {
                    files.Add(filename);
                }
            }
            else if (File.Exists(fn))
            {
                files.Add(fn);
            }
            else if (Directory.Exists(fn))
            {
                string wc = "*";
                string[] filenames = Directory.GetFiles(fn, wc);
                foreach (string filename in filenames)
                {
                    files.Add(filename);
                }
            }
            else
            {
                throw new ArgumentException("file not found:" + fn);
            }
            return files;
        }

        public static long findTotalLength(string filespec)
        {
            long fileSize = 0;
            foreach (string fn in findFileNames(filespec))
            {
                if (File.Exists(fn)) fileSize += (new FileInfo(fn).Length);
            }
            return fileSize;
        }

        public static bool isAnyData(List<string> filespecs)
        {
            foreach (string filespec in filespecs)
            {
                try
                {
                    if (filespec == null) continue;
                    foreach (string fn in findFileNames(filespec))
                    {
                        if (File.Exists(fn)&& (new FileInfo(fn).Length) > 0 ) return true;
                    }
                }
                catch (ArgumentException) { }
            }
            return false;
        }
    }
}
