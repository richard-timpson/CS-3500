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
        /// <summary>
        /// Constructor for Projectile.
        /// </summary>
        public Projectile()
        {

        }


        /// <summary>
        /// Unique projectile ID assigned by server.
        /// </summary>
        [JsonProperty(PropertyName = "proj")]
        public int ID { get; private set; }

        /// <summary>
        /// Location of projectile.
        /// </summary>
        [JsonProperty]
        public Vector.Vector2D loc { get; private set; }

        /// <summary>
        /// Direction projectile is traveling.
        /// </summary>
        [JsonProperty]
        public Vector.Vector2D dir { get; private set; }

        public Vector.Vector2D vel { get; private set; }

        /// <summary>
        /// Boolean value for whether projectile is active.
        /// </summary>
        [JsonProperty]
        public bool alive { get; private set; }

        /// <summary>
        /// The owner of the projectile is the player ID of the player that shot it.
        /// </summary>
        [JsonProperty]
        public int owner { get; private set; }


        public void SetID(int _ID)
        {
            this.ID = _ID;
        }

        public void SetLoc(Vector2D _loc)
        {
            this.loc = _loc;
        }
        public void SetDir(Vector2D _dir)
        {
            this.dir = _dir;
        }
        public void SetVel(Vector2D _vel)
        {
            this.vel = _vel;
        }

        public void SetAlive(bool _alive)
        {
            this.alive = _alive;
        }
        public void SetOwner(int _owner)
        {
            this.owner = _owner;
        }
    }

}
