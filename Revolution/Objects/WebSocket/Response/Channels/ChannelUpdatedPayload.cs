using Newtonsoft.Json;
using Revolution.Objects.Channel;
using System;

namespace Revolution.Objects.WebSocket.Response.Channels
{
    internal class ChannelUpdatedPayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ChannelId { get; private set; }

        [JsonProperty("data")]
        public IChannel Channel { get; private set; }

        [JsonProperty("clear")]
        public string Clear { get; private set; }
    }
}
