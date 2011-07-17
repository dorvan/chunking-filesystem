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
 *  classes to represent a drive configuration, and a collection of them that can be serialized to/from xml
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ChunkFS
{
    public class CFSConfig
    {
        public List<CFSDriveConfig> Drives { get; set; }

        public CFSConfig()
        {
            Drives = new List<CFSDriveConfig>();
        }

        public void add(CFSDriveConfig drive)
        {
            Drives.Add(drive);
        }

        public static string ConfigFilePath(string fname)
        {
            string rv = (fname == null || fname.Length == 0) ? "CFS.config" : fname;
            string appath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChunkFS");
            DirectoryInfo di = new DirectoryInfo(appath);
            if (!di.Exists) di.Create();
            return di.FullName + Path.DirectorySeparatorChar + rv;
        }

        #region Serialization

        public bool Save(string optionalName)
        {
            StreamWriter myWriter = null;
            XmlSerializer mySerializer = null;
            bool rv = false;
            try
            {
                mySerializer = new XmlSerializer(typeof(CFSConfig));
                myWriter = new StreamWriter(ConfigFilePath(optionalName), false);
                mySerializer.Serialize(myWriter, this);
                rv = true;
            }
            catch (Exception ex)
            {
                Logger.getLogger().log(ex.Message);
            }
            finally
            {
                if (myWriter != null)
                {
                    myWriter.Close();
                }
            }
            return rv;
        }
        public static CFSConfig Load(string optionalName)
        {
            XmlSerializer mySerializer = null;
            FileStream myFileStream = null;
            CFSConfig rv = null;
            try
            {
                mySerializer = new XmlSerializer(typeof(CFSConfig));
                FileInfo fi = new FileInfo(ConfigFilePath(optionalName));
                if (fi.Exists)
                {
                    myFileStream = fi.OpenRead();
                    rv = (CFSConfig)mySerializer.Deserialize(myFileStream);
               }
            }
            catch (Exception ex)
            {
                Logger.getLogger().log(ex.Message);
            }
            finally
            {
                if (myFileStream != null)
                {
                    myFileStream.Close();
                }
            }
            return rv;
        }
        #endregion 

    }

    public class CFSDriveConfig
    {
        public String Name { get; set; }
        public String MountPoint { get; set; }
        public List<String> Files { get; set; }
        public String ChunkSize { get; set; }
        public Boolean RunningChunks { get; set; }
        public Double MinimumChunk { get; set; }
        public Boolean UseExtension { get; set; }
        public Boolean ShowSubdirs { get; set; }
        public Boolean SmartChunk { get; set; }
        [XmlIgnoreAttribute()]
        public Boolean Mounted { get; set; }
        [XmlIgnoreAttribute()]
        public ChunkFSDriver RunningDriver { get; set; }


        public CFSDriveConfig() {
            this.Files = new List<string>();
            MinimumChunk = 0.1;
        }


        public CFSDriveConfig(string name, string mount, string csize, string[] files, int startix) 
        {
            this.Name = name;
            this.MountPoint = mount;
            this.ChunkSize = csize;
            this.MinimumChunk = 0.1;
            this.Files = new List<string>();
            for (int i = startix; i < files.Length; i++)
                this.Files.Add(files[i]);
        }

        public bool isValid()
        {
            bool rv = true;
            if (rv) rv = this.MountPoint!=null && this.MountPoint.Length == 3;
            if (rv) 
            {
                char[] mp = MountPoint.ToUpper().ToCharArray();
                rv = mp[0] >= 'C' && mp[0] <= 'Z';
                if (rv) rv = mp[1] == Path.VolumeSeparatorChar;
                if (rv) rv = mp[2] == Path.DirectorySeparatorChar;
            }
            if (rv) rv = Files.Count > 0;
            if (rv) rv = FileHelper.isAnyData(Files);
            return rv;
        }
    }
}
