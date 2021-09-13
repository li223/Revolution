using Newtonsoft.Json;
using Revolution.Objects.Channel;
using Revolution.Objects.ModelActions;
using Revolution.Objects.WebSocket;
using Revolution.Objects.WebSocket.Enums;
using Revolution.Objects.WebSocket.Response;
using Revolution.Objects.WebSocket.Response.Channels;
using Revolution.Objects.WebSocket.Response.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Revolution.Client
{
    public class RevoltClient : RevoltClientBase
    {
        public event ClientReadyArgs Ready;
        public event ClientErrorArgs ClientErrored;
        public event ClientPongArgs Pinged;

        public event MessageReceivedArgs MessageReceived;
        public event MessageUpdatedArgs MessageUpdated;
        public event MessageDeletedArgs MessageDeleted;

        public event ChannelCreatedArgs ChannelCreated;

        internal WebSocket Socket;

        /// <summary>
        /// Login via a token.
        /// </summary>
        /// <param name="token">Token to login with</param>
        /// <remarks>This ctor is meant for bot tokens. However, with the recent api changes, 
        /// User tokens can be inputted into this ctor as well and work the same as bot tokens</remarks>
        public RevoltClient(string token) : base(token) { }

        /// <summary>
        /// Login via email and password
        /// </summary>
        /// <param name="email">Email to login with</param>
        /// <param name="password">Password to login with</param>
        [Obsolete("User login is not supported")]
        public RevoltClient(string email, string password) : base(email, password)
            => throw new NotImplementedException("User login is currently not supported");

        public async Task<bool> UpdateSelfAsync(Action<SelfUpdateModel> action)
            => await this.Rest.UpdateSelfAsync(action).ConfigureAwait(false);

        public async Task ConnectAsync()
        {
            var data = await this.Rest.GetServerDetailsAsync();
            this.Socket = new WebSocket(data.WebSocketUrl);

            this.Socket.Closed += this.Websocket_Closed;
            this.Socket.Opened += this.Websocket_Opened;
            this.Socket.Error += this.Websocket_Error;
            this.Socket.MessageReceived += this.Socket_MessageReceived;
            this.Socket.EnableAutoSendPing = true;

            await this.Socket.OpenAsync();
        }

        /// <summary>
        /// Ping the server
        /// </summary>
        /// <param name="timestamp">An unix timestamp to associate with the request</param>
        public void Ping(ulong timestamp = 0) => this.Socket.Send($@"{{""type"": ""Ping"",""data"": {timestamp}}}");

        private void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[LOG] WebSocket errored: {e.Exception.Message} - {DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;

            this.ClientErrored?.Invoke(e.Exception.Message);
        }

        private void Websocket_Opened(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[LOG] WebSocket opened - {DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;

            this.Socket.Send(JsonConvert.SerializeObject(new Authenticate()
            {
                Token = this.Token
            }));
        }

        private void Websocket_Closed(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[LOG] WebSocket closed - {DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void Socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var baseType = JsonConvert.DeserializeObject<SocketResponse>(e.Message);

            switch (baseType.Type)
            {
                case "Ready":
                    var readyPayload = JsonConvert.DeserializeObject<ReadyPayload>(e.Message);
                    this.Ready?.Invoke(readyPayload.Users, readyPayload.Servers, readyPayload.Channels);

                    this.AvaliableUsers = readyPayload.Users;
                    this.AvaliableServers = readyPayload.Servers;
                    this.AvaliableChannels = readyPayload.Channels;
                    break;

                case "Pong":
                    var pongPayload = JsonConvert.DeserializeObject<PongPayload>(e.Message);
                    this.Pinged?.Invoke(pongPayload.Timestamp);
                    break;

                case "Authenticated":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[LOG] Authenticated Event Received - {DateTime.Now}");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "Error":
                    var errorPayload = JsonConvert.DeserializeObject<ErrorPayload>(e.Message);
                    this.ClientErrored?.Invoke(typeof(ErrorType).GetProperty(errorPayload.Error).GetCustomAttribute<DescriptionAttribute>().Description);
                    break;

                case "Message":
                    var messagePayload = JsonConvert.DeserializeObject<MessagePayload>(e.Message);
                    this.MessageReceived?.Invoke(messagePayload.Messages);
                    break;

                case "MessageUpdate":
                    var messageUpdatePayload = JsonConvert.DeserializeObject<MessageUpdatePayload>(e.Message);
                    this.MessageUpdated?.Invoke(messageUpdatePayload.Message, messageUpdatePayload.MessageId);
                    break;

                case "MessageDelete":
                    var messageDeletedPayload = JsonConvert.DeserializeObject<MessageDeletedPayload>(e.Message);
                    this.MessageDeleted?.Invoke(messageDeletedPayload.MessageId, messageDeletedPayload.ChannelId);
                    break;

                case "ChannelCreate":
                    var channelCreatedPayload = JsonConvert.DeserializeObject<ChannelCreatedPayload>(e.Message);
                    this.ChannelCreated?.Invoke(channelCreatedPayload.Channels);

                    var avaliableChannels = new List<IChannel>(AvaliableChannels);
                    avaliableChannels.AddRange(channelCreatedPayload.Channels);

                    AvaliableChannels = avaliableChannels;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[LOG] Unknown Event: {baseType.Type} - {DateTime.Now}");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}
