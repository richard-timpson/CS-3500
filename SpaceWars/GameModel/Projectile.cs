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

        [JsonProperty(PropertyName = "proj")]
        public int ID { get; private set; }

        [JsonProperty]
        public Vector.Vector2D loc { get; private set; }

        [JsonProperty]
        public Vector.Vector2D dir { get; private set; }

        [JsonProperty]
        public bool alive { get; private set; }

        [JsonProperty]
        public int owner { get; private set; }

    }

}
