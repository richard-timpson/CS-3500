using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;
using Newtonsoft.Json;

namespace GameModel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        public Projectile()
        {
            int ID = this.ID;
            Vector.Vector2D loc = this.loc;
            Vector.Vector2D dir = this.dir;
            bool alive = this.alive;
            int owner = this.owner;
        }
        public int GetID()
        {
            return this.ID;
        }

        public Vector.Vector2D GetLocation()
        {
            return this.loc;
        }

        public Vector.Vector2D GetOrientation()
        {
            return this.dir;
        }

        public bool GetAlive()
        {
            return this.alive;
        }

        [JsonProperty(PropertyName = "proj")]
        private int ID;

        [JsonProperty]
        private Vector.Vector2D loc;

        [JsonProperty]
        private Vector.Vector2D dir;

        [JsonProperty]
        private bool alive;

        [JsonProperty]
        private int owner;

    }

}
