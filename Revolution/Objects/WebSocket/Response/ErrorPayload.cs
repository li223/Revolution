using Newtonsoft.Json;
using Revolution.Objects.WebSocket.Enums;
using System;

namespace Revolution.Objects.WebSocket.Response
{
    internal class ErrorPayload : SocketResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonIgnore]
        public ErrorType ErrorType { get => (ErrorType)Enum.Parse(typeof(ErrorType), Error); }
    }
}
