using Newtonsoft.Json;
using System;

namespace Revolution.Objects.User
{
    public class LoginSessionData
    {
        [JsonProperty("id")]
        public Ulid Id { get; private set; }

        [JsonProperty("user_id")]
        public Ulid UserId { get; private set; }

        [JsonProperty("session_token")]
        public string SessionToken { get; private set; }
    }
}
