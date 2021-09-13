using Newtonsoft.Json;
using System;

namespace Revolution.Objects.WebSocket.Response.Messages
{
    internal class MessageDeletedPayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid MessageId { get; private set; }

        [JsonProperty("channel")]
        public Ulid ChannelId { get; private set; }
    }
}
