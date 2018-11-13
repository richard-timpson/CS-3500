using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetworkController;
using GameModel;

namespace Game
{
    public class GameController
    {
        private string UserName { get; set; }
        private string Host { get; set; }
        private int ID { get; set; }
        private int WorldSize;
        private World theWorld;

        public delegate void InitializeWorldHandler(int WorldSize);
        public delegate void UpdateWorldHandler(IEnumerable<string> messages);

        public event InitializeWorldHandler WorldInitialized;
        public event UpdateWorldHandler WorldUpdated;

        public GameController(World theWorld)
        {
            this.theWorld = theWorld;
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
            foreach(string s in parts)
            {
                if (s.Length == 0)
                {
                    continue;
                }
                if (s[s.Length -1] != '\n')
                {
                    Networking.NetworkController.GetData(ss);
                    break;
                }
            }
            this.ID = Convert.ToInt32(parts[0]);
            int WorldSize = Convert.ToInt32(parts[1]);
            WorldInitialized(WorldSize);
            ss._call = ReceiveWorld;
            
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
            WorldUpdated(parts);
            UpdateTheWorld(parts);

            
        }
        
        public void ConnectInitial(string Name, string Host)
        {
            this.UserName = Name;
            this.Host = Host;
            Networking.NetworkController.ConnectToServer(this.Host, FirstContact);
        }
        
        public void UpdateTheWorld(IEnumerable<string> messages)
        {
            this.theWorld.UpdateWorld(messages);
        }
    }
}
