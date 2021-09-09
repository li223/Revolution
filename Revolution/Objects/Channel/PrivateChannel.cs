using Newtonsoft.Json;
using Revolution.Client;
using Revolution.Objects.Messaging;
using Revolution.Objects.Messaging.Payloads;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revolution.Objects.Channel
{
    /// <summary>
    /// Object that represents a Private / Direct Message Channel
    /// </summary>
    public partial class PrivateChannel : RestClient, IChannel
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
        /// ULID of the Channel
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
    }

    public partial class PrivateChannel
    {
        /// <summary>
        /// Closes the current channel
        /// </summary>
        /// <returns>True if the channel was closed; otherwise, false</returns>
        public async Task<bool> CloseAsync()
            => await base.CloseChannelAsync(this.Id).ConfigureAwait(false);

        /// <summary>
        /// Sends a message to the current channel
        /// </summary>
        /// <param name="message">Message to Send to the Channel</param>
        /// <returns><see cref="CreatedMessage"/> object that represents the message that was sent</returns>
        public async Task<CreatedMessage> SendMessageAsync(NewMessage message)
            => await base.SendMessageAsync(this.Id, message).ConfigureAwait(false);

        /// <summary>
        /// Sends a message to the current channel
        /// </summary>
        /// <param name="content">The string message content to send</param>
        /// <param name="attachments">Collection of Attachment Ids to send with the message</param>
        /// <param name="replies">Collection of <see cref="MessageReply"/> objects that represent what messages to reply to</param>
        /// <returns><see cref="CreatedMessage"/> object that represents the message that was sent</returns>
        public async Task<CreatedMessage> SendMessageAsync(string content, IEnumerable<Ulid> attachments = null, IEnumerable<MessageReply> replies = null)
            => await base.SendMessageAsync(this.Id, new NewMessage()
            {
                Content = content,
                Attachments = attachments,
                Replies = replies
            }).ConfigureAwait(false);

        /// <summary>
        /// Gets messages from the current channel
        /// </summary>
        /// <param name="limit">The total number of messages to get</param>
        /// <param name="sort">The sort direction of messages to get</param>
        /// <param name="before">The Id of the message to get other messages before</param>
        /// <param name="after">The Id of the message to get other messages after</param>
        /// <param name="nearby">The Id of the message to get other messages nearby from</param>
        /// <param name="includeUsers">Whether or not to include users in the response</param>
        /// <returns><see cref="IEnumerable{T}"/> where <see cref="T"/> is <see cref="ShortMessage"/></returns>
        public async Task<IEnumerable<FetchedMessage>> GetMessagesAsync(int limit, MessageSort sort = MessageSort.Latest, Ulid? before = null, Ulid? after = null, Ulid? nearby = null, bool includeUsers = false)
            => await base.GetMessagesAsync(this.Id, new MessageFetchPayload()
            {
                Limit = limit,
                AfterMessageId = after,
                BeforeMessageId = before,
                IncludeUsers = includeUsers,
                NearbyMessageId = nearby,
                Sort = sort.ToString()
            }).ConfigureAwait(false);

        /// <summary>
        /// Searches messages for the current channel
        /// </summary>
        /// <param name="limit">The total number of messages to get</param>
        /// <param name="query">The search query for the request</param>
        /// <param name="sort">The sort direction of messages to get</param>
        /// <param name="before">The Id of the message to get other messages before</param>
        /// <param name="after">The Id of the message to get other messages after</param>
        /// <param name="includeUsers">Whether or not to include users in the response</param>
        /// <returns><see cref="MessageSearchPayload"/> object which represents the result of the fetch</returns>
        [Obsolete("Endpoint always returns 404, use Channel#GetMessagesAsync")]
        public async Task<MessageSearchResult> SearchMessagesAsync(int limit, string query, MessageSort sort = MessageSort.Latest, Ulid? before = null, Ulid? after = null, bool includeUsers = false)
            => await base.SearchMessagesAsync(this.Id, new MessageSearchPayload()
            {
                Limit = limit,
                AfterMessageId = after,
                BeforeMessageId = before,
                IncludeUsers = includeUsers,
                Sort = sort.ToString(),
                Query = query
            }).ConfigureAwait(false);

        /// <summary>
        /// Gets the specified message from the current channel
        /// </summary>
        /// <param name="messageId">Id of the message to get</param>
        /// <returns><see cref="ShortMessage"/> object representing the requested message</returns>
        public async Task<ShortMessage> GetMessageAsync(Ulid messageId)
            => await base.GetMessageAsync(this.Id, messageId).ConfigureAwait(false);

        /// <summary>
        /// Deletes the specified message from the current channel
        /// </summary>
        /// <param name="messageId">Id of the message to delete</param>
        /// <returns>True if the message was deleted; otherwise false</returns>
        public async Task<bool> DeleteMessageAsync(Ulid messageId)
            => await base.DeleteMessageAsync(this.Id, messageId).ConfigureAwait(false);

        /// <summary>
        /// Sends an Acknowledgement for the specified message
        /// </summary>
        /// <param name="messageId">Id of the message to acknowledge</param>
        /// <returns>True if the message was acknowledged; otherwise false</returns>
        public async Task<bool> AcknowledgeMessageAsync(Ulid messageId)
            => await base.AcknowledgeMessageAsync(this.Id, messageId).ConfigureAwait(false);
    }
}
