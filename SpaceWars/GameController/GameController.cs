using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetworkController;
using GameModel;
using Newtonsoft.Json;

namespace Game
{
    public class GameController
    {
        private string UserName { get; set; }
        private string Host { get; set; }
        private int ID { get; set; }
        //private int WorldSize;
        public World theWorld { get; private set; }

        public delegate void InitializeWorldHandler(int WorldSize);
        public delegate void UpdateWorldHandler();

        public event InitializeWorldHandler WorldInitialized;
        public event UpdateWorldHandler WorldUpdated;

        public GameController()
        {
            this.theWorld = new World();
        }

        private void FirstContact(Networking.SocketState ss)
        {
            Console.WriteLine("Calling FirstContact function");
            ss._call = ReceiveStartup; // change delegate
            string protocolUserName = this.UserName + "\n";
            Networking.NetworkController.Send(protocolUserName, ss); // send the name
            Networking.NetworkController.GetData(ss); // start the loop to get data. 

        }
        private void ReceiveStartup(Networking.SocketState ss)
        {
            Console.WriteLine("Calling ReceiveStartup Function");
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            foreach (string s in parts)
            {
                if (s.Length == 0)
                {
                    continue;
                }
                if (s[s.Length - 1] != '\n')
                {
                    Networking.NetworkController.GetData(ss);
                    break;
                }
            }
            ss.sb.Clear();
            this.ID = Convert.ToInt32(parts[0]);
            int WorldSize = Convert.ToInt32(parts[1]);
            WorldInitialized(WorldSize);
            ss._call = ReceiveWorld;
            Networking.NetworkController.GetData(ss);

        }

        private void ReceiveWorld(Networking.SocketState ss)
        {
            Console.WriteLine("Calling Receive World Function");
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            foreach (string s in parts)
            {
                if (s.Length == 0)
                {
                    continue;
                }
                if (s[s.Length - 1] != '\n')
                {
                    Networking.NetworkController.GetData(ss);
                    break;
                }
                
            }
            ss.sb.Clear();
            lock (this.theWorld)
            {
                ProcessObject(parts);
            }
            WorldUpdated();
            Networking.NetworkController.GetData(ss);



        }

        public void ConnectInitial(string Name, string Host)
        {
            this.UserName = Name;
            this.Host = Host;
            Networking.NetworkController.ConnectToServer(this.Host, FirstContact);
        }


        private void ProcessObject(string[] parts)
        {
            foreach (string s in parts)
            {
                Ship temp;
                Star tempStar;
                Projectile tempProj;
                Console.WriteLine(s);
                // if the object is a ship
                if (s.Length >= 4 && s[2] == 's' && s[3] == 'h')
                {
                    temp = JsonConvert.DeserializeObject<Ship>(s);
                    // if the ship isn't in the current list, and it is not dead
                    if (!theWorld.GetShips().Any(item => item.ID == temp.ID && temp.hp != 0))
                    {
                        theWorld.AddShip(temp);
                    }
                    // if the ship is in the current list, and it is not dead
                    else if (theWorld.GetShips().Any(item => item.ID == temp.ID) && temp.hp != 0)
                    {
                        // remove old ship
                        theWorld.RemoveShip(temp.ID);
                        // add new ship
                        theWorld.AddShip(temp);
                    }
                    // if the ship is dead, remove it
                    else if (temp.hp == 0)
                    {
                        // need to tell view to explode 

                        theWorld.RemoveShip(temp.ID);
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
                    else if (theWorld.GetProjectiles().Any(item => item.ID == tempProj.ID) && tempProj.alive)
                    {
                        // remove the old projectile
                        theWorld.RemoveProjectile(tempProj.ID);
                        // add the new one
                        theWorld.AddProjectile(tempProj);
                    }
                    // if projectile is dead
                    else if (tempProj.alive == false)
                    {
                        theWorld.RemoveProjectile(tempProj.ID);
                    }
                }
            }
        }
    }
}
