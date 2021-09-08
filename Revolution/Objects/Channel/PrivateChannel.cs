using Newtonsoft.Json;
using Revolution.Client;
using Revolution.Objects.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revolution.Objects.Channel
{
    /// <summary>
    /// Object that represents a Private / Direct Message Channel
    /// </summary>
    public class PrivateChannel : RestClient, IChannel
    {
        /// <summary>
        /// Whether or not the current Channel is active
        /// </summary>
        [JsonProperty("active")]
        public bool IsActive { get; private set; }

        /// <summary>
        /// List of Ids of users who are recipients in the Channel 
        /// </summary>
        [JsonProperty("recipients")]
        public IEnumerable<Ulid> Recipients { get; private set; }

        /// <summary>
        /// The most recent message that has been sent
        /// </summary>
        [JsonProperty("last_message")]
        public ShortMessage LastMessage { get; private set; }

        /// <summary>
        /// Id of the Channel
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        [JsonProperty("channel_type")]
        internal string channelType { get; private set; }

        /// <summary>
        /// The Type of Channel
        /// </summary>
        [JsonIgnore]
        public ChannelType ChannelType { get => (ChannelType)Enum.Parse(typeof(ChannelType), channelType); }

        public async Task<bool> CloseAsync()
            => await base.CloseChannelAsync(this.Id).ConfigureAwait(false);

        public async Task<CreatedMessage> SendMessageAsync(NewMessage message)
            => await base.SendMessageAsync(this.Id, message).ConfigureAwait(false);

        public async Task<CreatedMessage> SendMessageAsync(string content, IEnumerable<Ulid> attachments = null, IEnumerable<MessageReply> replies = null)
            => await base.SendMessageAsync(this.Id, new NewMessage()
            {
                Content = content,
                Attachments = attachments,
                Replies = replies
            }).ConfigureAwait(false);
    }
}
