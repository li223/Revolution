using Newtonsoft.Json;
using System.Collections.Generic;

namespace Revolution.Objects.WebSocket.Response
{
    internal class MessagePayload : SocketResponse
    {
        public IEnumerable<Message.Message> Messages { get; private set; }
    }
}
