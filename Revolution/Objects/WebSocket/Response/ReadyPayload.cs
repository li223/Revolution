using Newtonsoft.Json;
using Revolution.Objects.Channel;
using System.Collections.Generic;

namespace Revolution.Objects.WebSocket.Response
{
    internal class ReadyPayload : SocketResponse
    {
        [JsonProperty("users")]
        public IEnumerable<User.User> Users { get; private set; }

        [JsonProperty("servers")]
        public IEnumerable<Server.Server> Servers { get; private set; }

        [JsonProperty("channels")]
        public IEnumerable<ChannelBase> Channels { get; private set; }
    }
}
