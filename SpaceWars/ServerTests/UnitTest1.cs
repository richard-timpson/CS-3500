using System;
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
            for (int i = 0; i < 100; i++)
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
            Assert.AreEqual(100, shipCount);

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
            ServerClass.ClientConnections = new Dictionary<int, Client>();
            for (int i = 0; i < 4; i++)
            {
                SocketInformation si = new SocketInformation();
                Socket s = null;
                Networking.SocketState ss = new Networking.SocketState(s, socketstate => { }, i);
                Client c = new Client(i, "JonDoe" + i, ss);
                ServerClass.ClientConnections.Add(c.ID, c);
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
            ServerClass.ProcessShips();
        }

        [TestMethod()]
        public void TestProcessProjectiles()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.ClientConnections = new Dictionary<int, Client>();
            Dictionary<int, Vector2D> projectileCompare = new Dictionary<int, Vector2D>();
            int clientCounter = 0;
            for (int i = 0; i < 4; i++)
            {
                Socket s = null;
                Networking.SocketState ss = new Networking.SocketState(s, socketstate => { }, i);
                Client c = new Client(i, "JonDoe" + i, ss);
                ServerClass.ClientConnections.Add(c.ID, c);
                ServerClass.InsertShip(i, "JonDoe" + 1, 0);
            }


            foreach (KeyValuePair<int, Client> c in ServerClass.ClientConnections)
            {
                Client client = c.Value;
                ServerClass.ClientConnections[clientCounter].fire = true;
                ServerClass.ProcessCommands();
                clientCounter++;
            }
            foreach(Projectile proj in ServerClass.TheWorld.GetProjectiles())
            {
                projectileCompare.Add(proj.ID, proj.loc);
            }

            ServerClass.ProcessProjectiles();

            foreach(Projectile proj in ServerClass.TheWorld.GetProjectiles())
            {
                Assert.AreEqual(15, (proj.loc - projectileCompare[proj.ID]).Length());
            }


            foreach (Projectile proj in ServerClass.TheWorld.GetProjectiles())
            {
                projectileCompare[proj.ID] = proj.dir;
            }

            ServerClass.ProcessProjectiles();

            foreach (Projectile proj in ServerClass.TheWorld.GetProjectiles())
            {
                Assert.AreEqual(proj.dir, projectileCompare[proj.ID]);
            }
            ServerClass.ProcessShips();
        }


        [TestMethod()]
        public void TestProcessProjectilesDeadProjectile()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.ClientConnections = new Dictionary<int, Client>();
            Dictionary<int, Vector2D> projectileCompare = new Dictionary<int, Vector2D>();
            int clientCounter = 0;
            int projCounter = 0;
            int projCounter2 = 0;
            for (int i = 0; i < 4; i++)
            {
                Socket s = null;
                Networking.SocketState ss = new Networking.SocketState(s, socketstate => { }, i);
                Client c = new Client(i, "JonDoe" + i, ss);
                ServerClass.ClientConnections.Add(c.ID, c);
                ServerClass.InsertShip(i, "JonDoe" + i, 0);
            }


            foreach (KeyValuePair<int, Client> c in ServerClass.ClientConnections)
            {
                Client client = c.Value;
                ServerClass.ClientConnections[clientCounter].fire = true;
                ServerClass.ProcessCommands();
                clientCounter++;
            }
            
            foreach (Projectile p in ServerClass.TheWorld.GetProjectiles())
            {
                projCounter++;
            }

            Assert.IsTrue(projCounter > 0);

            for (int i = 0; i < 50; i++)
            {
                ServerClass.ProcessProjectiles();
            }

            foreach (Projectile p in ServerClass.TheWorld.GetProjectiles())
            {
                projCounter2++;
            }

            Assert.AreEqual(0, projCounter2);
            ServerClass.ProcessShips();
        }

        [TestMethod()]
        public void TestProcessProjectilesKillShot()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.ClientConnections = new Dictionary<int, Client>();
            Dictionary<int, Vector2D> projectileCompare = new Dictionary<int, Vector2D>();
            int clientCounter = 0;
            int projCounter = 0;
            int projCounter2 = 0;
            for (int i = 0; i < 4; i++)
            {
                Socket s = null;
                Networking.SocketState ss = new Networking.SocketState(s, socketstate => { }, i);
                Client c = new Client(i, "JonDoe" + i, ss);
                ServerClass.ClientConnections.Add(c.ID, c);
                ServerClass.InsertShip(i, "JonDoe" + i, 0);
            }

            Vector2D shipLoc = new Vector2D(ServerClass.TheWorld.GetShipAtId(0).loc);
            Vector2D offset = new Vector2D(0, 50);
            ServerClass.TheWorld.GetShipAtId(1).SetLoc(shipLoc + offset);


            for (int i = 0; i < 10; i++)
            {
                foreach (KeyValuePair<int, Client> c in ServerClass.ClientConnections)
                {
                    Client client = c.Value;
                    ServerClass.ClientConnections[clientCounter].fire = true;
                    ServerClass.ProcessCommands();
                    for (int j = 0; j < 10; j++)
                    {
                        ServerClass.ProcessProjectiles();
                    }
                    ServerClass.TheWorld.GetShipAtId(0).fireRateCounter = Convert.ToInt32(ServerClass.gameSettings["FramesPerShot"]);
                    clientCounter++;
                }

                clientCounter = 0;
            }

            Assert.AreEqual(0, ServerClass.TheWorld.GetShipAtId(1).hp);
            ServerClass.ProcessShips();
        }

        [TestMethod()]
        public void TestProcessProjectilesStarShot()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.ClientConnections = new Dictionary<int, Client>();
            Dictionary<int, Vector2D> projectileCompare = new Dictionary<int, Vector2D>();
            Socket s = null;
            Networking.SocketState ss = new Networking.SocketState(s, socketstate => { }, 0);
            ServerClass.InsertStars();
            Client c = new Client(0, "JonDoe", ss);
            ServerClass.ClientConnections.Add(c.ID, c);
            ServerClass.InsertShip(0, "JonDoe", 0);
            int projCounter = 0;
            Vector2D offset = new Vector2D(0, -50);
            ServerClass.TheWorld.GetShipAtId(0).SetLoc(offset);

            for (int i = 0; i < 20; i++)
            {
                ServerClass.ClientConnections[0].fire = true;
                ServerClass.ProcessCommands();
                for (int j = 0; j < 10; j++)
                {
                    ServerClass.ProcessProjectiles();
                }
                ServerClass.TheWorld.GetShipAtId(0).fireRateCounter = Convert.ToInt32(ServerClass.gameSettings["FramesPerShot"]);
            }

            foreach (Projectile p in ServerClass.TheWorld.GetProjectiles())
            {
                projCounter++;
            }

            Assert.AreEqual(0, projCounter);
            ServerClass.ProcessShips();
        }

        [TestMethod()]
        public void TestProcessShips()
        {
            ServerClass.TheWorld = new World();
            string path = "../../ValidXmlSettings.xml";
            ServerClass.gameSettings = ServerClass.XmlSettingsReader(path);
            ServerClass.ClientConnections = new Dictionary<int, Client>();
            ServerClass.InsertStars();
            List<Vector2D> locations = new List<Vector2D>();
            for ( int i = 0; i < 20; i++)
            {
                ServerClass.InsertShip(i, "john" + 1, 0);
                if (i == 5)
                {
                    ServerClass.TheWorld.GetShipAtId(i).SetLoc(new Vector2D(0,0));
                }
                if (i == 6)
                {
                    ServerClass.TheWorld.GetShipAtId(i).SetDir(new Vector2D(0,-1));
                    ServerClass.TheWorld.GetShipAtId(i).SetThrust(true);
                }
                if (i == 7)
                {
                    ServerClass.TheWorld.GetShipAtId(i).SetDir(new Vector2D(-1, 0));
                    ServerClass.TheWorld.GetShipAtId(i).SetThrust(true);
                }
                if (i == 8)
                {
                    ServerClass.TheWorld.GetShipAtId(i).SetDir(new Vector2D(1, 0));
                    ServerClass.TheWorld.GetShipAtId(i).SetThrust(true);
                }
                if (i == 9)
                {
                    ServerClass.TheWorld.GetShipAtId(i).SetThrust(true);
                }
                locations.Add(ServerClass.TheWorld.GetShipAtId(i).loc);
                ServerClass.ProcessShips();
            }
            ServerClass.ProcessShips();
            int idCounter = 0;
            foreach (Vector2D loc in locations)
            {
                Assert.AreNotEqual(ServerClass.TheWorld.GetShipAtId(idCounter).loc, loc);
            }

            int gameCounter = 0;
            ServerClass.TheWorld.GetShipAtId(6).fireRateCounter = 3;
            while (gameCounter< 1000)
            {
                ServerClass.UpdateWorld();
                gameCounter++;
            }

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
        

