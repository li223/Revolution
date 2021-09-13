using Newtonsoft.Json;
using Revolution.Objects.Messaging;
using System;

namespace Revolution.Objects.WebSocket.Response.Messages
{
    internal class MessageUpdatePayload : SocketResponse
    {
        [JsonProperty("id")]
        public Ulid MessageId { get; private set; }

        [JsonProperty("data")]
        public PartialMessage Message { get; private set; }
    }
}
