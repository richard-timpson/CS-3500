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
        public Ship()
        {
            int ID = this.ID;
            Vector.Vector2D loc = this.loc;
            Vector.Vector2D dir = this.dir;
            bool thrust = this.thrust;
            string name = this.name;
            int hp = this.hp;
            int score = this.score;
            string color = this.color;
        }


        [JsonProperty(PropertyName = "ship")]
        public int ID { get; private set; }

        [JsonProperty]
        public Vector.Vector2D loc { get; private set; }

        [JsonProperty]
        public Vector.Vector2D dir { get; private set; }

        [JsonProperty]
        public bool thrust { get; private set; }

        [JsonProperty]
        public string name { get; private set; }

        [JsonProperty]
        public int hp { get; private set; }

        [JsonProperty]
        public int score { get; private set; }

        [JsonProperty]
        public string color { get;  private set; }
    }

}

