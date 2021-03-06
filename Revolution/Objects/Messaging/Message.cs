using Newtonsoft.Json;
using Revolution.Client;
using Revolution.Objects.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revolution.Objects.Messaging
{
    /// <summary>
    /// Represents a Revolt User Message
    /// </summary>
    public class Message : RestClient
    {
        /// <summary>
        /// ULID of the message
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        /// <summary>
        /// Nonce string for the message
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; private set; }

        /// <summary>
        /// ULID of the channel the message was sent in
        /// </summary>
        [JsonProperty("channel")]
        public Ulid ChannelId { get; private set; }

        /// <summary>
        /// ULID of the message author
        /// </summary>
        [JsonProperty("author")]
        public Ulid AuthorId { get; private set; }

        /// <summary>
        /// Message content
        /// </summary>
        [JsonProperty("content")]
        public Content Content { get; private set; }

        /// <summary>
        /// Collection of Images attached to the message
        /// </summary>
        [JsonProperty("attachments")]
        public IEnumerable<Image>? Attachments { get; private set; }

        /// <summary>
        /// Contains data on the edit for an edited message
        /// </summary>
        [JsonProperty("edited")]
        public Edited? Edited { get; private set; }

        /// <summary>
        /// States whether or not a message has been edited
        /// </summary>
        [JsonIgnore]
        public bool IsEdited { get => Edited == null || Edited.Value.IsEdited; }

        //Soon TM
        //[JsonProperty("embed")]
        //public Embed Embed { get; private set; }

        /// <summary>
        /// A list of user ULIDs that were mentioned
        /// </summary>
        [JsonProperty("mentions")]
        public IEnumerable<Ulid> Mentions { get; private set; }

        /// <summary>
        /// A list of message ULIDs that were replied to
        /// </summary>
        [JsonProperty("replies")]
        public IEnumerable<Ulid> Replies { get; private set; }

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
