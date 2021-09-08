using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Messaging
{
    /// <summary>
    /// Object representing a new message that has been created
    /// </summary>
    public class CreatedMessage
    {
        /// <summary>
        /// Id of the message
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        /// <summary>
        /// The security nonce
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; private set; }

        /// <summary>
        /// Id of the Channel the message was sent in
        /// </summary>
        [JsonProperty("channel")]
        public Ulid ChannelId { get; private set; }

        /// <summary>
        /// The Id of the user who authored the message
        /// </summary>
        [JsonProperty("author")]
        public Ulid AuthorId { get; private set; }

        /// <summary>
        /// The content of the message that was send
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; private set; }
    }
}
