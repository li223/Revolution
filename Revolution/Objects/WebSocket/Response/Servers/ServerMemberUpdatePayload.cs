using Newtonsoft.Json;
using Revolution.Objects.User;
using System;

namespace Revolution.Objects.WebSocket.Response.Servers
{
    internal class ServerMemberUpdatePayload : SocketResponse
    {
        [JsonProperty("id")]
        public ServerMemberUpdatePayloadId Id { get; private set; }

        [JsonProperty("data")]
        public Member Member { get; private set; }

        [JsonProperty("clear")]
        public string Clear { get; private set; }
    }

    internal class ServerMemberUpdatePayloadId
    {
        [JsonProperty("server")]
        public Ulid ServerId { get; private set; }

        [JsonProperty("user")]
        public Ulid UserId { get; private set; }
    }
}
