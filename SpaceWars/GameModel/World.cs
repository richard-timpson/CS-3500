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
        private List<Ship> ShipsActive;
        private List<Ship> ShipsTotal;
        private HashSet<Projectile> ProjectilesActive;
        private List<Star> StarsActive;
        public Dictionary<int, object[]> PlayerScores { get; private set; }
        public List<int> Players;
        public World()
        {
            ShipsActive = new List<Ship>();
            ShipsTotal = new List<Ship>();
            ProjectilesActive = new HashSet<Projectile>();
            StarsActive = new List<Star>();
            PlayerScores = new Dictionary<int, object[]>();
            Players = new List<int>();
        }

        

        public void AddShipActive(Ship s)
        {
            this.ShipsActive.Add(s);
        }

        public void RemoveShipActive(int ID)
        {
            this.ShipsActive.RemoveAll((item) => item.ID == ID);
        }

        public IEnumerable<Ship> GetShips()
        {
            foreach (Ship s in ShipsTotal)
                yield return s;
        }
        public void AddShip(Ship s)
        {
            this.ShipsTotal.Add(s);
        }

        public void RemoveShip(int ID)
        {
            this.ShipsTotal.RemoveAll((item) => item.ID == ID);
        }

        public IEnumerable<Ship> GetShipsActive()
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
