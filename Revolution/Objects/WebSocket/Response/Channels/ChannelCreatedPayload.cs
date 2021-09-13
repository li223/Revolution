using Revolution.Objects.Channel;
using System.Collections.Generic;

namespace Revolution.Objects.WebSocket.Response.Channels
{
    internal class ChannelCreatedPayload : SocketResponse
    {
        public IEnumerable<IChannel> Channels { get; private set; }
    }
}
