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
        private Dictionary<int, Ship> ShipsTotal;
        private Dictionary<int,Dictionary<int, Projectile>> ProjectilesActive;
        private List<Star> StarsActive;
        private List<Explosion> Explosions;

        /// <summary>
        /// Constructor for the World.
        /// </summary>
        public World()
        {
            ShipsActive = new List<Ship>();
            ShipsTotal = new Dictionary<int, Ship>();
            ProjectilesActive = new Dictionary<int, Dictionary<int, Projectile>>();
            StarsActive = new List<Star>();
            Explosions = new List<Explosion>();
        }

        
        /// <summary>
        /// Adds ship to list of ships that are alive.
        /// </summary>
        /// <param name="s"></param>
        public void AddShipActive(Ship s)
        {
            this.ShipsActive.Add(s);
        }

        /// <summary>
        /// Removes ship from list of ships that are alive.
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveShipActive(int ID)
        {
            this.ShipsActive.RemoveAll((item) => item.ID == ID);
        }

        /// <summary>
        /// Retrives an enumerable of ships that are alive.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Ship> GetShipsActive()
        {
            foreach (Ship s in ShipsActive)
                yield return s;
        }

        /// <summary>
        /// Retrives an enumerable of total ships in game.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Ship> GetShipsAll()
        {
            foreach (KeyValuePair<int, Ship> ship in ShipsTotal)
            {
                yield return ship.Value;
            }
        }
        public Ship GetShipAtId(int id)
        {
            return this.ShipsTotal[id];
        }

        /// <summary>
        /// Adds ship to list of total ships in game.
        /// </summary>
        /// <param name="s"></param>
        public void AddShipAll(Ship s)
        {
            this.ShipsTotal.Add(s.ID, s);
        }

        /// <summary>
        /// removes ship from list of total ships in game.
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveShipAll(int ID)
        {
            this.ShipsTotal.Remove(ID);
        }

        /// <summary>
        /// Adds explosion to the world
        /// </summary>
        /// <param name="e"></param>
        public void AddExplosion(Explosion e)
        {
            this.Explosions.Add(e);
        }

        /// <summary>
        /// removes ship from list of total ships in game.
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveExplosion(int ID)
        {
            this.Explosions.RemoveAll((item) => item.GetID() == ID);
        }

        /// <summary>
        /// retrieves all active explosions in the world.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Explosion> GetExplosions()
        {
            foreach (Explosion e in Explosions)
                yield return e;
        }

        /// <summary>
        /// Adds projectile to list of projectiles active.
        /// </summary>
        /// <param name="p"></param>
        public void AddProjectile(int ownerID, Projectile p)
        {
            if (this.ProjectilesActive.ContainsKey(ownerID))
            {
                this.ProjectilesActive[ownerID].Add(p.ID,p);
            }
            else
            {
                Dictionary<int, Projectile> projectiles = new Dictionary<int, Projectile>();
                projectiles.Add(p.ID, p);
                this.ProjectilesActive.Add(ownerID, projectiles);
            }
        }

        /// <summary>
        /// Removes projectile from list of projectiles active.
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveProjectile(int ownerID, int projectileID)
        {
            if (this.ProjectilesActive.ContainsKey(ownerID))
            {
                this.ProjectilesActive[ownerID].Remove(projectileID);
            }

        }

        /// <summary>
        /// Retrives an enumerable of active projectiles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Projectile> GetProjectiles()
        {
            foreach (KeyValuePair<int, Dictionary<int, Projectile>> dictionary in this.ProjectilesActive)
            {
                foreach(KeyValuePair<int, Projectile> projectile in dictionary.Value)
                {
                    yield return projectile.Value;
                }
            }
        }

        /// <summary>
        /// Adds star to list of active stars in game.
        /// </summary>
        /// <param name="s"></param>
        public void AddStar(Star s)
        {
            this.StarsActive.Add(s);
        }

        /// <summary>
        /// Removes star from list of active stars in game.
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveStar(int ID)
        {
            this.StarsActive.RemoveAll((item) => item.ID == ID);
        }

        /// <summary>
        /// returns an enumerable of stars active in game.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Star> GetStars()
        {
            foreach (Star s in StarsActive)
                yield return s;
        }
    }
}
