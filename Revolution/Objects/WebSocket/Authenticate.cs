using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket
{
    internal class AuthenticateBase
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "Authenticate";
    }

    internal class AuthenticateUser : AuthenticateBase
    {
        [JsonProperty("user_id")]
        public Ulid UserId { get; set; }

        [JsonProperty("session_token")]
        public string SessionToken { get; set; }
    }

    internal class AuthenticateBot : AuthenticateBase
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
