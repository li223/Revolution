using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Channel
{
    internal class ChannelBase : IChannel
    {
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        [JsonProperty("channel_type")]
        internal string channelType { get; private set; }

        [JsonIgnore]
        public ChannelType ChannelType { get => (ChannelType)Enum.Parse(typeof(ChannelType), channelType); }
    }
}
