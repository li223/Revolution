using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.Servers
{
    internal class ServerUpdatedPayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ServerId { get; private set; }

        [JsonProperty("data")]
        public Server.Server Server { get; private set; }

        [JsonProperty("clear")]
        public string Clear { get; private set; }
    }
}
