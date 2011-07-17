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
 *  simple stats counting and snapshot for the cfs driver
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace ChunkFS
{
    public class CFSStats
    {
        public long ReadBytes { get; set; }
        public long ReadCalls { get; set; }
        public long OtherCalls { get; set; }
        public long UnsupportedCalls { get; set; }
        public DateTime Starting { get; set; }

        private Object myLock = new Object();

        public CFSStats()
        {
            Starting = DateTime.Now;
        }

        public void reset()
        {
            lock (myLock)
            {
                ReadBytes = 0;
                ReadCalls = 0;
                OtherCalls = 0;
                UnsupportedCalls = 0;
                Starting = DateTime.Now;
            }
        }

        public void count(long bytes)
        {
            lock (myLock)
            {
                ReadCalls++;
                ReadBytes += bytes;
            }
        }

        public void count()
        {
            lock (myLock)
            {
                OtherCalls++;
            }
        }

        public void count(bool unsupp)
        {
            lock (myLock)
            {
                if (unsupp) UnsupportedCalls++;
                else OtherCalls++;
            }
        }

        public CFSStats snapshot()
        {
            lock (myLock)
            {
                CFSStats rv = new CFSStats();
                rv.OtherCalls = this.OtherCalls;
                rv.ReadBytes = this.ReadBytes;
                rv.ReadCalls = this.ReadCalls;
                rv.UnsupportedCalls = this.UnsupportedCalls;
                rv.Starting = DateTime.Now;
                return rv;
            }
        }
    }
}
