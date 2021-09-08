using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Channel
{
    internal class ChannelUpdate
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Include)]
        public string Description { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Include)]
        public Ulid? IconId { get; set; }

        [JsonProperty("remove", NullValueHandling = NullValueHandling.Include)]
        public string Remove { get; set; } = null;
    }
}
