﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System.IO;
using GameModel;
using Vector;
using NetworkController;
using System.Net.Sockets;
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
            Assert.AreEqual(16, gameSettings["MSPerFrame"]);
            Assert.AreEqual(6, gameSettings["FramesPerShot"]);
            Assert.AreEqual(300, gameSettings["RespawnRate"]);
            Assert.AreEqual(5, gameSettings["StartingHP"]);
            Assert.AreEqual(0.2, gameSettings["EnginePower"]);
            Assert.AreEqual(120, gameSettings["RespawnTime"]);
            List<double[]> temp = (List<double[]>)gameSettings["stars"];
            int counter = 0;
            foreach (double[] star in temp)
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
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.InsertStars();
            int starCount = 0;
            foreach (Star s in ServerClass.TheWorld.GetStars())
            {
                starCount++;
            }
            Assert.AreEqual(2, starCount);
        }

        [TestMethod()]
        public void TestInsertShip()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.InsertShip(0, "JohnDoe", 0);
            ServerClass.InsertShip(1, "Timmy", 0);
            ServerClass.InsertStars();
            int shipCount = 0;
            Vector2D vel = new Vector2D(0, 0);
            foreach (Ship s in ServerClass.TheWorld.GetShipsAll())
            {
                if (shipCount == 0)
                {
                    Assert.AreEqual(0, s.ID);
                    Assert.AreEqual(0, s.score);
                    Assert.AreEqual("JohnDoe", s.name);
                    Assert.AreEqual(vel, s.vel);
                }
                shipCount++;
            }
            Assert.AreEqual(2, shipCount);
            
        }

        [TestMethod()]
        public void TestInsertTooManyShips()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidStarsXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.InsertStars();
            for (int i = 0; i < 150; i++)
            {
                ServerClass.InsertShip(i, "JohnDoe" + i, 0);

            }
            int shipCount = 0;
            Vector2D vel = new Vector2D(0, 0);
            foreach (Ship s in ServerClass.TheWorld.GetShipsAll())
            {
                if (shipCount == 0)
                {
                    Assert.AreEqual(0, s.ID);
                    Assert.AreEqual(0, s.score);
                    Assert.AreEqual("JohnDoe0", s.name);
                    Assert.AreEqual(vel, s.vel);
                }
                shipCount++;
            }
            Assert.AreEqual(150, shipCount);

        }

        [TestMethod()]
        public void TestInsertProjectiles()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.InsertStars();
            for (int i = 0; i < 10; i++)
            {
                ServerClass.InsertShip(i, "JohnDoe" + i, 0);

            }
            foreach (Ship s in ServerClass.TheWorld.GetShipsAll())
            {
                ServerClass.InsertProjectile(ServerClass.projectileCounter, (s.loc + (s.dir * 20)), s.dir, s.dir * 15, s);
            }
            int projectileCounter = 0;
            foreach (Projectile p in ServerClass.TheWorld.GetProjectiles())
            {
                projectileCounter++;

            }
            Assert.AreEqual(10, projectileCounter);

        }

        [TestMethod()]
        public void TestProcessCommands()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.ClientConnections = new List<Client>();
            for (int i = 0; i < 4; i++)
            {
                SocketInformation si = new SocketInformation();
                Socket s = null;
                Networking.SocketState ss = new Networking.SocketState(s, socketstate => { }, i);
                Client c = new Client(i, "JonDoe" + i, ss);
                ServerClass.ClientConnections.Add(c);
                ServerClass.InsertShip(i, "JonDoe" + 1, 0);
            }


            ServerClass.ClientConnections[0].right = true;
            Vector2D prevDir1 = ServerClass.TheWorld.GetShipAtId(0).dir;
            ServerClass.ProcessCommands();
            Assert.IsFalse(ServerClass.ClientConnections[0].right);
            Vector2D newDir1 = ServerClass.TheWorld.GetShipAtId(0).dir;
            Assert.AreNotEqual(prevDir1, newDir1);

            ServerClass.ClientConnections[0].left = true;
            Vector2D prevDir2 = ServerClass.TheWorld.GetShipAtId(0).dir;
            ServerClass.ProcessCommands();
            Assert.IsFalse(ServerClass.ClientConnections[0].right);
            Vector2D newDir2 = ServerClass.TheWorld.GetShipAtId(0).dir;
            Assert.AreNotEqual(prevDir2, newDir2);

            ServerClass.ClientConnections[0].thrust = true;
            ServerClass.ProcessCommands();
            Ship ship = ServerClass.TheWorld.GetShipAtId(0);
            Assert.IsTrue(ship.thrust);
            Assert.IsFalse(ServerClass.ClientConnections[0].right);


            ServerClass.ClientConnections[0].fire = true;
            ServerClass.ProcessCommands();
            int projectileCount = 0;
            foreach (Projectile p in ServerClass.TheWorld.GetProjectiles())
            {
                Assert.AreEqual(ship.dir, p.dir);
                projectileCount++;
            }
            Assert.AreEqual(1, projectileCount);
            Assert.AreEqual(-1, ship.fireRateCounter);
            
        }

        [TestMethod()]
        public void TestProcessProjectiles()
        {
            

        }

    }
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
        

