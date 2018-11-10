using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetworkController;

namespace Game
{
    public class GameController
    {
        private string UserName { get; set; }
        private string Host { get; set; }
        private int ID { get; set; }
        private int WorldSize;

        public delegate void InitializeWorldHandler(int WorldSize);
        public delegate void UpdateWorldHandler(IEnumerable<string> messages);

        public event InitializeWorldHandler WorldInitialized;
        public event UpdateWorldHandler WorldUpdated;

        private void FirstContact(Networking.SocketState ss)
        {
            ss._call = ReceiveStartup; // change delegate
            Networking.NetworkController.Send(this.UserName, ss); // send the name
            Networking.NetworkController.GetData(ss); // start the loop to get data. 

        }
        private void ReceiveStartup(Networking.SocketState ss)
        {
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
            // check the buffer to see if it's a complete message
            // store data as world size and game id
            // change the delegate to main data loop
            // get more data. 
            // register WorldInitialized event;
            
        }

        private void ReceiveWorld(Networking.SocketState ss)
        {
            // check buffer to see if it's complete message
            // if it isn't, get more data
            // if it is, parse data as objects
            // get more data. 
        }

        //private void ProcessMessage(Networking.SocketState ss)
        //{
        //    string totalData = ss.sb.ToString();
        //    string[] parts = Regex.Split(totalData, @"(?<=[\n])");

        //    // Loop until we have processed all messages.
        //    // We may have received more than one.

        //    foreach (string p in parts)
        //    {
        //        // Ignore empty strings added by the regex splitter
        //        if (p.Length == 0)
        //            continue;
        //        // The regex splitter will include the last string even if it doesn't end with a '\n',
        //        // So we need to ignore it if this happens. 
        //        if (p[p.Length - 1] != '\n')
        //            break;

        //        // Display the message
        //        // "messages" is the big message text box in the form.
        //        // We must use a MethodInvoker, because only the thread that created the GUI can modify it.
        //        this.Invoke(new MethodInvoker(
        //          () => messages.AppendText(p)));

        //        // Then remove it from the SocketState's growable buffer
        //        ss.sb.Remove(0, p.Length);
        //    }
        //}
        public void ConnectInitial(string Name, string Host)
        {
            this.UserName = Name;
            this.Host = Host;
            Networking.NetworkController.ConnectToServer(this.Host, FirstContact);
        }
    }
}
