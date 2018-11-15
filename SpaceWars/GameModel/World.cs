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
        HashSet<Projectile> ProjectilesActive;
        List<Star> StarsActive;

        public World()
        {
            ShipsActive = new List<Ship>();
            ProjectilesActive = new HashSet<Projectile>();
            StarsActive = new List<Star>();
        }

        

        public void AddShip(Ship s)
        {
            this.ShipsActive.Add(s);
        }

        public void RemoveShip(int ID)
        {
            this.ShipsActive.RemoveAll((item) => item.ID == ID);
        }

        public IEnumerable<Ship> GetShips()
        {
            foreach (Ship s in ShipsActive)
                yield return s;
        }


        public void AddProjectile(Projectile p)
        {
            this.ProjectilesActive.Add(p);
        }

        public void RemoveProjectile(int ID)
        {
            this.ProjectilesActive.RemoveWhere((item) => item.ID == ID);

        }

        public IEnumerable<Projectile> GetProjectiles()
        {
            foreach (Projectile p in ProjectilesActive)
                yield return p;
        }

        public void AddStar(Star s)
        {
            this.StarsActive.Add(s);
        }

        public void RemoveStar(int ID)
        {
            this.StarsActive.RemoveAll((item) => item.ID == ID);
        }

        public IEnumerable<Star> GetStars()
        {
            foreach (Star s in StarsActive)
                yield return s;
        }
    }
}
