using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.User
{
    internal class UserUpdatedPayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid UserId { get; private set; }

        [JsonProperty("data")]
        public Objects.User.User User { get; private set; }

        [JsonProperty("clear")]
        public string Clear { get; private set; }
    }
}
