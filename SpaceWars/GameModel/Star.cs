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
    public class Star
    {    
        public Star()
        {
            int ID = this.ID;
            Vector.Vector2D loc = this.loc;
            double mass = this.mass;
        }


        [JsonProperty(PropertyName = "star")]
        public int ID { get; private set; }

        [JsonProperty]
        public Vector.Vector2D loc { get; private set; }

        [JsonProperty]
        public double mass { get; private set; }
    }
}
