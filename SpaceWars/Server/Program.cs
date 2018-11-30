using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;
using System.Xml;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        public Dictionary<string, string> XmlSettingsReader()
        {
            Dictionary<string, string> gameSettings = new Dictionary<string, string>();
            bool openfile = true;
            string path = Resources.Resource1.settings;
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
                                    bool starActive = true;
                                    while (starActive)
                                    {
                                        if (reader.Read())
                                        {
                                            if (reader.IsStartElement())
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
