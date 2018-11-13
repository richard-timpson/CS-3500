using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameModel
{   
    public class World
    {
        List<Ship> ShipsActive;
        List<Projectile> ProjectilesActive;
        List<Star> StarsActive;

        public World()
        {
            ShipsActive = new List<Ship>();
            ProjectilesActive = new List<Projectile>();
            StarsActive = new List<Star>();
        }

        public void UpdateWorld(IEnumerable<string> messages)
        {
            foreach (string s in messages)
            {
                Ship temp;
                Star tempStar;
                Projectile tempProj;
                if (s.Length >= 4 && s[2] == 's' && s[3] == 'h')
                {
                    temp = JsonConvert.DeserializeObject<Ship>(s);
                    if (!ShipsActive.Any(item => item.ID == temp.ID && temp.GetHP() != 0))
                    {
                        ShipsActive.Add(temp);
                    }
                    else if (ShipsActive.Any(item => item.ID == temp.ID))
                    {
                        ShipsActive.RemoveAll(item => item.ID == temp.ID);
                        ShipsActive.Add(temp);
                    }
                    else if (temp.GetHP() == 0)
                    {
                        ShipsActive.RemoveAll(item => item.ID == temp.ID);
                    }
                }
                if (s.Length >= 4 && s[2] == 's' && s[3] == 't')
                {
                    tempStar = JsonConvert.DeserializeObject<Star>(s);
                    if (!StarsActive.Any(item => item.GetID() == tempStar.GetID()))
                    {
                        StarsActive.Add(tempStar);
                    }
                }
                if (s.Length >= 4 && s[2] == 'p')
                {
                    tempProj = JsonConvert.DeserializeObject<Projectile>(s);
                    if (!ProjectilesActive.Any(item => item.GetID() == tempProj.GetID()))
                    {
                        ProjectilesActive.Add(tempProj);
                    }
                    else if (ProjectilesActive.Any(item => item.GetID() == tempProj.GetID()))
                    {
                        ProjectilesActive.RemoveAll(item => item.GetID() == tempProj.GetID());
                        ProjectilesActive.Add(tempProj);
                    }
                    else if (tempProj.GetAlive() == false)
                    {
                        ProjectilesActive.RemoveAll(item => item.GetID() == tempProj.GetID());
                    }
                }


            }
        }

        public IEnumerable<Ship> GetShips()
        {
            foreach (Ship s in ShipsActive)
                yield return s;
        }

        public IEnumerable<Projectile> GetProjectiles()
        {
            foreach (Projectile p in ProjectilesActive)
                yield return p;
        }

        public IEnumerable<Star> GetStars()
        {
            foreach (Star s in StarsActive)
                yield return s;
        }
    }
}
