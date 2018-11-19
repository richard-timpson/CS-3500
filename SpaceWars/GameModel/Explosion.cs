using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;

namespace GameModel
{
    public class Explosion
    {
        private int ID;
        private Vector.Vector2D loc;
        private int count = 0;

        /// <summary>
        /// Conctructor for explosions.
        /// </summary>
        /// <param name="s"></param>
        public Explosion(Ship s)
        {
            this.ID = s.ID;
            this.loc = s.loc;
        }

        /// <summary>
        /// Increments count once per frame to simulate animation
        /// </summary>
        public void IncrementCount()
        {
            count++;
        }

        /// <summary>
        /// returns frame count
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return count;
        }

        /// <summary>
        /// returns ID of explosion
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// returns location of explosion
        /// </summary>
        /// <returns></returns>
        public Vector.Vector2D GetLoc()
        {
            return loc;
        }

    }
}
