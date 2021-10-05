using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.User
{
    internal class UserRelationshipUpdated : SocketResponse
    {
        [JsonProperty("user")]
        public Ulid TargetUserId { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }
    }
}
