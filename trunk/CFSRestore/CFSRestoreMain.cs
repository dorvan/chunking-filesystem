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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChunkFS
{
    class CFSRestoreMain
    {
        static void Main(string[] args)
        {
            Logger.getLogger().log(AssemblyHelper.About(System.Reflection.Assembly.GetExecutingAssembly()));
            if (args.Length != 2)
            {
                Logger.getLogger().log("Usage: CFSRestore <firstfile> <destdir>");
                return;
            }
            CFSRestore resto = new CFSRestore();
            resto.Restore(args[0], args[1]);
        }
    }
}
