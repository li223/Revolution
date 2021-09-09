using Newtonsoft.Json;
using Revolution.Client;
using System;
using System.Threading.Tasks;

namespace Revolution.Objects.Messaging
{
    /// <summary>
    /// Object representing a new message that has been created
    /// </summary>
    public class CreatedMessage : RestClient
    {
        /// <summary>
        /// ULID of the message
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        /// <summary>
        /// The security nonce
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; private set; }

        /// <summary>
        /// ULID of the Channel the message was sent in
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
}
