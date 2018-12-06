using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;
using System.Xml;
using NetworkController;
using GameModel;
using Vector;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Server
{
    public class ServerClass
    {
        public static List<Client> ClientConnections { get; set; }
        //private static int IdCounter { get; set; }

        public static Dictionary<string, object> gameSettings { get; set; }

        public static World TheWorld { get; set; }

        public static int projectileCounter { get; private set; }

        static void Main(string[] args)
        {
            ClientConnections = new List<Client>();
            string settingsFilePath = "..\\..\\..\\Resources\\settings.xml";
            gameSettings = XmlSettingsReader(settingsFilePath);
            TheWorld = new World();
            InsertStars();
            projectileCounter = 0;
            Networking.NetworkController.ServerAwaitingClientLoop(HandleNewClient, 0);
            Stopwatch watch = new Stopwatch();
            while (true)
            {
                watch.Start();
                while (watch.ElapsedMilliseconds < Convert.ToInt32(gameSettings["MSPerFrame"])) { }
                Update();
                watch.Reset();
            }


        }


        private static void HandleNewClient(Networking.SocketState ss)
        {
            ss._call = ReceiveName;
            Networking.NetworkController.GetData(ss);
        }

        /// <summary>
        /// Receives the name of the client after it's connection. 
        /// This method needs to be tested to ensure that it handles the different cases
        /// for the ways that the name can be passed. 
        /// </summary>
        /// <param name="ss"></param>
        private static void ReceiveName(Networking.SocketState ss)
        {
            ss._call = ReceiveCommand;
            string totalData = ss.sb.ToString();
            string[] name = totalData.Split('\n');
            Client client = new Client(ss.ID, name[0], ss);
            lock (TheWorld)
            {
                ClientConnections.Add(client);
            }
            string startupInfo = ss.ID + "\n" + gameSettings["UniverseSize"] + "\n";
            InsertShip(ss.ID, name[0], 0);
            Networking.NetworkController.Send(startupInfo, ss);
            Networking.NetworkController.GetData(ss);
        }
        private static void ReceiveCommand(Networking.SocketState ss)
        {
            string totalData = ss.sb.ToString();
            char[] commands = totalData.ToCharArray();

            lock (TheWorld)
            {
                foreach (Client client in ClientConnections)
                {
                    if (client.ID == ss.ID)
                    {
                        foreach (char s in commands)
                        {
                            if (s != '(' || s != ')')
                            {
                                switch (s)
                                {
                                    case 'L':
                                        if (!client.right) client.left = true;
                                        break;
                                    case 'R':
                                        if (!client.left) client.right = true;
                                        break;
                                    case 'T':
                                        client.thrust = true;
                                        break;
                                    case 'F':
                                        client.fire = true;
                                        break;
                                }
                            }

                        }

                    }
                }
            }

            ss.sb.Clear();
            Networking.NetworkController.GetData(ss);
        }
        public static Dictionary<string, object> XmlSettingsReader(string filePath)
        {
            List<double[]> stars = new List<double[]>();
            Dictionary<string, object> gameSettings = new Dictionary<string, object>();
            gameSettings.Add("stars", stars);
            bool openfile = true;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath, settings))
                {
                    while (openfile)
                    {
                        if (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "UniverseSize")
                                {
                                    reader.Read();
                                    gameSettings.Add("UniverseSize", Convert.ToInt32(reader.Value));
                                }
                                if (reader.Name == "EnginePower")
                                {
                                    reader.Read();
                                    gameSettings.Add("EnginePower", Convert.ToDouble(reader.Value));
                                }
                                if (reader.Name == "MSPerFrame")
                                {
                                    reader.Read();
                                    gameSettings.Add("MSPerFrame", Convert.ToInt32(reader.Value));
                                }
                                if (reader.Name == "FramesPerShot")
                                {
                                    reader.Read();
                                    gameSettings.Add("FramesPerShot", Convert.ToInt32(reader.Value));
                                }
                                if (reader.Name == "RespawnRate")
                                {
                                    reader.Read();
                                    gameSettings.Add("RespawnRate", Convert.ToInt32(reader.Value));
                                }
                                if (reader.Name == "StartingHP")
                                {
                                    reader.Read();
                                    gameSettings.Add("StartingHP", Convert.ToInt32(reader.Value));
                                }
                                if (reader.Name == "RespawnTime")
                                {
                                    reader.Read();
                                    gameSettings.Add("RespawnTime", Convert.ToInt32(reader.Value));
                                }
                                if (reader.Name == "FancyGame")
                                {
                                    reader.Read();
                                    gameSettings.Add("FancyGame", reader.Value);
                                }
                                if (reader.Name == "Star")
                                {
                                    double[] star = new double[] {0,0, 0};
                                    bool starActive = true;
                                    while (starActive)
                                    {
                                        if (reader.Read())
                                        {
                                            if (reader.IsStartElement())
                                            {
                                                if (reader.Name == "x")
                                                {
                                                    reader.Read();
                                                    star[0] = Convert.ToDouble(reader.Value);
                                                }
                                                if (reader.Name == "y")
                                                {
                                                    reader.Read();
                                                    star[1] = Convert.ToDouble(reader.Value);
                                                }
                                                if (reader.Name == "mass")
                                                {
                                                    reader.Read();
                                                    star[2] = Convert.ToDouble(reader.Value);
                                                }
                                            }
                                            if (reader.NodeType == XmlNodeType.EndElement)
                                            {
                                                if (reader.Name == "Star")
                                                {
                                                    starActive = false;
                                                }
                                            }
                                        }
                                    }
                                    List<double[]> temp = new List<double[]>();
                                    temp = (List<double[]>)(gameSettings["stars"]);
                                    temp.Add(star);
                                    gameSettings["stars"] = temp;
                                }
                            }
                        }
                        else
                        {
                            // if we aren't reading elements anymore, close the file. 
                            openfile = false;

                        }
                    }
                }
                return gameSettings;
            }
            catch (FormatException E)
            {
                Console.WriteLine(E.Message);
                throw E;
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                throw E;
            }
        }

        public static void InsertStars()
        {
            if ((string)gameSettings["FancyGame"] != "Yes")
            {
                List<double[]> temp = new List<double[]>();
                temp = (List<double[]>)(gameSettings["stars"]);
                int StarIdCounter = 0;
                lock (TheWorld)
                {
                    foreach (double[] s in temp)
                    {
                        Star star = new Star();
                        star.SetID(StarIdCounter);
                        Vector2D loc = new Vector2D(s[0], s[1]);
                        star.SetLoc(loc);
                        star.SetMass(s[2]);
                        TheWorld.AddStar(star);
                        StarIdCounter++;
                    }
                }
            }
            else
            {
                double radius = (int)gameSettings["UniverseSize"] / 3.5;

                Star star1 = new Star();
                star1.SetID(1);
                Vector2D loc1 = new Vector2D(0, radius);
                star1.SetLoc(loc1);
                star1.SetDir(new Vector2D(1, 0));
                star1.SetMass(.01);
                TheWorld.AddStar(star1);

                Star star2 = new Star();
                star2.SetID(2);
                Vector2D loc2 = new Vector2D(0, -radius);
                star2.SetLoc(loc2);
                star2.SetDir(new Vector2D(-1, 0));
                star2.SetMass(.01);
                TheWorld.AddStar(star2);

                Star star3 = new Star();
                star3.SetID(3);
                Vector2D loc3 = new Vector2D(radius, 0);
                star3.SetLoc(loc3);
                star3.SetDir(new Vector2D(0, -1));
                star3.SetMass(.01);
                TheWorld.AddStar(star3);

                Star star4 = new Star();
                star4.SetID(4);
                Vector2D loc4 = new Vector2D(-radius, 0);
                star4.SetLoc(loc4);
                star4.SetDir(new Vector2D(0, 1));
                star4.SetMass(.01);
                TheWorld.AddStar(star4);
            }

        }

        public static void InsertShip(int id, string name, int score)
        {
            Ship s = new Ship();


            s.SetID(id);
            s.SetName(name);
            s.SetScore(score);

            lock (TheWorld)
            {
                SpawnShip(s);
                TheWorld.AddShipAll(s);
            }
        }

        public static void InsertProjectile(int id, Vector2D loc, Vector2D dir, Vector2D vel, Ship ship)
        {
            if (ship.hp > 0)
            {
                Projectile p = new Projectile(projectileCounter, loc, dir, vel, true, id);

                TheWorld.AddProjectile(ship.ID, p);
                projectileCounter++;
            }
        }

        public static void Update()
        {
            UpdateWorld();
            SendWorld();
        }

        public static void UpdateWorld()
        {
            lock (TheWorld)
            {
                ProcessCommands();
                ProcessProjectiles();
                ProcessShips();

                if ((string)gameSettings["FancyGame"] == "Yes")
                {
                    ProcessStars();
                }
            }
        }

        public static void ProcessCommands()
        {
            foreach (Client c in ClientConnections)
            {
                foreach (Ship s in TheWorld.GetShipsAll())
                {
                    if (s.ID == c.ID)
                    {
                        if (c.left == true)
                        {
                            Vector2D temp = new Vector2D(s.dir);
                            temp.Rotate(-5);
                            s.SetDir(temp);
                            c.left = false;
                        }
                        if (c.right == true)
                        {
                            Vector2D temp = new Vector2D(s.dir);
                            temp.Rotate(5);
                            s.SetDir(temp);
                            c.right = false;
                        }
                        if (c.thrust == false)
                        {
                            s.SetThrust(false);
                        }
                        if (c.thrust == true)
                        {
                            s.SetThrust(true);
                            c.thrust = false;
                        }
                        if (c.fire == true && s.fireRateCounter == Convert.ToInt32(gameSettings["FramesPerShot"]))
                        {
                            Vector2D temp = new Vector2D(s.loc);
                            Vector2D projVel = new Vector2D(s.dir * 15);
                            Vector2D startPos = new Vector2D(s.loc + (s.dir * 20));
                            InsertProjectile(c.ID, startPos, s.dir, projVel, s);
                            c.fire = false;
                            s.fireRateCounter = -1;
                        }
                    }
                }
            }
        }

        public static void ProcessProjectiles()
        {
            int worldSize = Convert.ToInt32(gameSettings["UniverseSize"]) - 1;
            List<Projectile> projToDelete = new List<Projectile>();

            foreach (Projectile p in TheWorld.GetProjectiles())
            {
                if (p.alive == false)
                {
                    projToDelete.Add(p);
                }
                else
                {
                    Vector2D newLoc = new Vector2D(p.loc + p.vel);
                    p.SetLoc(newLoc);
                    if (p.loc.GetX() >= worldSize / 2 || p.loc.GetY() >= worldSize / 2 || p.loc.GetX() <= -worldSize / 2 || p.loc.GetY() <= -worldSize / 2)
                    {
                        p.SetAlive(false);
                    }
                    foreach (Ship ship in TheWorld.GetShipsAll())
                    {
                        if ((ship.loc - p.loc).Length() <= 20)
                        {
                            if (ship.hp > 0 && ship.ID != p.owner)
                            {
                                int newHP = ship.hp - 1;
                                ship.SetHp(newHP);
                                if (newHP == 0)
                                {
                                    int score = TheWorld.GetShipAtId(p.owner).score;
                                    TheWorld.GetShipAtId(p.owner).SetScore(score + 1);
                                }
                                p.SetAlive(false);
                            }
                            if (ship.hp == 0)
                            {
                                ship.deathCounter++;
                                
                            }

                        }
                    }
                    foreach (Star star in TheWorld.GetStars())
                    {
                        if ((star.loc - p.loc).Length() <= 35)
                        {
                            p.SetAlive(false);
                        }
                    }
                }
            }

            foreach (Projectile p in projToDelete)
            {
                TheWorld.RemoveProjectile(p.owner, p.ID);
            }
            projToDelete.Clear();
        }

        public static void ProcessStars()
        {
            foreach (Star s in TheWorld.GetStars())
            {
                    Vector2D temp = new Vector2D(s.dir);
                    Vector2D temp2 = new Vector2D(s.dir);
                    temp.Rotate(-1);
                    s.SetDir(temp);
                    Vector2D tempLoc = new Vector2D(s.loc + (temp2 * Math.Tan(Math.PI/180) * (Convert.ToInt32(gameSettings["UniverseSize"]) / 3.5)));
                    s.SetLoc(tempLoc);
            }
        }

        public static void ProcessShips()
        {
            int worldSize = Convert.ToInt32(gameSettings["UniverseSize"]) - 1;
            foreach (Ship ship in TheWorld.GetShipsAll())
            {
                if (ship.deathCounter > 0 && ship.deathCounter < Convert.ToInt32(gameSettings["RespawnTime"]))
                {
                    ship.deathCounter++;
                }
                if (ship.deathCounter == Convert.ToInt32(gameSettings["RespawnTime"]))
                {
                    SpawnShip(ship);
                }
                if (ship.fireRateCounter < Convert.ToInt32(gameSettings["FramesPerShot"]))
                {
                    ship.fireRateCounter++;
                }
                Vector2D totalAccel = new Vector2D(0, 0);
                foreach (Star star in TheWorld.GetStars())
                {
                    Vector2D grav = star.loc - ship.loc;
                    grav.Normalize();
                    grav = grav * star.mass;
                    totalAccel += grav;
                }

                double enginePower = Convert.ToDouble(gameSettings["EnginePower"]);
                if (ship.thrust == true)
                {
                    Vector2D thrust = new Vector2D(ship.dir);
                    thrust = thrust * enginePower;
                    totalAccel += thrust;
                }
                Vector2D newVel = new Vector2D(ship.vel + totalAccel);
                if (newVel.Length() > 20)
                {
                    newVel.Normalize();
                    newVel = newVel * 20;
                }
                ship.SetVelocity(newVel);
                Vector2D newLoc = new Vector2D(ship.loc + ship.vel);
                ship.SetLoc(newLoc);

                if (newLoc.GetX() >= worldSize / 2)
                {
                    Vector2D temp = new Vector2D(-worldSize / 2, newLoc.GetY());
                    ship.SetLoc(temp);
                }
                if (newLoc.GetX() <= -worldSize / 2)
                {
                    Vector2D temp = new Vector2D(worldSize / 2, newLoc.GetY());
                    ship.SetLoc(temp);
                }
                if (newLoc.GetY() >= worldSize / 2)
                {
                    Vector2D temp = new Vector2D(newLoc.GetX(), -worldSize / 2);
                    ship.SetLoc(temp);
                }
                if (newLoc.GetY() <= -worldSize / 2)
                {
                    Vector2D temp = new Vector2D(newLoc.GetX(), worldSize / 2);
                    ship.SetLoc(temp);
                }

                foreach (Star star in TheWorld.GetStars())
                {
                    if ((ship.loc - star.loc).Length() <= 35)
                    {
                        ship.SetHp(0);
                        ship.deathCounter++;
                    }
                }

            }
        }

        public static void SpawnShip(Ship s)
        {
            Random rand = new Random();
            int LocX;
            int LocY;
            bool safeSpawn = false;
            Vector2D position = new Vector2D(0.0, 0.0);

            //loop through potential random spawn locations to find location that is 
            //not near a star or a ship.
            while (!safeSpawn)
            {
                int worldSize = Convert.ToInt32(gameSettings["UniverseSize"]) - 1;
                LocX = rand.Next(-(worldSize / 2), worldSize / 2);
                LocY = rand.Next(-(worldSize / 2), worldSize / 2);
                position = new Vector2D(LocX, LocY);

                bool starSpawn = true;
                bool shipSpawn = true;


                foreach (Star star in TheWorld.GetStars())
                {
                    if ((star.loc - position).Length() <= 50)
                    {
                        starSpawn = false;
                    }

                }

                foreach (Ship ship in TheWorld.GetShipsAll())
                {
                    if ((ship.loc - position).Length() <= 50)
                    {
                        shipSpawn = false;
                    }

                }

                if (starSpawn == true && shipSpawn == true)
                {
                    safeSpawn = true;
                }
            }

            s.SetLoc(position);
            s.SetHp(Convert.ToInt32(gameSettings["StartingHP"]));
            s.SetThrust(false);
            Vector2D vel = new Vector2D(0, 0);
            s.SetVelocity(vel);
            s.deathCounter = 0;
            Vector2D Dir = new Vector2D(0, 1);
            s.SetDir(Dir);
            s.fireRateCounter = Convert.ToInt32(gameSettings["FramesPerShot"]);
        }

        public static void SendWorld()
        {
            StringBuilder jsonString = new StringBuilder() ;
            lock (TheWorld)
            {
                foreach (Ship ship in TheWorld.GetShipsAll())
                {
                    jsonString.Append(JsonConvert.SerializeObject(ship) + "\n");
                }
                foreach (Star star in TheWorld.GetStars())
                {
                    jsonString.Append(JsonConvert.SerializeObject(star) + "\n");
                }
                foreach (Projectile projectile in TheWorld.GetProjectiles())
                {
                    jsonString.Append(JsonConvert.SerializeObject(projectile) + "\n");
                }
            }
            lock(TheWorld)
            {
                foreach (Client client in ClientConnections)
                {
                    if (client.ss.theSocket.Connected)
                    {
                        Networking.NetworkController.Send(jsonString.ToString(), client.ss);
                    }
                }
            }
        }

    }

    public class Client
    {
        public int ID { get; set; }
        public string name;
        public string command { get; set; }

        public bool fire { get; set; }
        public bool thrust { get; set; }
        public bool left { get; set; }
        public bool right { get; set; }

        public Networking.SocketState ss { get; set; }

        public Client(int _ID, string Name, Networking.SocketState _ss)
        {
            ID = _ID;
            name = Name;
            command = "";
            fire = false;
            thrust = false;
            left = false;
            right = false;
            ss = _ss;
        }


    }
}
