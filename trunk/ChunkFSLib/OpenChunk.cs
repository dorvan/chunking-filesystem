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
 *  class to relate a chunk to an open file stream.  mapped by dokan context id in the driver, this lets us
 *  keep files open between calls in a thread-safe / handle-safe way.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ChunkFS
{
    public class OpenChunk
    {
        public Chunk chunk;
        public FileStream file;
        public Boolean isDirectory = false;
        public string name = null;
        public List<Chunk> contents = null;

        public OpenChunk(Chunk c, FileStream f)
        {
            this.chunk = c;
            this.file = f;
        }

        public OpenChunk(string name, List<Chunk> contents)
        {
            isDirectory = true;
            this.name = name;
            this.contents = contents;
        }
    }
}
