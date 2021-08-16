using Newtonsoft.Json;

namespace Revolution.Objects.WebSocket.Response
{
    internal class SocketResponse
    {
        [JsonProperty("type")]
        public string Type { get; private set; }
    }
}
