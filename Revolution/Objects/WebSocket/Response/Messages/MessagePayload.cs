using Revolution.Objects.Messaging;
using System.Collections.Generic;

namespace Revolution.Objects.WebSocket.Response.Messages
{
    internal class MessagePayload : SocketResponse
    {
        public IEnumerable<Message> Messages { get; private set; }
    }
}
