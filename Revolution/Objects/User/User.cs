using Newtonsoft.Json;
using Revolution.Objects.Shared;
using System;
using System.Collections.Generic;

namespace Revolution.Objects.User
{
    public class User
    {
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("avatar")]
        public Image Avatar { get; private set; }

        [JsonProperty("relations")]
        public IEnumerable<Relation> Relations { get; private set; }

        [JsonProperty("badges")]
        public int Badges { get; private set; }

        [JsonProperty("status")]
        public Status Status { get; private set; }

        [JsonProperty("relationship")]
        public string Relationship { get; private set; }

        [JsonProperty("online")]
        public bool IsOnline { get; private set; }

        [JsonProperty("flags")]
        public int Flags { get; private set; }

        [JsonProperty("bot")]
        public BotInformation? BotInformation { get; private set; }

        [JsonIgnore]
        public DateTime CreationTimestamp { get => Id.Time.DateTime; }
    }
}
