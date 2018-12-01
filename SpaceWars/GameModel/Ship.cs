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

        /// <summary>
        /// Contructor for Ship
        /// </summary>
        public Ship()
        {
            int ID = this.ID;
            Vector.Vector2D loc = this.loc;
            Vector.Vector2D dir = this.dir;
            bool thrust = this.thrust;
            string name = this.name;
            int hp = this.hp;
            int score = this.score;
        }

        /// <summary>
        /// Unique ID of the player that owns the ship.
        /// </summary>
        [JsonProperty(PropertyName = "ship")]
        public int ID { get; private set; }

        /// <summary>
        /// Location of the ship.
        /// </summary>
        [JsonProperty]
        public Vector.Vector2D loc { get; private set; }

        /// <summary>
        /// Direction the ship is facing.
        /// </summary>
        [JsonProperty]
        public Vector.Vector2D dir { get; private set; }

        /// <summary>
        /// Boolean value for whether the thrust is active on the ship.
        /// </summary>
        [JsonProperty]
        public bool thrust { get; private set; }

        /// <summary>
        /// The name that the player gave the server upon login.
        /// </summary>
        [JsonProperty]
        public string name { get; private set; }

        /// <summary>
        /// The health value of the ship.
        /// </summary>
        [JsonProperty]
        public int hp { get; private set; }

        /// <summary>
        /// The total score for the player.
        /// </summary>
        [JsonProperty]
        public int score { get; private set; }

        public void SetID(int id)
        {
            this.ID = id;
        }

        public void SetLoc(Vector2D loc)
        {
            this.loc = loc;
        }

        public void SetDir(Vector2D dir)
        {
            this.dir = dir;
        }

        public void SetThrust(bool thrust)
        {
            this.thrust = thrust;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetHp(int hp)
        {
            this.hp = hp;
        }

        public void SetScore(int score)
        {
            this.score = score;
        }
    }

}

