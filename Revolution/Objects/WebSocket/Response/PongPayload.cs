using Newtonsoft.Json;

namespace Revolution.Objects.WebSocket.Response
{
    internal class PongPayload : SocketResponse
    {
        [JsonProperty("data")]
        public ulong Timestamp { get; private set; }
    }
}
