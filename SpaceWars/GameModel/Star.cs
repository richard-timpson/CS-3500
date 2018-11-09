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
        [JsonProperty(PropertyName = "star")]
        private int ID;

        [JsonProperty]
        private Vector.Vector2D loc;

        [JsonProperty]
        private double mass;
    }
}
