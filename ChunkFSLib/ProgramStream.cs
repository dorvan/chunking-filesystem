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
 *  class to seek smart chop points in mpg files.
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
     * a helper for finding good chop points in mpg (mpeg-2 mixed program streams) files
     */ 
    public class ProgramStream : IFileChunkHelper
    {

	    public class Packet
	    {

		    private byte[] bs;

		    private int streamIx;
		    public byte[] Data {
			    get { return this.bs; }
		    }


		    public Packet()
		    {
		    }
		    public Packet(byte[] bytes)
		    {
			    init(bytes);
		    }

		    public void init(byte[] bytes)
		    {
			    bs = bytes;
			    try {
				    streamIx = 23 + bs[22];
			    } catch (Exception) {
				    streamIx = 0;
			    }
		    }
            public bool IsVideoStartPacket()
            {
                return IsVideoStartPacket(0xe0);
            }

		    public bool IsVideoStartPacket(byte vidStreamId)
		    {
			    bool rv = false;
			    try {
                    rv = this.StreamId == vidStreamId && this.WordOne() == 0x1b3;
			    } catch {
			    }
			    return rv;
		    }

		    public bool IsSystemPacket()
		    {
			    bool rv = false;
			    try {
				    rv = this.StreamId == 0xbb;
			    } catch {
			    }
			    return rv;
		    }

		    public bool IsResync()
		    {
			    bool rv = false;
			    try {
				    rv = this.bs[0x21] == 0 && this.bs[0x22] == 1 && this.bs[0x23] == 0xb && this.bs[0x24] == 0x77;
			    } catch {
			    }
			    return rv;
		    }

		    public Int32 WordOne()
		    {
			    Int32 rv = 0;
			    try {
				    for (int i = streamIx; i <= streamIx + 3; i++) {
					    rv = rv << 8;
					    rv = rv | bs[i];
				    }
			    } catch {
			    }
			    return rv;
		    }

		    public byte StreamId {
			    get {
				    try {
					    return bs[17];
				    } catch (Exception) {
					    return 0;
				    }
			    }
			    set {
				    try {
					    bs[17] = value;
				    } catch {
				    }
			    }
		    }

		    public byte Ac3SubstreamId {
			    get {
                    if (this.StreamId == ProgramStream.SID_PRIVATE_1)
                    {
					    byte rv = bs[31];
                        if ((rv & ProgramStream.SID_MASK) == ProgramStream.AC3_STREAM_0)
                        {
						    return rv;
					    } else {
						    return 0;
					    }
				    } else {
					    return 0;
				    }
			    }
		    }


	    }

	    const byte SID_VID_MIN = 0xe0;
	    const byte SID_VID_MAX = 0xef;
	    const byte SID_AUD_MIN = 0xc0;
	    const byte SID_AUD_MID = 0xd0;
	    const byte SID_AUD_MAX = 0xdf;
	    const byte SID_PRIVATE_1 = 0xbd;
	    const byte SID_PRIVATE_2 = 0xbf;
	    const byte SID_MASK = 0xf0;
	    const byte AC3_STREAM_0 = 0x80;
	    const int SEARCH_PACKETS_LIM = 500;

	    byte vidSid = 0;
	    byte audSid = 0;
	    byte ac3Sid = 0;

	    List<byte> foundStreamids = new System.Collections.Generic.List<byte>();
	    List<byte> foundPrivates = new System.Collections.Generic.List<byte>();

	    Packet sysHdr;

	    string pathAsEvaluated = null;

	    public bool CanMakeGoodChunks {get; set;}

	    public long LocateChunkEndPoint(long proposed)
	    {
            if (!CanMakeGoodChunks) return proposed;
            return this.LocateChunkEndPoint(pathAsEvaluated, proposed);
	    }

        public bool Supports(string fileExt)
        {
            return (fileExt.Equals(".mpg", StringComparison.CurrentCultureIgnoreCase)
                || fileExt.Equals(".mpeg", StringComparison.CurrentCultureIgnoreCase)
                );
        }

	    public long LocateChunkEndPoint(string path, long proposed)
	    {
            if (pathAsEvaluated == null || !pathAsEvaluated.Equals(path, StringComparison.CurrentCultureIgnoreCase))
            {
                EvaluateThisFile(path); // we have to find the vid stream id
            }
            if (!CanMakeGoodChunks) return proposed;
		    BinaryReader br = null;
		    try {
			    br = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));
			    return LocateChunkEndPoint(br, proposed);
		    } catch (Exception) {
		    } finally {
			    if (br != null)
				    br.Close();
		    }
            return proposed;
	    }

	    public long LocateChunkEndPoint(BinaryReader br, long proposed)
	    {
		    Packet aPacket = new Packet();
		    long rv = proposed;
            int packetsTried = 0;
		    try {
			    br.BaseStream.Seek(proposed + 4096, SeekOrigin.Begin);
			    do {
				    br.BaseStream.Seek(-4096, SeekOrigin.Current);
				    aPacket.init(br.ReadBytes(2048));
                } while (!aPacket.IsVideoStartPacket(vidSid) && br.BaseStream.Position > 4096 && ++packetsTried < SEARCH_PACKETS_LIM);
                if (aPacket.IsVideoStartPacket(vidSid))
                {
				    rv = (br.BaseStream.Position - 2048);
			    }
		    } catch {
		    }
		    return rv;
	    }

	    public void EvaluateThisFile(string mpath)
	    {
		    string ifn = mpath;
		    string ifnl = ifn.ToLower();
		    int blockSize = 0;
		    if (ifnl.EndsWith(".mpg") | ifnl.EndsWith(".mpeg")) {
			    blockSize = 2048;
		    } else {
			    throw new Exception("Must be an mpg");
		    }
		    BinaryReader br = null;
		    FileStream fs = null;
		    Packet aPacket = new Packet();

		    foundStreamids = new System.Collections.Generic.List<byte>();
		    foundPrivates = new System.Collections.Generic.List<byte>();

		    try {
			    fs = new FileStream(ifn, FileMode.Open, FileAccess.Read, FileShare.Read);
			    br = new BinaryReader(fs);
			    // check if there is a system header:
                aPacket.init(br.ReadBytes(blockSize));
			    if (!aPacket.IsSystemPacket()) {
				    fs.Seek(0, SeekOrigin.Begin);
			    } else {
				    sysHdr = new Packet(aPacket.Data);
			    }

			    // look for stream ids, and for ac3 substreams in the private stream. 
                // note: ac3 stream info may be used in other application but not used in CFS
			    for (int i = 0; i <= SEARCH_PACKETS_LIM; i++) {
                    aPacket.init(br.ReadBytes(blockSize));
				    byte sid = aPacket.StreamId;
				    if (!foundStreamids.Contains(sid)) {
					    foundStreamids.Add(sid);
					    byte masked = (byte)(sid & SID_MASK);
					    if (masked == SID_VID_MIN) {
						    if (vidSid == 0 || sid == SID_VID_MIN) {
							    vidSid = sid;
						    }
					    } else if (masked == SID_AUD_MIN || masked == SID_AUD_MID) {
						    if (audSid == 0 || sid == SID_AUD_MIN) {
							    audSid = sid;
						    }
					    }
				    }
				    if (sid == SID_PRIVATE_1) {
					    byte ac3 = aPacket.Ac3SubstreamId;
					    if (ac3 >= AC3_STREAM_0) {
						    if (!foundPrivates.Contains(ac3)) {
							    foundPrivates.Add(ac3);
						    }
						    if (ac3Sid == 0 || ac3 == AC3_STREAM_0) {
							    ac3Sid = ac3;
						    }
					    }
				    }
			    }

			    // decide the matter; assume if we have a vid stream our chunk find logic can work
			    CanMakeGoodChunks = (vidSid > 0); // && ac3Sid > 0);
			    pathAsEvaluated = mpath;
		    } catch (Exception ex) {
			    Logger.getLogger().log(ex.ToString() + ex.StackTrace);
		    } finally {
			    if (fs != null) {
				    fs.Close();
			    }
		    }

	    }


    }
}
