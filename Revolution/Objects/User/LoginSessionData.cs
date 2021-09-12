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

        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("subscription")]
        public string Subscription { get; private set; }
    }
}
