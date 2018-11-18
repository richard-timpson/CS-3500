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

        public Explosion(Ship s)
        {
            this.ID = s.ID;
            this.loc = s.loc;
        }

        public void IncrementCount()
        {
            count++;
        }

        public int GetCount()
        {
            return count;
        }

        public int GetID()
        {
            return ID;
        }

        public Vector.Vector2D GetLoc()
        {
            return loc;
        }

    }
}
