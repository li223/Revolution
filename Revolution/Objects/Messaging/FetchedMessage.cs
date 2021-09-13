using Newtonsoft.Json;
using Revolution.Client;
using System;
using System.Threading.Tasks;

namespace Revolution.Objects.Messaging
{
    /// <summary>
    /// Represents a Revolt User Message
    /// </summary>
    public class PartialMessage : PartialMessageBase
    {
        /// <summary>
        /// Content for the Message
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Deletes the current message
        /// </summary>
        /// <returns>True if the message was deleted; otherwise false</returns>
        public async Task<bool> DeleteAsync() => await base.DeleteMessageAsync(this.ChannelId, this.Id).ConfigureAwait(false);

        /// <summary>
        /// Sends an Acknowledgement for the current message
        /// </summary>
        /// <returns>True if the message was acknowledged; otherwise false</returns>
        public async Task<bool> AcknowledgeMessageAsync()
            => await base.AcknowledgeMessageAsync(this.ChannelId, this.Id).ConfigureAwait(false);
    }

    public class PartialMessageBase : RestClient
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
        /// ULID of the channel the message was sent in
        /// </summary>
        [JsonProperty("channel")]
        public Ulid ChannelId { get; private set; }

        /// <summary>
        /// Security Nonce
        /// </summary>
        [JsonProperty("nonce")]
        public Ulid Nonce { get; private set; }
    }
}
