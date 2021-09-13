using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.Channels
{
    internal class ChannelAckPayload : GenericChannelUserPayload
    {
        [JsonProperty("message")]
        public Ulid MessageId { get; private set; }
    }
}
