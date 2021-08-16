using Newtonsoft.Json;
using System;

namespace Revolution.Objects.User
{
    /// <summary>
    /// Provides data on the user's current relations
    /// </summary>
    public struct Relation
    {
        /// <summary>
        /// Current relation status
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// ULID of the current relation
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }
    }
}
