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
 *  extract properties from an assembly, as a convenience and to keep this mess isolated
 */ 

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ChunkFS
{
    public class AssemblyHelper
    {
        public static String About (Assembly ay)
        {
                String pn = AssemblyProduct(ay);
                String ver = String.Format("Version {0}", AssemblyVersion(ay));
                String cr = AssemblyCopyright(ay);
                String co = AssemblyCompany(ay);
                String about = pn + " " + ver + " " + cr + " " + co;
                return about;
        }

        public static String AssemblyTitle (Assembly ay)
        {
                object[] attributes = ay.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(ay.CodeBase);
        }

        public static string AssemblyVersion (Assembly ay)
        {
                return ay.GetName().Version.ToString();
        }

        public static string AssemblyDescription (Assembly ay)
        {
                object[] attributes = ay.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }

        public static string AssemblyProduct (Assembly ay)
        {
                object[] attributes = ay.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
        }

        public static string AssemblyCopyright (Assembly ay)
        {
                object[] attributes = ay.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        public static string AssemblyCompany (Assembly ay)
        {
                object[] attributes = ay.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
}
