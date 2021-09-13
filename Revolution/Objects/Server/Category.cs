using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Revolution.Objects.Server
{
    /// <summary>
    /// Represents a Category in a Server
    /// </summary>
    public class Category
    {
        /// <summary>
        /// ULID of the Category
        /// </summary>
        [JsonProperty("id")]
        public Ulid Id { get; private set; }

        /// <summary>
        /// The title of the Category
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        /// Collection of Channel Ids that belong to this Category
        /// </summary>
        [JsonProperty("channels")]
        public IEnumerable<Ulid> ChannelIds { get; private set; }
    }
}
