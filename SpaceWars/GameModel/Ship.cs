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

        public Vector.Vector2D GetLocation()
        {
            return this.loc;
        }

        public Vector.Vector2D GetOrientation()
        {
            return this.dir;
        }

        public int GetID()
        {
            return this.ID;
        }

        public int GetHP()
        {
            return this.hp;
        }

        public bool GetThrust()
        {
            return this.thrust;
        }

        [JsonProperty(PropertyName = "ship")]
        private int ID { get; set; }

        [JsonProperty]
        private Vector.Vector2D loc { get; set; }

        [JsonProperty]
        private Vector.Vector2D dir { get; set; }

        [JsonProperty]
        private bool thrust { get; set; }

        [JsonProperty]
        private string name { get; set; }

        [JsonProperty]
        private int hp { get; set; }

        [JsonProperty]
        private int score { get; set; }

        [JsonProperty]
        private string color { get;  set; }
    }

}

