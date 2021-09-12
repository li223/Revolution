using Newtonsoft.Json;

namespace Revolution.Objects.WebSocket
{
    internal class Authenticate
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "Authenticate";

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
