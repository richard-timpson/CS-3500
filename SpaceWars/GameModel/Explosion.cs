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

        //Increments count once per frame to simulate animation
        public void IncrementCount()
        {
            count++;
        }

        //returns frame count
        public int GetCount()
        {
            return count;
        }

        //returns ID of explosion
        public int GetID()
        {
            return ID;
        }

        //returns location of explosion
        public Vector.Vector2D GetLoc()
        {
            return loc;
        }

    }
}
