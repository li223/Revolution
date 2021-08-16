using Newtonsoft.Json;

namespace Revolution.Objects.Message
{
    public struct Content
    {
        /// <summary>
        /// Content Type of the Message
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// Message Text
        /// </summary>
        [JsonProperty("content")]
        public string Text { get; private set; }
    }
}
