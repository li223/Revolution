using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Messaging.Payloads
{
    public sealed class MessageSearchPayload
    {
        /// <summary>
        /// The search query for the request
        /// </summary>
        [JsonProperty("query", NullValueHandling = NullValueHandling.Include)]
        public string Query { get; internal set; }

        /// <summary>
        /// The maximum number of messages to fetch
        /// </summary>
        [JsonProperty("limit", NullValueHandling = NullValueHandling.Include)]
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
        [JsonProperty("sort", NullValueHandling = NullValueHandling.Include)]
        public string Sort { get; internal set; }

        /// <summary>
        /// Whether or not to include users within the response
        /// </summary>
        [JsonProperty("include_users")]
        public bool IncludeUsers { get; internal set; }
    }
}
