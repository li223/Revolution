using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Messaging.Payloads
{
    public sealed class MessageFetchPayload
    {
        /// <summary>
        /// The maximum number of messages to fetch
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; internal set; }

        /// <summary>
        /// The Id of the message to get other messages before
        /// </summary>
        [JsonProperty("before", NullValueHandling = NullValueHandling.Include)]
        public Ulid? BeforeMessageId { get; internal set; }

        /// <summary>
        /// The Id of the message to get other messages after
        /// </summary>
        [JsonProperty("after", NullValueHandling = NullValueHandling.Include)]
        public Ulid? AfterMessageId { get; internal set; }

        /// <summary>
        /// The sort direction for the messages
        /// </summary>
        [JsonProperty("sort")]
        public string Sort { get; internal set; }

        /// <summary>
        /// The message to get nearby messages for
        /// </summary>
        /// <remarks>This will ignore 'before', 'after' and 'sort' options. Limits in each direction will be half of the specified limit.</remarks>
        [JsonProperty("nearby", NullValueHandling = NullValueHandling.Include)]
        public Ulid? NearbyMessageId { get; internal set; }

        /// <summary>
        /// Whether or not to include users within the response
        /// </summary>
        [JsonProperty("include_users")]
        public bool IncludeUsers { get; internal set; }
    }
}
