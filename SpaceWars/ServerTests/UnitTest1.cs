using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System.IO;
using GameModel;
namespace ServerTests
{
    [TestClass]
    public class InitialTests
    {
        /// <summary>
        /// Testing to make sure all of the values are what they should be
        /// and that we can add multiple stars
        /// </summary>
        [TestMethod()]
        public void TestXmlReader()
        {
            string path = "../../ValidXmlSettings.xml";
            Dictionary<string, object> gameSettings = ServerClass.XmlSettingsReader(path);
            Assert.AreEqual(750, gameSettings["UniverseSize"]);
            Assert.AreEqual(16,gameSettings["MSPerFrame"]);
            Assert.AreEqual(6, gameSettings["FramesPerShot"]);
            Assert.AreEqual(300, gameSettings["RespawnRate"]);
            Assert.AreEqual(5, gameSettings["StartingHP"]);
            Assert.AreEqual(0.2, gameSettings["EnginePower"]);
            Assert.AreEqual(120,gameSettings["RespawnTime"]);
            List<double[]> temp = (List<double[]>)gameSettings["stars"];
            int counter = 0;
            foreach(double[] star in temp)
            {
                if (counter == 0)
                {
                    Assert.AreEqual((double)0, star[0]);
                    Assert.AreEqual((double)0, star[1]);
                    Assert.AreEqual(0.01, star[2]);
                }
                if (counter == 1)
                {
                    Assert.AreEqual((double)1, star[0]);
                    Assert.AreEqual((double)1, star[1]);
                    Assert.AreEqual(0.02, star[2]);
                }
                counter++;
            }
        }
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestInvalidXmlPropertyType()
        {
            string path = "../../InValidXmlSettings.xml";
            Dictionary<string, object> gameSettings = ServerClass.XmlSettingsReader(path);

        }
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestInvalidFilePath()
        {
            string path = "../../InValidXlSettings.xml";
            Dictionary<string, object> gameSettings = ServerClass.XmlSettingsReader(path);

        }

        [TestMethod()]
        public void TestInsertStar()
        {
            public World TheWorld = new World;

        }

            // test star insertion with multiple stars
            // test ship instertion with multiple ships
            // test projectile insertion with multiple projectiles
            // test update function
            // test updateworld function
            // test commands
            //      L
            //      R
            //      T
            //      F
            //      combinations of the them all
            // test projectiles and collisions
            // test ships, firing projectiles, and collisions
            // test send world, make sure we are sending correct json
        }
}
