using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.Servers
{
    internal class ServerMemberJoinLeavePayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ServerId { get; private set; }

        [JsonProperty("user")]
        public Ulid UserId { get; private set; }
    }
}
