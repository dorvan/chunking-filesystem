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

using ChunkFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ChunkFS_TestProject1
{
    
    
    /// <summary>
    ///This is a test class for CFSConfigTest and is intended
    ///to contain all CFSConfigTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CFSConfigTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Save and Load
        ///</summary>
        [TestMethod()]
        public void SaveLoadTest()
        {
            CFSConfig target = new CFSConfig();
            target.add(makeConfig("test1"));
            target.add(makeConfig("test2"));
            string optionalName = "testCfg.config"; 
            bool expected = true; 
            bool actual;
            actual = target.Save(optionalName);
            Assert.AreEqual(expected, actual);

            CFSConfig loaded;
            loaded = CFSConfig.Load(optionalName);
            Assert.AreEqual(target.Drives.Count, loaded.Drives.Count);
 
        }

        private CFSDriveConfig makeConfig(string name)
        {
            string mount = "k:\\";
            string csize = "22.5G";
            string[] files = { "k:\\", "22.5G", "file1.big", "file2.big" };
            int startix = 2;
            return new CFSDriveConfig(name, mount, csize, files, startix);
        }

        /// <summary>
        ///A test for ConfigFilePath
        ///</summary>
        [TestMethod()]
        public void ConfigFilePathTest()
        {
            string fname = string.Empty; 
            string expected = "\\ChunkFS\\CFS.config"; 
            string actual;
            actual = CFSConfig.ConfigFilePath(fname);
            Assert.IsTrue(actual.EndsWith(expected) && actual.IndexOf(":") > 0);
        }
    }
}
