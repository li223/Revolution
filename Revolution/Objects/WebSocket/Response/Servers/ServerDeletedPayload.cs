using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.Servers
{
    internal class ServerDeletedPayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ServerId { get; private set; }
    }
}
