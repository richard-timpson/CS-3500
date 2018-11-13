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

        public int GetID()
        {
            return this.ID;
        }

        public Vector.Vector2D GetLocation()
        {
            return this.loc;
        }

        [JsonProperty(PropertyName = "star")]
        private int ID;

        [JsonProperty]
        private Vector.Vector2D loc;

        [JsonProperty]
        private double mass;
    }
}
