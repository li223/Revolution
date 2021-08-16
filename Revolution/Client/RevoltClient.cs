using Newtonsoft.Json;
using Revolution.Objects.Channel;
using Revolution.Objects.Server;
using Revolution.Objects.Shared;
using Revolution.Objects.User;
using Revolution.Objects.WebSocket;
using Revolution.Objects.WebSocket.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Revolution.Client
{
    public class RevoltClient : RevoltClientBase
    {
        public event ClientReadyArgs Ready;
        public event ClientErrorArgs ClientErrored;
        public event MessageReceivedArgs MessageReceived;

        public IEnumerable<User> AvaliableUsers { get; internal set; }

        public IEnumerable<Server> AvaliableServers { get; internal set; }

        public IEnumerable<Channel> AvaliableChannels { get; internal set; }

        private LoginType LoginType { get; set; }

        public RevoltClient(string token) : base(token)
            => LoginType = LoginType.Bot;

        public RevoltClient(string email, string password) : base(email, password)
            => LoginType = LoginType.User;

        public async Task ConnectAsync()
        {
            var data = await this.Rest.GetServerDetailsAsync();
            var websocket = new WebSocket(data.WebSocketUrl);

            websocket.DataReceived += this.Websocket_DataReceived;
            websocket.Closed += this.Websocket_Closed;
            websocket.Opened += this.Websocket_Opened;
            websocket.Error += this.Websocket_Error;
            websocket.EnableAutoSendPing = true;

            await websocket.OpenAsync();

            if (this.LoginType == LoginType.Bot)
            {
                websocket.Send(JsonConvert.SerializeObject(new AuthenticateBot()
                {
                    Token = this.BotToken
                }));
            }
            else
            {
                websocket.Send(JsonConvert.SerializeObject(new AuthenticateUser()
                {
                    SessionToken = this.SessionToken,
                    UserId = this.UserId
                }));
            }
        }

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
        }

        private void Websocket_Closed(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[LOG] WebSocket closed - {DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void Websocket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            var jsonData = Encoding.Unicode.GetString(e.Data);
            var baseType = JsonConvert.DeserializeObject<SocketResponse>(jsonData);

            Console.WriteLine($"[LOG] WebSocket Data Received: {jsonData} - {DateTime.Now}");

            switch (baseType.Type)
            {
                case "Ready":
                    var readyPayload = JsonConvert.DeserializeObject<ReadyPayload>(jsonData);
                    this.Ready?.Invoke(readyPayload.Users, readyPayload.Servers, readyPayload.Channels);
                    this.AvaliableUsers = readyPayload.Users;
                    this.AvaliableServers = readyPayload.Servers;
                    this.AvaliableChannels = readyPayload.Channels;
                    break;

                case "Message":
                    var messagePayload = JsonConvert.DeserializeObject<MessagePayload>(jsonData);
                    this.MessageReceived?.Invoke(messagePayload.Messages);
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[LOG] Unknown Event: {baseType.Type} - {DateTime.Now}");
                    break;
            }
        }
    }
}
