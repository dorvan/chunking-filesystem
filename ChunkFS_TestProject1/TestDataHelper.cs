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

// NOTE: this class "finds" test data for some tests.  it is quite weakly done and is a temporary lashup to get some mess out of the actual test code

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ChunkFS_TestProject1
{
    public class TestDataHelper
    {
        // a simple dumb lashup to the path of the test file
        public static string locateTestText()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\testdata\\text.txt";
        }

        // a simple dumb lashup to the path of the test file
        public static string locateTestChunk1()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\testdata\\out\\text.txt.201107151206.1.4";
        }
        
        public static string locateTestRestore()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\testdata\\out\\restore\\";
        }


    }
}
