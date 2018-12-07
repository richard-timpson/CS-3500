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

        }

        /// <summary>
        /// Unique ID of the player that owns the ship.
        /// </summary>
        [JsonProperty(PropertyName = "ship")]
        public int ID { get ; private set; }

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

        public Vector.Vector2D vel { get; private set; }

        public int deathCounter { get; private set; }

        public int fireRateCounter { get; private set; }

        public void SetID(int _id)
        {
            this.ID = _id;
        }

        public void SetDeathCounter(int _deathCounter)
        {
            this.deathCounter = _deathCounter;
        }
        public void SetFireRateCounter(int _fireRateCounter)
        {
            this.fireRateCounter = _fireRateCounter;
        }

        public void SetLoc(Vector2D _loc)
        {
            this.loc = _loc;
        }

        public void SetDir(Vector2D _dir)
        {
            this.dir = _dir;
        }

        public void SetThrust(bool _thrust)
        {
            this.thrust = _thrust;
        }

        public void SetName(string _name)
        {
            this.name = _name;
        }

        public void SetHp(int _hp)
        {
            this.hp = _hp;
        }

        public void SetScore(int _score)
        {
            this.score = _score;
        }
        public void SetVelocity(Vector2D _vel)
        {
            this.vel = _vel;
        }
    }

}

