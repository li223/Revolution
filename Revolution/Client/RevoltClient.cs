using Newtonsoft.Json;
using Revolution.Objects.Channel;
using Revolution.Objects.ModelActions;
using Revolution.Objects.Server;
using Revolution.Objects.Shared;
using Revolution.Objects.User;
using Revolution.Objects.WebSocket;
using Revolution.Objects.WebSocket.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Revolution.Client
{
    public class RevoltClient : RevoltClientBase
    {
        public event ClientReadyArgs Ready;
        public event ClientErrorArgs ClientErrored;
        public event MessageReceivedArgs MessageReceived;
        public event NoArgs Pinged;
        internal WebSocket Socket;

        public IEnumerable<User> AvaliableUsers { get; internal set; }

        public IEnumerable<Server> AvaliableServers { get; internal set; }

        public IEnumerable<ServerChannel> AvaliableChannels { get; internal set; }

        private LoginType LoginType { get; set; }

        public RevoltClient(string token) : base(token)
            => LoginType = LoginType.Bot;

        [Obsolete("User Login is currently not supported")]
        public RevoltClient(string email, string password) : base(email, password)
            => LoginType = LoginType.User;

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

        public void Ping() => this.Socket.Send(@"{{""type"": ""Ping"",""time"": 0}}");

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

            if (this.LoginType == LoginType.Bot)
                this.Socket.Send(JsonConvert.SerializeObject(new AuthenticateBot()
                {
                    Token = this.BotToken
                }));

            else
                this.Socket.Send(JsonConvert.SerializeObject(new AuthenticateUser()
                {
                    SessionToken = this.SessionToken,
                    UserId = this.UserId
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
                    this.Pinged?.Invoke();
                    break;

                case "Authenticated":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[LOG] Authenticated Event Received - {DateTime.Now}");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "Message":
                    var messagePayload = JsonConvert.DeserializeObject<MessagePayload>(e.Message);
                    this.MessageReceived?.Invoke(messagePayload.Messages);
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
