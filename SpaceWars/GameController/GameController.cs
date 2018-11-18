using System;
using System.Diagnostics;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using NetworkController;
using GameModel;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Game
{
    public class GameController
    {
        private string UserName { get; set; }
        private string Host { get; set; }
        private int ID { get; set; }

        private bool Connected { get; set; }
        public World theWorld { get; private set; }

        public delegate void InitializeWorldHandler(int WorldSize);
        public delegate void UpdateWorldHandler();
        public delegate void SendMessageHandler(Networking.SocketState ss);

        public event InitializeWorldHandler WorldInitialized;
        public event UpdateWorldHandler WorldUpdated;
        public event SendMessageHandler SendMessage;

        

        /// <summary>
        /// Constructor the GameController. Initiallizes the world.
        /// </summary>
        public GameController()
        {
            this.theWorld = new World();
            Networking.NetworkController.Error += StopConnection;

        }

        /// <summary>
        /// Method for making first contact with server
        /// </summary>
        /// <param name="ss"></param>
        private void FirstContact(Networking.SocketState ss)
        {
            ss._call = ReceiveStartup; // change delegate
            string protocolUserName = this.UserName + "\n";
            try
            {
                Networking.NetworkController.Send(protocolUserName, ss); // send the name
                Networking.NetworkController.GetData(ss); // start the loop to get data. 
                Connected = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Receives unique ID and worldSize from server and starts the callMe delegate loop to
        /// continually receive data from server.
        /// </summary>
        /// <param name="ss"></param>
        private void ReceiveStartup(Networking.SocketState ss)
        {
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            //loop to find incomplete messages 
            foreach (string s in parts)
            {
                if (s.Length == 0)
                {
                    continue;
                }
                if (s[s.Length - 1] != '\n')
                {
                    try
                    {
                        Networking.NetworkController.GetData(ss);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        Connected = false;
                    }
                    break;
                }
            }
            ss.sb.Clear(); //clear stringbuilder
            this.ID = Convert.ToInt32(parts[0]); //set ID to Id sent from server
            int WorldSize = Convert.ToInt32(parts[1]); //set worldsize to worldsize sent from server
            WorldInitialized(WorldSize); //initialize world event
            ss._call = ReceiveWorld; //set the callMe delegate to ReceiveWorld
            try
            {
                Networking.NetworkController.GetData(ss); //get data from server
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Connected = false;
            }
            TriggerTimer(ss); //enable user key input to move ship and fire

        }

        private void ReceiveWorld(Networking.SocketState ss)
        {
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            string temp = "";

            //loop to process objects only if complete object. If message contains an
            //incomplete object, then the stringbuilder is cleared and the partial
            //is then appended to the stringbuilder.
            foreach (string s in parts)
            {
                if (s.Length == 0)
                {
                    continue;
                }
                if (s[s.Length - 1] != '\n')
                {
                    temp = s;
                    break;
                }
                lock (this.theWorld)
                {
                    ProcessObject(s);
                }
            }
            ss.sb.Clear();
            ss.sb.Append(temp);
            try
            {
                Networking.NetworkController.GetData(ss);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Connected = false;

            }
            WorldUpdated(); //update world event

        }

        /// <summary>
        /// Initiates initial connection with server.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Host"></param>
        public void ConnectInitial(string Name, string Host)
        {
            this.UserName = Name;
            this.Host = Host;
            try
            {
                Networking.NetworkController.ConnectToServer(this.Host, FirstContact);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }
        }

       


        /// <summary>
        /// Updates the world by updating the lists that contain the objects of the world.
        /// </summary>
        /// <param name="s"></param>
        private void ProcessObject(string s)
        {
            Ship temp;
            Star tempStar;
            Projectile tempProj;
            object[] tempArr = new object[2];
            // if the object is a ship
            if (s.Length >= 4 && s[2] == 's' && s[3] == 'h')
            {
                temp = JsonConvert.DeserializeObject<Ship>(s);

                //logic for explosion
                if (theWorld.GetShipsActive().Any<Ship>(x => x.ID == temp.ID) && temp.hp == 0)
                {
                    Explosion exp = new Explosion(temp);
                    theWorld.AddExplosion(exp);
                }
                if (theWorld.GetExplosions().Any(item => item.GetCount() == 109 && item.GetID() == temp.ID))
                {
                    theWorld.RemoveExplosion(temp.ID);
                }

                // logic for active ships

                // if the ship isn't in the current list, and it is not dead
                if (!theWorld.GetShipsActive().Any(item => item.ID == temp.ID && temp.hp != 0))
                {
                    theWorld.AddShipActive(temp);
                    Console.WriteLine("Adding ship: " + temp.name);
                }
                // if the ship is in the current list, and it is not dead
                if (theWorld.GetShipsActive().Any(item => item.ID == temp.ID) && temp.hp != 0)
                {
                    // remove old ship
                    theWorld.RemoveShipActive(temp.ID);
                    // add new ship
                    theWorld.AddShipActive(temp);
                    Console.WriteLine("Adding ship: " + temp.name);

                }
                // if the ship is dead, remove it
                if (theWorld.GetShipsActive().Any(item => item.ID == temp.ID) && temp.hp == 0)
                {
                    // need to tell view to explode 

                    theWorld.RemoveShipActive(temp.ID);
                }



                // logic for all ships

                // if the ship is not in the list, add it
                if (!theWorld.GetShips().Any(item => item.ID == temp.ID))
                {
                    theWorld.AddShip(temp);
                }
                // if the ship is in the list, update it
                if (theWorld.GetShips().Any(item => item.ID == temp.ID))
                {
                    theWorld.RemoveShip(temp.ID);
                    theWorld.AddShip(temp);
                }





            }
            // if the object is a start
            if (s.Length >= 4 && s[2] == 's' && s[3] == 't')
            {
                tempStar = JsonConvert.DeserializeObject<Star>(s);
                // if star does not exist
                if (!theWorld.GetStars().Any(item => item.ID == tempStar.ID))
                {
                    // add star
                    theWorld.AddStar(tempStar);
                }
            }
            // if the object is a projectile
            if (s.Length >= 4 && s[2] == 'p')
            {
                tempProj = JsonConvert.DeserializeObject<Projectile>(s);
                // if projectile does not exists
                if (!theWorld.GetProjectiles().Any(item => item.ID == tempProj.ID))
                {
                    // add projectile
                    theWorld.AddProjectile(tempProj);
                }
                // if projectile exists and is alive
                if (theWorld.GetProjectiles().Any(item => item.ID == tempProj.ID) && tempProj.alive)
                {
                    // remove the old projectile
                    theWorld.RemoveProjectile(tempProj.ID);
                    // add the new one
                    theWorld.AddProjectile(tempProj);
                }
                // if projectile is dead
                if (tempProj.alive == false)
                {
                    theWorld.RemoveProjectile(tempProj.ID);
                }
            }

        }

        /// <summary>
        /// Sends the user key inputs every 15 ms.
        /// </summary>
        /// <param name="ss"></param>
        private void TriggerTimer(Networking.SocketState ss)
        {
            // every 15 ms, trigger the SendKey event

            while (Connected)
            {

                Stopwatch watch = new Stopwatch();
                watch.Start();
                while( watch.ElapsedMilliseconds < 16) { }
                SendMessage(ss);
                watch.Reset();
                //System.Timers.Timer timer = new System.Timers.Timer(15);
                //timer.Elapsed += delegate { TriggerSendMessage(ss)};
                //timer.AutoReset = true;
                //timer.Enabled = true;
            }
        }
        private void TriggerSendMessage(Networking.SocketState ss)
        {
                SendMessage(ss);
        }

        /// <summary>
        /// 
        /// Sends the message to the server with keyinputs.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ss"></param>
        public void SendControls(string message, Networking.SocketState ss)
        {
            try
            {
                Networking.NetworkController.Send(message, ss);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Connected = false;
            }
        }
        private void StopConnection(string message)
        {
            Connected = false;


            //create a copy of world objects to iterate thru and delete everything upon loss of connection
            List<Ship> ShipsCopy = new List<Ship>();
            List<Star> StarsCopy = new List<Star>();
            List<Projectile> ProjectilesCopy = new List<Projectile>();
            List<Explosion> ExplosionsCopy = new List<Explosion>();
            IEnumerable<Ship> Ships = DeepCopyShip(theWorld.GetShipsActive(), ShipsCopy);
            IEnumerable<Star> Stars = DeepCopyStar(theWorld.GetStars(), StarsCopy);
            IEnumerable<Projectile> Projectiles = DeepCopyProj(theWorld.GetProjectiles(), ProjectilesCopy);
            IEnumerable<Explosion> Explosions = DeepCopyExp(theWorld.GetExplosions(), ExplosionsCopy);

            lock (theWorld)
            {
                //remove objects from the world on disconnect
                foreach (Ship s in Ships)
                {
                    theWorld.RemoveShipActive(s.ID);
                }
                foreach (Projectile p in Projectiles)
                {
                    theWorld.RemoveProjectile(p.ID);
                }
                foreach (Star s in Stars)
                {
                    theWorld.RemoveStar(s.ID);
                }
                foreach (Explosion exp in Explosions)
                {
                    theWorld.RemoveExplosion(exp.GetID());
                }
            }
        }

        /// <summary>
        /// helper method to create deep copy
        /// </summary>
        /// <param name="ships"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public IEnumerable<Ship> DeepCopyShip(IEnumerable<Ship> ships, List<Ship> copy)
        {
            foreach (Ship s in ships)
            {
                Ship temp = new Ship();
                temp = s;
                copy.Add(temp);
            }
            foreach (Ship s in copy)
                yield return s;
        }

        /// <summary>
        /// helper method to create deep copy
        /// </summary>
        /// <param name="stars"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public IEnumerable<Star> DeepCopyStar(IEnumerable<Star> stars, List<Star> copy)
        {
            foreach (Star s in stars)
            {
                Star temp = new Star();
                temp = s;
                copy.Add(temp);
            }
            foreach (Star s in copy)
                yield return s;
        }

        /// <summary>
        /// helper method to create deep copy
        /// </summary>
        /// <param name="projectiles"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public IEnumerable<Projectile> DeepCopyProj(IEnumerable<Projectile> projectiles, List<Projectile> copy)
        {
            foreach (Projectile p in projectiles)
            {
                Projectile temp = new Projectile();
                temp = p;
                copy.Add(temp);
            }
            foreach (Projectile p in copy)
                yield return p;
        }

        /// <summary>
        /// helper method to create deep copy
        /// </summary>
        /// <param name="explosions"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public IEnumerable<Explosion> DeepCopyExp(IEnumerable<Explosion> explosions, List<Explosion> copy)
        {
            foreach (Explosion e in explosions)
            {
                Ship s = new Ship();
                Explosion temp = new Explosion(s);
                temp = e;
                copy.Add(temp);
            }
            foreach (Explosion e in copy)
                yield return e;
        }


    }

}
