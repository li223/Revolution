using Newtonsoft.Json;
using Revolution.Objects.Shared;
using System;
using System.Collections.Generic;

namespace Revolution.Objects.User
{
    /// <summary>
    /// Object representing the member identity for a user in a server
    /// </summary>
    public class Member
    {
        /// <summary>
        /// ULID of the member
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }
    
        /// <summary>
        /// The member's nickname for the current server
        /// </summary>
        [JsonProperty("nickname")]
        public string Nickname { get; private set; }

        /// <summary>
        /// The member's avatar for the current server
        /// </summary>
        [JsonProperty("avatar")]
        public Image? Avatar { get; private set; }

        /// <summary>
        /// A collection of role ids the member has
        /// </summary>
        [JsonProperty("roles")]
        public IEnumerable<Ulid> Roles { get; private set; }
    }
}
