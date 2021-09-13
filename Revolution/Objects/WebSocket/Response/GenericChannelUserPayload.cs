using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response
{
    internal class GenericChannelUserPayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid ChannelId { get; private set; }

        [JsonProperty("user")]
        public Ulid UserId { get; private set; }
    }
}
