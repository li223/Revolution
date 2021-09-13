using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.Channels
{
    internal class ChannelDeletedPayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ChannelId { get; private set; }
    }
}
