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
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using ChunkFS;

namespace ChunkFSgui
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
            string product = AssemblyHelper.AssemblyProduct(Assembly.GetExecutingAssembly());
            this.Text = String.Format("About {0}", product);
            this.labelProductName.Text = product;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyHelper.AssemblyVersion(Assembly.GetExecutingAssembly()));
            this.labelCopyright.Text = AssemblyHelper.AssemblyCopyright(Assembly.GetExecutingAssembly());
            this.labelCompanyName.Text = AssemblyHelper.AssemblyCompany(Assembly.GetExecutingAssembly());
            ResourceManager rm = new ResourceManager("ChunkFSgui.StringResources", Assembly.GetExecutingAssembly());
            this.textBoxDescription.Text = rm.GetString("Notice") + rm.GetString("Included"); //AssemblyDescription;
        }

    }
}
