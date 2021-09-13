using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Channel
{
    public class ChannelCreate
    {
        [JsonProperty("type")]
        public string Type { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("description")]
        public string Description { get; internal set; }
        
        [JsonProperty("nsfw")]
        public bool IsNSFW { get; internal set; }

        [JsonProperty("nonce")]
        public string Nonce { get => Ulid.NewUlid().ToString(); }
    }
}
