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

namespace Server
{
    class Program
    {
        private static Dictionary<int, string> ClientConnections { get; set; }
        private static int IdCounter { get; set; }

        private static Dictionary<string, object> gameSettings { get; set; }

        private static World TheWorld { get; set; }

        static void Main(string[] args)
        {
            ClientConnections = new Dictionary<int, string>();
            IdCounter = 0;
            gameSettings = XmlSettingsReader();
            TheWorld = new World();
            InsertStars();
            Networking.NetworkController.ServerAwaitingClientLoop(HandleNewClient);
            Console.Read();
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
            string[] parts = totalData.Split('\n');
            ClientConnections.Add(IdCounter, parts[0]);
            foreach (KeyValuePair<int, string> connection in ClientConnections)
            {
                Console.WriteLine(connection);
            }
            string startupInfo = IdCounter + "\n" + gameSettings["UniverseSize"] + "\n";
            IdCounter++;
            Networking.NetworkController.Send(startupInfo, ss);
        }
        private static void ReceiveCommand(Networking.SocketState ss )
        {
            Console.WriteLine("Receiving commands");
            string totalData = ss.sb.ToString();
            string[] parts = totalData.Split();
            Console.WriteLine(parts);
        }
        private static  Dictionary<string, object> XmlSettingsReader()
        {
            List<string[]> stars = new List<string[]>();
            Dictionary<string, object> gameSettings = new Dictionary<string, object>();
            gameSettings.Add("stars", stars);
            bool openfile = true;
            string path = "..\\..\\..\\Resources\\settings.xml";
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                using (XmlReader reader = XmlReader.Create(path, settings))
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
                                    gameSettings.Add("UniverseSize", reader.Value);
                                }
                                if (reader.Name == "MSPerFrame") 
                                {
                                    reader.Read();
                                    gameSettings.Add("MSPerFrame", reader.Value);
                                }
                                if (reader.Name == "FramesPerShot")
                                {
                                    reader.Read();
                                    gameSettings.Add("FramesPerShot", reader.Value);
                                }
                                if (reader.Name == "RespawnRate")
                                {
                                    reader.Read();
                                    gameSettings.Add("RespawnRate", reader.Value);
                                }
                                if (reader.Name == "Star")
                                {
                                    string[] star = new string[] { "", "", "" };
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
                                                    star[0] = reader.Value;
                                                }
                                                if (reader.Name == "y")
                                                {
                                                    reader.Read();
                                                    star[1] = reader.Value;
                                                }
                                                if (reader.Name == "mass")
                                                {
                                                    reader.Read();
                                                    star[2] = reader.Value;
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
                                        else
                                        {
                                            starActive = false;
                                        }
                                    }
                                    List<string[]> temp = new List<string[]>();
                                    temp = (List<string[]>)(gameSettings["stars"]);
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
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                return null;
            }
        }

        private static void InsertStars()
        {
            List<string[]> temp = new List<string[]>();
            temp = (List<string[]>)(gameSettings["stars"]);
            int StarIdCounter = 0;
            foreach (string[] s in temp)
            {
                Star star = new Star();
                star.SetID(StarIdCounter);
                Vector2D loc = new Vector2D(Convert.ToDouble(s[0]), Convert.ToDouble(s[1]));
                star.SetLoc(loc);
                star.SetMass(Convert.ToDouble(s[2]));
                StarIdCounter++;
            }
        }

        private static void InsertShip()
        {
            Ship s = new Ship();
            int id = IdCounter;
            s.SetID(id);

            

            IdCounter++;
            
        }
    }
}
