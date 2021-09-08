using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Messaging
{
    /// <summary>
    /// Object Representing a message reply
    /// </summary>
    public sealed class MessageReply
    {
        /// <summary>
        /// Id of the Message to reply to
        /// </summary>
        [JsonProperty("id")]
        public Ulid Id { get; set; }

        /// <summary>
        /// Whether or not to mention the author of the message
        /// </summary>
        [JsonProperty("mention")]
        public bool MentionAuthor { get; set; }
    }
}
