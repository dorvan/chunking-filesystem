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
 *  interface and factory for classes that can hunt for "smart" chunk points
 *  if you add a new implementor of IFileChunkHelper you need to construct one in the FileChunkHelperFactory.InitHelpers()
 */

using System;
using System.Collections.Generic;
namespace ChunkFS
{
    public interface IFileChunkHelper
    {
        bool Supports(string fileExt);
        bool CanMakeGoodChunks { get; set; }
        void EvaluateThisFile(string mpath);
        long LocateChunkEndPoint(string path, long proposed);
        long LocateChunkEndPoint(System.IO.BinaryReader br, long proposed);
        long LocateChunkEndPoint(long proposed);

    }

    public class FileChunkHelperFactory
    {
        static List<IFileChunkHelper> helpers = null;
        public static IFileChunkHelper GetInstance(string path)
        {
            if (helpers == null) InitHelpers();
            IFileChunkHelper rv = null;
            if (path != null)
            {
                string ext = System.IO.Path.GetExtension(path);
                if (ext != null)
                {
                    foreach (IFileChunkHelper ch in helpers)
                    {
                        if (ch.Supports(ext))
                        {
                            rv = ch;
                            break;
                        }
                    }
                }
            }
            return rv;
        }

        private static void InitHelpers()
        {
            helpers = new List<IFileChunkHelper>();
            helpers.Add(new ProgramStream());
            helpers.Add(new TransportStream());
        }
    }

}
