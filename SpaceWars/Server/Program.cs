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
            Networking.NetworkController.Error += ErrorHandler;
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

        /// <summary>
        /// Inserts star into the world's list of stars based on settings from the game settings xml file
        /// </summary>
        public static void InsertStars()
        {
            //If the fancy game mode is off, loads stars from the settings file
            if ((string)gameSettings["FancyGame"] != "Yes" || (string)gameSettings["FancyGame"] != "yes")
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
            //If the fancy game mode is on, loads 4 stars and prepares them for orbit
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

        /// <summary>
        /// Inserts ship into the world's dictionary of ships
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="score"></param>
        public static void InsertShip(int id, string name, int score)
        {
            Ship s = new Ship();


            s.SetID(id);
            s.SetName(name);
            s.SetScore(score);

            lock (TheWorld)
            {
                //Spawn ship randomizes location and sets direction
                SpawnShip(s);
                //Adds ship to dictionary
                TheWorld.AddShipAll(s);
            }
        }

        /// <summary>
        /// Inserts projectile into the world's dictionary of projectiles
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loc"></param>
        /// <param name="dir"></param>
        /// <param name="vel"></param>
        /// <param name="ship"></param>
        public static void InsertProjectile(int id, Vector2D loc, Vector2D dir, Vector2D vel, Ship ship)
        {
            if (ship.hp > 0)
            {
                Projectile p = new Projectile(projectileCounter, loc, dir, vel, true, id);

                TheWorld.AddProjectile(ship.ID, p);
                projectileCounter++; //increment so that each projectile has a unique ID
            }
        }

        /// <summary>
        /// Update called every frame based on framerate timer in main()
        /// Updates world and sends world to every client on every frame.
        /// </summary>
        public static void Update()
        {
            UpdateWorld();
            SendWorld();
        }

        /// <summary>
        /// Updates the state of the world.
        /// </summary>
        public static void UpdateWorld()
        {
            lock (TheWorld)
            {
                ProcessCommands();
                ProcessProjectiles();
                ProcessShips();

                //Process star movement if fancy game mode is on
                if ((string)gameSettings["FancyGame"] == "Yes" || (string)gameSettings["FancyGame"] == "yes")
                {
                    ProcessStars();
                }
            }
        }

        /// <summary>
        /// Commands from client are stored in each instance of the client class, unique to each player.
        /// On every frame, if a client command is set to true, process the command and reset flag.
        /// </summary>
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
                        //Checks fireRateCounter to prevent ship from firing too fast (LASERS ARE BAD!!)
                        if (c.fire == true && s.fireRateCounter == Convert.ToInt32(gameSettings["FramesPerShot"]))
                        {
                            Vector2D temp = new Vector2D(s.loc);
                            Vector2D projVel = new Vector2D(s.dir * 15);
                            Vector2D startPos = new Vector2D(s.loc + (s.dir * 20));
                            InsertProjectile(c.ID, startPos, s.dir, projVel, s);
                            c.fire = false;
                            s.fireRateCounter = -1; //Reset fireRateCounter after a ship fires
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Moves projectile at constant speed based on direction of ship upon initial fire
        /// </summary>
        public static void ProcessProjectiles()
        {
            int worldSize = Convert.ToInt32(gameSettings["UniverseSize"]) - 1;
            List<Projectile> projToDelete = new List<Projectile>();

            foreach (Projectile p in TheWorld.GetProjectiles())
            {
                //If projectile is dead, add it to list of projectiles to delete.
                if (p.alive == false)
                {
                    projToDelete.Add(p);
                }
                else
                {
                    Vector2D newLoc = new Vector2D(p.loc + p.vel);
                    p.SetLoc(newLoc);
                    //Kills projectiles that reach the boundary of the world.
                    if (p.loc.GetX() >= worldSize / 2 || p.loc.GetY() >= worldSize / 2 || p.loc.GetX() <= -worldSize / 2 || p.loc.GetY() <= -worldSize / 2)
                    {
                        p.SetAlive(false);
                    }
                    foreach (Ship ship in TheWorld.GetShipsAll())
                    {
                        //Collision detection check
                        if ((ship.loc - p.loc).Length() <= 20)
                        {
                            //If ship is alive and not the same as the owner of the projectile (the ship that fired it)
                            if (ship.hp > 0 && ship.ID != p.owner)
                            {
                                //decrements health by one
                                int newHP = ship.hp - 1;
                                ship.SetHp(newHP);
                                //if the ship is dead after removing 1 hp, increment score of ship that fired projectile
                                if (newHP == 0)
                                {
                                    int score = TheWorld.GetShipAtId(p.owner).score;
                                    TheWorld.GetShipAtId(p.owner).SetScore(score + 1);
                                }
                                //kill projectile after collision
                                p.SetAlive(false);
                            }
                            //start deathCounter (respawn counter) for dead ship
                            if (ship.hp == 0)
                            {
                                ship.deathCounter++;
                                
                            }

                        }
                    }
                    //kills projectile if it collides with a star
                    foreach (Star star in TheWorld.GetStars())
                    {
                        if ((star.loc - p.loc).Length() <= 35)
                        {
                            p.SetAlive(false);
                        }
                    }
                }
            }
            //deletes projectiles that are flagged to delete
            foreach (Projectile p in projToDelete)
            {
                TheWorld.RemoveProjectile(p.owner, p.ID);
            }
            projToDelete.Clear();
        }

        /// <summary>
        /// Moves stars in orbit for fancy game mode
        /// </summary>
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

        /// <summary>
        /// Updates ship based on gravity of star(s). Checks deathCounter to respawn if ship is ready to respawn.
        /// Increements fireRateCounter if ship is waiting to fire again.
        /// </summary>
        public static void ProcessShips()
        {
            int worldSize = Convert.ToInt32(gameSettings["UniverseSize"]) - 1;
            foreach (Ship ship in TheWorld.GetShipsAll())
            {
                //DeathCounter checks for ability to respawn if needed
                if (ship.deathCounter > 0 && ship.deathCounter < Convert.ToInt32(gameSettings["RespawnTime"]))
                {
                    ship.deathCounter++;
                }
                if (ship.deathCounter == Convert.ToInt32(gameSettings["RespawnTime"]))
                {
                    SpawnShip(ship);
                }
                //Increemnts fireRateCounter if ship has recently fired and is awaiting ability to fire again
                if (ship.fireRateCounter < Convert.ToInt32(gameSettings["FramesPerShot"]))
                {
                    ship.fireRateCounter++;
                }
                //Calculates acceleration due to gravity of all stars in the world.
                Vector2D totalAccel = new Vector2D(0, 0);
                foreach (Star star in TheWorld.GetStars())
                {
                    Vector2D grav = star.loc - ship.loc;
                    grav.Normalize();
                    grav = grav * star.mass;
                    totalAccel += grav;
                }
                //Caclulates acceleration due to thrust if ship is thrusting and adds it to totalAccel
                double enginePower = Convert.ToDouble(gameSettings["EnginePower"]);
                if (ship.thrust == true)
                {
                    Vector2D thrust = new Vector2D(ship.dir);
                    thrust = thrust * enginePower;
                    totalAccel += thrust;
                }
                //Calculates new velocity based on prev velocity and sum of forces
                Vector2D newVel = new Vector2D(ship.vel + totalAccel);
                if (newVel.Length() > 20)
                {
                    newVel.Normalize();
                    newVel = newVel * 20;
                }
                ship.SetVelocity(newVel);
                Vector2D newLoc = new Vector2D(ship.loc + ship.vel);
                ship.SetLoc(newLoc);

                //Checks for ship hitting world boundary, and 'teleports' ship to other side of map
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

                //Collission detection with star
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
        public static void ErrorHandler(string message)
        {
            Console.WriteLine(message);
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
