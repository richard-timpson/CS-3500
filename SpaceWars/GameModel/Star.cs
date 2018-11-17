﻿using System;
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
        /// <summary>
        /// Constructor for a star.
        /// </summary>
        public Star()
        {
            int ID = this.ID;
            Vector.Vector2D loc = this.loc;
            double mass = this.mass;
        }

        /// <summary>
        /// Unique ID for the star sent by the server.
        /// </summary>
        [JsonProperty(PropertyName = "star")]
        public int ID { get; private set; }

        /// <summary>
        /// Location of the star.
        /// </summary>
        [JsonProperty]
        public Vector.Vector2D loc { get; private set; }

        /// <summary>
        /// Mass of the star.
        /// </summary>
        [JsonProperty]
        public double mass { get; private set; }
    }
}
