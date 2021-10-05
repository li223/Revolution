using Newtonsoft.Json;
using Revolution.Objects.Server;
using System;

namespace Revolution.Objects.WebSocket.Response.Servers
{
    internal class ServerRoleUpdatePayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ServerId { get; private set; }

        [JsonProperty("role_id")]
        public Ulid RoleId { get; private set; }

        [JsonProperty("data")]
        public Role Role { get; private set; }

        [JsonProperty("clear")]
        public string Clear { get; private set; }
    }
}
