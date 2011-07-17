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
 *  interface, and implementation of Console and Gui loggers, each able to optionally prompt and wait for a response 
 *  only one can be active at a time, by default that is a console logger, and thus does nothing under the gui by default.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ChunkFS
{
    public interface LogHandler
    {
        void debug(string txt);
        void log(string txt);
        bool logAndWait(string txt);
        bool isConsole();
    }

    public class Logger
    {
        private static LogHandler theLogger = null;
        public static bool UsuallyDebugging = false;

        public static LogHandler getLogger()
        {
            if (theLogger == null) theLogger = new ConsoleLogger(UsuallyDebugging);
            return theLogger;
        }

        public static LogHandler setLogger(LogHandler lh)
        {
            LogHandler rv = theLogger;
            theLogger = lh;
            return rv;
        }

    }
    public class ConsoleLogger : LogHandler
    {
        bool debugging;
        public ConsoleLogger(bool dbg)
        {
            this.debugging = dbg;
        }

        public bool isConsole() { return true; }

        public void debug(string txt)
        {
            if (debugging) log(txt);
        }

        public void log(string txt)
        {
            Console.WriteLine(txt);
        }

        public bool logAndWait(string txt)
        {
            log(txt);
            log("OK to continue?");
            string response = Console.ReadLine();
            return !"N".Equals(response, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    public delegate void CFSLogHandler(object o, String txt);

    public class GuiLogger : LogHandler
    {
        public event CFSLogHandler CFSLog;
        
        bool debugging;
        public GuiLogger(bool dbg)
        {
            this.debugging = dbg;
        }

        public bool isConsole() { return false; }

        public void debug(string txt)
        {
            if (debugging) log(txt);
        }

        public void log(string txt)
        {
            CFSLog(this, txt);
        }

        public bool logAndWait(string txt)
        {
            log(txt + " (response required)");
            DialogResult dr = MessageBox.Show(txt, "(ChunkFS) Response needed", MessageBoxButtons.OKCancel);
            return dr==DialogResult.OK;
        }
    }

}
