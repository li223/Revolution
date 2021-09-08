using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Revolution.Objects.Messaging
{
    /// <summary>
    /// Object that represents a new message
    /// </summary>
    public sealed class NewMessage
    {
        /// <summary>
        /// The content of the message
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// The security nonce for the request
        /// </summary>
        [JsonProperty("nonce")]
        private Ulid Nonce { get => Ulid.NewUlid(); }

        /// <summary>
        /// A collection of attachments for the message
        /// </summary>
        [JsonProperty("attachments", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Ulid> Attachments { get; set; }

        /// <summary>
        /// A collection of messages this message is replying to
        /// </summary>
        [JsonProperty("replies", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<MessageReply> Replies { get; set; }

        /// <summary>
        /// Adds a reply to the message
        /// </summary>
        /// <param name="reply">The message to reply to</param>
        /// <returns></returns>
        public NewMessage WithReply(MessageReply reply)
        {
            var replyList = new List<MessageReply>(this.Replies)
            {
                reply
            };

            this.Replies = replyList;
            return this;
        }

        /// <summary>
        /// Adds a reply to the message
        /// </summary>
        /// <param name="messageId">Id of the message to add a reply for</param>
        /// <param name="mentionAuthor">Whether or not to mention the author of the message</param>
        /// <returns></returns>
        public NewMessage WithReply(Ulid messageId, bool mentionAuthor = false)
        {
            var replyList = new List<MessageReply>(this.Replies)
            {
                new MessageReply()
                {
                    Id = messageId,
                    MentionAuthor = mentionAuthor
                }
            };

            this.Replies = replyList;
            return this;
        }

        /// <summary>
        /// Adds an attachment to the message
        /// </summary>
        /// <param name="attachmentId">The attachment to add</param>
        /// <returns></returns>
        public NewMessage WithAttatchment(Ulid attachmentId)
        {
            var attachmentList = new List<Ulid>(this.Attachments)
            {
                attachmentId
            };

            this.Attachments = attachmentList;
            return this;
        }
    }
}
