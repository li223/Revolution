using Newtonsoft.Json;
using Revolution.Objects.Shared;

namespace Revolution.Objects
{
    internal class ServerDetails
    {
        [JsonProperty("revolt")]
        public string Revolt { get; private set; }

        [JsonProperty("features")]
        public RevoltFeatures Features { get; private set; }

        [JsonProperty("ws")]
        public string WebSocketUrl { get; private set; }

        [JsonProperty("app")]
        public string App { get; private set; }

        [JsonProperty("vapid")]
        public string Vapid { get; private set; }
    }
}
