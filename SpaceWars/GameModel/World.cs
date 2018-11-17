﻿using System;
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


        /// <summary>
        /// Constructor for the World.
        /// </summary>
        public World()
        {
            ShipsActive = new List<Ship>();
            ShipsTotal = new List<Ship>();
            ProjectilesActive = new HashSet<Projectile>();
            StarsActive = new List<Star>();
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
        public IEnumerable<Ship> GetShips()
        {
            foreach (Ship s in ShipsTotal)
                yield return s;
        }

        /// <summary>
        /// Adds ship to list of total ships in game.
        /// </summary>
        /// <param name="s"></param>
        public void AddShip(Ship s)
        {
            this.ShipsTotal.Add(s);
        }

        /// <summary>
        /// removes ship from list of total ships in game.
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveShip(int ID)
        {
            this.ShipsTotal.RemoveAll((item) => item.ID == ID);
        }

        /// <summary>
        /// Adds projectile to list of projectiles active.
        /// </summary>
        /// <param name="p"></param>
        public void AddProjectile(Projectile p)
        {
            this.ProjectilesActive.Add(p);
        }

        /// <summary>
        /// Removes projectile from list of projectiles active.
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveProjectile(int ID)
        {
            this.ProjectilesActive.RemoveWhere((item) => item.ID == ID);

        }

        /// <summary>
        /// Retrives an enumerable of active projectiles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Projectile> GetProjectiles()
        {
            foreach (Projectile p in ProjectilesActive)
                yield return p;
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