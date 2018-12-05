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
            int ID = this.ID;
            Vector.Vector2D loc = this.loc;
            Vector.Vector2D dir = this.dir;
            bool alive = this.alive;
            int owner = this.owner;
        }

        public Projectile(int _id, Vector2D _loc, Vector2D _dir, Vector2D _vel, bool _alive, int _owner)
        {
            this.ID = _id;
            this.loc = _loc;
            this.dir = _dir;
            this.alive = _alive;
            this.owner = _owner;
            this.vel = _vel;
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

        public void SetLoc(Vector2D _loc)
        {
            this.loc = _loc;
        }
        
        public void SetAlive(bool _alive)
        {
            this.alive = _alive;
        }
    }

}
