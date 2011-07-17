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
 *  class to seek smart chop points in transport stream files.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace ChunkFS
{
    /**
     * a helper for finding good chop points in ts (transport stream) files
     */
    public class TransportStream : IFileChunkHelper
    {

	    static byte[] SIGNATURE = { 0x47, 0x40, 0 };
        const int BLOCKSIZE = 188;
	    const int BLOCKSTOCHECK = 4;
	    const int IOSIZE = 65536;
	    const int HUNTLIMIT = 1000000;


        #region IFileChunkHelper Members

        public bool Supports(string fileExt)
        {
            return fileExt.EndsWith(".ts") || fileExt.EndsWith(".tp") || fileExt.EndsWith(".m2ts") || fileExt.EndsWith(".m2t");
        }

        private bool mCanChunk = false;
        private string pathAsEvaluated = null;

        public bool CanMakeGoodChunks {get; set;}

        public void EvaluateThisFile(string mpath)
        {
            CanMakeGoodChunks = CheckMyStream(mpath);
            pathAsEvaluated = mpath;
        }

        public long LocateChunkEndPoint(string path, long proposed)
        {
            if (pathAsEvaluated == null || !pathAsEvaluated.Equals(path, StringComparison.CurrentCultureIgnoreCase))
            {
                CanMakeGoodChunks=CheckMyStream(path);
                pathAsEvaluated = path;
            }
            if (!CanMakeGoodChunks) return proposed;
            return FindCutPoint(path, proposed);
        }

        public long LocateChunkEndPoint(BinaryReader br, long proposed)
        {
            return FindCutPoint(br, proposed, -1, HUNTLIMIT);
        }

        public long LocateChunkEndPoint(long proposed)
        {
            return LocateChunkEndPoint(pathAsEvaluated, proposed);
        }

        #endregion

	    public static bool CheckMyStream(string infile)
	    {
		    BinaryReader br = null;
		    byte b = 0;
		    bool rv = true;
		    // assume it is good 
		    try {
			    br = new BinaryReader(new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read, IOSIZE));
			    for (int i = 0; i <= BLOCKSTOCHECK; i++) {
				    br.BaseStream.Seek(i * BLOCKSIZE, SeekOrigin.Begin);
				    b = br.ReadByte();
				    if (b != SIGNATURE[0]) {
					    rv = false;
					    break; 
				    }
			    }
		    } finally {
			    if (br != null)
				    br.Close();
		    }
		    return rv;
	    }

        public static long FindCutPoint(String fn, long proposed)
        {
		    BinaryReader br = null;
		    long rv = proposed;
		    try {
			    br = new BinaryReader(new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.Read, IOSIZE));
                rv = FindCutPoint(br, proposed, -1, HUNTLIMIT);
            } catch {
		    } finally {
			    if (br != null)
				    br.Close();
		    }
            if (rv == proposed) rv -= rv % BLOCKSIZE;
		    return rv;
        }

	    public static long FindCutPoint(BinaryReader br, long proposed, int direction, int maxseek)
	    {
		    long rv = proposed;
		    long repos = br.BaseStream.Position;
		    int skip = BLOCKSIZE * direction;
		    long limitPlus = br.BaseStream.Length - 1;
		    long limitMinus = 0;
		    if (direction == 1) {
			    limitPlus = proposed + maxseek;
		    } else if (direction == -1) {
			    limitMinus = proposed - maxseek;
		    }
		    byte[] sig = SIGNATURE;
		    int sigl = sig.Length;
		    int siglm = sigl * -1;
		    byte[] sigMaybe = null;
		    int i = 0;
		    bool ok = false;

		    try {
			    br.BaseStream.Seek(proposed - skip, SeekOrigin.Begin);
			    bool success = false;
			    while (!success && br.BaseStream.Position < limitPlus && br.BaseStream.Position > limitMinus) {
				    br.BaseStream.Seek(skip, SeekOrigin.Current);
				    sigMaybe = br.ReadBytes(sigl);
				    br.BaseStream.Seek(siglm, SeekOrigin.Current);
				    ok = false;
				    for (i = 0; i <= sigl - 1; i++) {
					    ok = sig[i] == sigMaybe[i];
					    if (!ok)
						    break; 
				    }
				    success = ok;
			    }
			    if (success) {
				    rv = br.BaseStream.Position;
			    }
		    } finally {
			    br.BaseStream.Seek(repos, SeekOrigin.Begin);
		    }

		    return rv;
	    }
    }
}
