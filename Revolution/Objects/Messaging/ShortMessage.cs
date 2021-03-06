using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Messaging
{
    /// <summary>
    /// Represents a Revolt User Message
    /// </summary>
    public class ShortMessage
    {
        /// <summary>
        /// ULID of the message
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        /// <summary>
        /// ULID of the message author
        /// </summary>
        [JsonProperty("author")]
        public Ulid AuthorId { get; private set; }

        /// <summary>
        /// Content for the Message
        /// </summary>
        [JsonProperty("short")]
        public string Content { get; private set; }
    }
}
