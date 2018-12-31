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
        // List that keeps track of current client connections. 
        public static Dictionary<int, Client> ClientConnections { get; set; }

        // a dictionary that holds all of the game settings and are specified in xml reader
        public static Dictionary<string, object> gameSettings { get; set; }

        // holding the state of the world
        public static World TheWorld { get; set; }

        // used to count the amount of total projectiles in the game, so we can allocate id. 
        public static int projectileCounter { get; private set; }

        static void Main(string[] args)
        {
            // registering event handlers for error handling
            Networking.NetworkController.DisconnectError += DisconnectClientHandler;

            ClientConnections = new Dictionary<int, Client>();

            // setting the game settings from xml file
            string settingsFilePath = "settings.xml";
            gameSettings = XmlSettingsReader(settingsFilePath);

            // initializing world. 
            TheWorld = new World();

            // the initial star insert, metadata comes from gameSettings
            InsertStars();
            projectileCounter = 0;

            // starting main networking loop
            Networking.NetworkController.ServerAwaitingClientLoop(HandleNewClient, 0);

            // starting main server loop
            int frameRate = (int)gameSettings["MSPerFrame"];
            Stopwatch watch = new Stopwatch();
            while (true)
            {
                watch.Start();
                while (watch.ElapsedMilliseconds < frameRate) { }
                Update();
                watch.Reset();
            }


        }

        /// <summary>
        /// Sets callback to receive name and requests data from the client.
        /// </summary>
        /// <param name="ss"></param>
        private static void HandleNewClient(Networking.SocketState ss)
        {
            ss._call = ReceiveName;
            Networking.NetworkController.GetData(ss);
        }

        /// <summary>
        /// Receives the name of the client after it's connection. 
        /// </summary>
        /// <param name="ss"></param>
        private static void ReceiveName(Networking.SocketState ss)
        {
            ss._call = ReceiveCommand;
            string totalData = ss.sb.ToString();
            string[] name = totalData.Split('\n');
            Client client = new Client(ss.ID, name[0], ss);
            // on a successful connection, add a client
            lock (TheWorld)
            {
                ClientConnections.Add(client.ID, client);
            }
            // send useful information back to client
            string startupInfo = ss.ID + "\n" + gameSettings["UniverseSize"] + "\n";
            InsertShip(ss.ID, name[0], 0);
            Networking.NetworkController.Send(startupInfo, ss);
            Networking.NetworkController.GetData(ss);
        }

        /// <summary>
        /// Parses messages sent from client for commands and sets bool flag for each individual client
        /// based on the command string.
        /// </summary>
        /// <param name="ss"></param>
        private static void ReceiveCommand(Networking.SocketState ss)
        {
            // getting the command from the specific socket connection
            string totalData = ss.sb.ToString();
            char[] commands = totalData.ToCharArray();

            lock (TheWorld)
            {
                Client client = ClientConnections[ss.ID];
                // settings the flags that will tell the world to change it's model on the next frame update
                foreach (char s in commands)
                {
                    if (s != '(' || s != ')')
                    {
                        switch (s)
                        {
                            case 'L':
                                // if right is not set, set the left. Otherwise, right will stay, and left won't
                                // this solves the ambiguity of trying to turn right and left at the same time
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
            ss.sb.Clear();
            // asking for more data to continue the event loop
            Networking.NetworkController.GetData(ss);
        }

        /// <summary>
        /// Reads settings file to load settings into the world.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The dictionary of gameSettings that the rest of the class uses</returns>
        public static Dictionary<string, object> XmlSettingsReader(string filePath)
        {
            // temporary storage for the stars
            List<double[]> stars = new List<double[]>();

            // the object that is going to be retured. 
            Dictionary<string, object> gameSettings = new Dictionary<string, object>();

            gameSettings.Add("stars", stars);

            // used to keep the file open
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
                                // most of the values are casted to integers or doubles to make casting later on easier. 
                                // otherwisze, the dictionary of objects is harder to work with. 
                                // Also, the Convert.ToInt32 function is for some reason expensive, 
                                // so we only want to do it one time. 
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
                                if (reader.Name == "MaxShipSpeed")
                                {
                                    reader.Read();
                                    gameSettings.Add("MaxShipSpeed", Convert.ToDouble(reader.Value));
                                }
                                if (reader.Name == "ProjectileSpeed")
                                {
                                    reader.Read();
                                    gameSettings.Add("ProjectileSpeed", Convert.ToDouble(reader.Value));
                                }
                                if (reader.Name == "ShipTurnRate")
                                {
                                    reader.Read();
                                    gameSettings.Add("ShipTurnRate", Convert.ToDouble(reader.Value));
                                }
                                if (reader.Name == "Star")
                                {

                                    double[] star = new double[] { 0, 0, 0 };
                                    // used to keep track of the star elements
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
                                                // if we hit the end element of the star, stop parsing the xml as a star. 
                                                if (reader.Name == "Star")
                                                {
                                                    starActive = false;
                                                }
                                            }
                                        }
                                    }
                                    // stores a list of double arrays in the dictionary for the stars
                                    List<double[]> tempStars = new List<double[]>();
                                    tempStars = (List<double[]>)(gameSettings["stars"]);
                                    tempStars.Add(star);
                                    gameSettings["stars"] = tempStars;
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
            // If we catch a format exception, throw. Used for unit testing
            catch (FormatException E)
            {
                Console.WriteLine(E.Message);
                throw E;
            }
            // otherwise throw any other exception. Used for unit testing. 
            catch (Exception E)
            {
                if (filePath == "settings.xml")
                {
                    string alternativeFilePath = "..\\..\\..\\Resources\\settings.xml";
                    return XmlSettingsReader(alternativeFilePath);
                }
                else
                {
                    Console.WriteLine(E.Message);
                    throw E;
                }
            }
        }

        /// <summary>
        /// Inserts star into the world's list of stars based on settings from the game settings xml file
        /// </summary>
        public static void InsertStars()
        {
            //If the fancy game mode is off, loads stars from the settings file
            if ((string)gameSettings["FancyGame"] != "Yes" && (string)gameSettings["FancyGame"] != "yes")
            {
                // getting the star information from the game settings
                List<double[]> tempStarList = new List<double[]>();
                tempStarList = (List<double[]>)(gameSettings["stars"]);

                // looping through each of the stars stored in game settings and creating them
                int StarIdCounter = 0;
                lock (TheWorld)
                {
                    foreach (double[] s in tempStarList)
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
                // the radius away from the center of the world. 
                double radius = (int)gameSettings["UniverseSize"] / 3.5;

                // creating 4 stars at specified locations away from the center
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
            Ship ship = new Ship();
            ship.SetID(id);
            ship.SetName(name);
            ship.SetScore(score);

            lock (TheWorld)
            {
                //Spawn ship randomizes location and sets direction
                SpawnShip(ship);
                //Adds ship to dictionary
                TheWorld.AddShipAll(ship);
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
        public static void InsertProjectile(Vector2D loc, Vector2D dir, Vector2D vel, Ship ship)
        {
            if (ship.hp > 0)
            {
                Projectile proj = new Projectile();
                proj.SetID(projectileCounter);
                proj.SetLoc(loc);
                proj.SetDir(dir);
                proj.SetVel(vel);
                proj.SetAlive(true);
                proj.SetOwner(ship.ID);
                TheWorld.AddProjectile(ship.ID, proj);

                //increment so that each projectile has a unique ID
                projectileCounter++;
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
            double shipTurnRate = (double)gameSettings["ShipTurnRate"];

            // Process the current flag in the correct way for every client. 
            foreach (KeyValuePair<int, Client> c in ClientConnections)
            {
                Client client = c.Value;
                Ship ship = TheWorld.GetShipAtId(client.ID);
                if (client.left == true)
                {
                    Vector2D tempDir = new Vector2D(ship.dir);
                    tempDir.Rotate(-shipTurnRate); // ship turn rate is specified as a game setting. 
                    ship.SetDir(tempDir);
                    client.left = false;
                }
                if (client.right == true)
                {
                    Vector2D tempDir = new Vector2D(ship.dir);
                    tempDir.Rotate(shipTurnRate);
                    ship.SetDir(tempDir);
                    client.right = false;
                }
                if (client.thrust == false)
                {
                    ship.SetThrust(false);
                }
                if (client.thrust == true)
                {
                    ship.SetThrust(true);
                    client.thrust = false;
                }

                // if we have waited long enough after previous fire, fire projectile
                if (client.fire == true && ship.fireRateCounter == (int)gameSettings["FramesPerShot"])
                {
                    // fire projectile
                    Vector2D projVel = new Vector2D(ship.dir * (double)gameSettings["ProjectileSpeed"]);
                    Vector2D startPos = new Vector2D(ship.loc + (ship.dir * 20));
                    InsertProjectile(startPos, ship.dir, projVel, ship);
                    client.fire = false;

                    //Reset fireRateCounter after a ship fires
                    ship.SetFireRateCounter(-1); 
                }
                // if we haven't waited long enough after previous fire
                else if (client.fire == true && ship.fireRateCounter < (int)gameSettings["FramesPerShot"]) 
                {
                    // increment the counter. 
                    ship.SetFireRateCounter(ship.fireRateCounter + 1);
                }

            }
        }

        /// <summary>
        /// Moves projectile at constant speed based on direction of ship upon initial fire
        /// </summary>
        public static void ProcessProjectiles()
        {
            int worldSize = (int)gameSettings["UniverseSize"] - 1;

            // keeping track of projectiles to delete, because we can't delete them as we loop through the list
            List<Projectile> projToDelete = new List<Projectile>();

            // Running a brute force search for collision detection
            foreach (Projectile proj in TheWorld.GetProjectiles())
            {
                //If projectile is dead, add it to list of projectiles to delete.
                if (proj.alive == false)
                {
                    projToDelete.Add(proj);
                }
                else
                {
                    // move the projectile
                    Vector2D newLoc = new Vector2D(proj.loc + proj.vel);
                    proj.SetLoc(newLoc);

                    // Kills projectiles that reach the boundary of the world.
                    if (proj.loc.GetX() >= worldSize / 2 || proj.loc.GetY() >= worldSize / 2 || proj.loc.GetX() <= -worldSize / 2 || proj.loc.GetY() <= -worldSize / 2)
                    {
                        proj.SetAlive(false);
                    }
                    foreach (Ship ship in TheWorld.GetShipsAll())
                    {
                        // Collision detection check
                        if ((ship.loc - proj.loc).Length() <= 20)
                        {
                            // If ship is alive and not the same as the owner of the projectile (the ship that fired it)
                            if (ship.hp > 0 && ship.ID != proj.owner)
                            {
                                // decrements health by one
                                int newHP = ship.hp - 1;
                                ship.SetHp(newHP);
                                // if the ship is dead after removing 1 hp, increment score of ship that fired projectile
                                if (newHP == 0)
                                {
                                    int score = TheWorld.GetShipAtId(proj.owner).score;
                                    TheWorld.GetShipAtId(proj.owner).SetScore(score + 1);
                                }
                                // kill projectile after collision
                                proj.SetAlive(false);
                            }
                            // start deathCounter (respawn counter) for dead ship
                            if (ship.hp == 0)
                            {
                                ship.SetDeathCounter(ship.deathCounter + 1);

                            }

                        }
                    }
                    // kills projectile if it collides with a star
                    foreach (Star star in TheWorld.GetStars())
                    {
                        if ((star.loc - proj.loc).Length() <= 35)
                        {
                            proj.SetAlive(false);
                        }
                    }
                }
            }
            // deletes projectiles that are flagged to delete
            foreach (Projectile proj in projToDelete)
            {
                TheWorld.RemoveProjectile(proj.owner, proj.ID);
            }
            projToDelete.Clear();
        }

        /// <summary>
        /// Moves stars in orbit for fancy game mode
        /// </summary>
        public static void ProcessStars()
        {
            foreach (Star star in TheWorld.GetStars())
            {
                Vector2D tempDir = new Vector2D(star.dir);
                Vector2D tempDir2 = new Vector2D(star.dir);
                tempDir.Rotate(-1);
                star.SetDir(tempDir);
                // rotates the star around the center of the world. 
                Vector2D tempLoc = new Vector2D(star.loc + (tempDir2 * Math.Tan(Math.PI / 180) * ((int)gameSettings["UniverseSize"] / 3.5)));
                star.SetLoc(tempLoc);
            }
        }

        /// <summary>
        /// Updates ship based on gravity of star(s). Checks deathCounter to respawn if ship is ready to respawn.
        /// Increements fireRateCounter if ship is waiting to fire again.
        /// </summary>
        public static void ProcessShips()
        {
            int worldSize = (int)gameSettings["UniverseSize"] - 1;
            foreach (Ship ship in TheWorld.GetShipsAll())
            {
                // If the ship is dead but hasn't waited long enough, increment the death counter
                if (ship.deathCounter > 0 && ship.deathCounter < (int)gameSettings["RespawnTime"])
                {
                    ship.SetDeathCounter(ship.deathCounter + 1);
                }
                // If the ship is dead but has waited long enough, spawn the ship. 
                if (ship.deathCounter == (int)gameSettings["RespawnTime"])
                {
                    SpawnShip(ship);
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
                double enginePower = (double)gameSettings["EnginePower"];
                if (ship.thrust == true)
                {
                    Vector2D thrust = new Vector2D(ship.dir);
                    thrust = thrust * enginePower;
                    totalAccel += thrust;
                }

                //Calculates new velocity based on prev velocity and sum of forces
                double maxShipSpeed = (double)gameSettings["MaxShipSpeed"];
                Vector2D newVel = new Vector2D(ship.vel + totalAccel);
                if (newVel.Length() > maxShipSpeed)
                {
                    newVel.Normalize();
                    newVel = newVel * maxShipSpeed;
                }
                ship.SetVelocity(newVel);
                // moves the ship based on velocity
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
                        ship.SetDeathCounter(ship.deathCounter + 1);
                    }
                }

            }
        }

        /// <summary>
        /// Randomize spawn location for ship
        /// </summary>
        /// <param name="ship"></param>
        public static void SpawnShip(Ship ship)
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
                int worldSize = (int)gameSettings["UniverseSize"] - 1;
                // finding possible random variables for the spawn location
                LocX = rand.Next(-(worldSize / 2), worldSize / 2);
                LocY = rand.Next(-(worldSize / 2), worldSize / 2);
                position = new Vector2D(LocX, LocY);
                
                // flags to determine if ship should spawn or not.     
                bool starSpawn = true; // set for spawning on top of stars
                bool shipSpawn = true; // set for spawning on top of other ships

                //checks to see if potential spawn location is too close to a star
                foreach (Star star in TheWorld.GetStars())
                {
                    if ((star.loc - position).Length() <= 50)
                    {
                        starSpawn = false;
                    }

                }

                //checks to see if potential spawn location is too close to a ship
                foreach (Ship shipLoop in TheWorld.GetShipsAll())
                {
                    if ((shipLoop.loc - position).Length() <= 50)
                    {
                        shipSpawn = false;
                    }

                }

                //If neither star or ship is hindering spawn, break out of loop
                if (starSpawn == true && shipSpawn == true)
                {
                    safeSpawn = true;
                }
            }

            ship.SetLoc(position);
            ship.SetHp((int)gameSettings["StartingHP"]);
            ship.SetThrust(false);
            Vector2D vel = new Vector2D(0, 0);
            ship.SetVelocity(vel);
            ship.SetDeathCounter(0);
            Vector2D Dir = new Vector2D(0, 1);
            ship.SetDir(Dir);
            ship.SetFireRateCounter((int)(gameSettings["FramesPerShot"]));
        }

        /// <summary>
        /// Serialize each object in the world and send to each client connection
        /// </summary>
        public static void SendWorld()
        {
            // string that is built for each frame
            StringBuilder jsonString = new StringBuilder();
            // loop through all of the object in the world to build the string. 
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
                foreach (Projectile proj in TheWorld.GetProjectiles())
                {
                    jsonString.Append(JsonConvert.SerializeObject(proj) + "\n");
                }
            }
            // loop through all of the clients to send the string to.
            lock (TheWorld)
            {
                foreach (KeyValuePair<int, Client> c in ClientConnections)
                {
                    Client client = c.Value;
                    if (client.ss.theSocket.Connected)
                    {
                        Networking.NetworkController.Send(jsonString.ToString(), client.ss);
                    }
                }
            }
        }


        /// <summary>
        /// Kills ship of client that disconnects and removes client from world
        /// </summary>
        /// <param name="ss"></param>
        public static void DisconnectClientHandler(Networking.SocketState ss, string message)
        {
            Console.WriteLine(message);
            Ship ship = TheWorld.GetShipAtId(ss.ID);
            ship.SetHp(0);
            SendWorld(); // make sure to send the world so that the client knows to terminate the ship. 
            lock (TheWorld)
            {

                // remove ship and client connection associated with that ship. 
                TheWorld.RemoveShipAll(ss.ID);
                ClientConnections.Remove(ss.ID);
            }
        }

    }

    /// <summary>
    /// Client class. Holds socketstate, command string and bool flags for client commands
    /// </summary>
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
