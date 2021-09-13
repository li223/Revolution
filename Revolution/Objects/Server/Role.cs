using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Revolution.Objects.Server
{
    public class Role
    {
        /// <summary>
        /// ULID of the Role
        /// </summary>
        [JsonProperty("id")]
        public Ulid Id { get; private set; }

        /// <summary>
        /// The name of the Role
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// List of Permissions the Role has
        /// </summary>
        [JsonProperty("permissions")]
        public IEnumerable<int> Permissions { get; private set; }
        
        /// <summary>
        /// The hexadecimal value of the Role's Colour
        /// </summary>
        [JsonProperty("colour")]
        public string Colour { get; private set; }

        /// <summary>
        /// Whether or not the Role is hoisted
        /// </summary>
        [JsonProperty("hoist")]
        public bool IsHoisted { get; private set; }

        /// <summary>
        /// How high/low the Role is in the hierarchy
        /// </summary>
        /// <remarks>0 is the lowest</remarks>
        [JsonProperty("rank")]
        public int Rank { get; private set; }
    }
}