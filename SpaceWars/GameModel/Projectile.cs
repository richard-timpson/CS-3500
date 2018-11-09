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
