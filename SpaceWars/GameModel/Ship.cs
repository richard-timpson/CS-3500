using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Vector;

namespace GameModel
{

    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        [JsonProperty(PropertyName = "ship")]
        public int ID { get; set; }

        [JsonProperty]
        public Vector.Vector2D loc { get; set; }

        [JsonProperty]
        public Vector.Vector2D dir { get; set; }

        [JsonProperty]
        public bool thrust { get; set; }

        [JsonProperty]
        public string name { get; set; }

        [JsonProperty]
        public int hp { get; set; }

        [JsonProperty]
        public int score { get; set; }

        [JsonProperty]
        public string color { get;  set; }
    }

}

