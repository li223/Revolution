using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.Servers
{
    internal class ServerRoleDeletePayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ServerId { get; private set; }

        [JsonProperty("role_id")]
        public Ulid RoleId { get; private set; }
    }
}
