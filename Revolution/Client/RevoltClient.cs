using Newtonsoft.Json;
using Revolution.Objects.Channel;
using Revolution.Objects.ModelActions;
using Revolution.Objects.WebSocket;
using Revolution.Objects.WebSocket.Enums;
using Revolution.Objects.WebSocket.Response;
using Revolution.Objects.WebSocket.Response.Channels;
using Revolution.Objects.WebSocket.Response.Messages;
using Revolution.Objects.WebSocket.Response.Servers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public event ChannelUpdatedArgs ChannelUpdated;
        public event ChannelDeletedArgs ChannelDeleted;

        public event GenericChannelUserArgs GroupJoined;
        public event GenericChannelUserArgs GroupLeft;

        public event GenericChannelUserArgs TypingStart;
        public event GenericChannelUserArgs TypingEnd;

        public event ChannelAckArgs ChannelAcknowledged;

        public event ServerUpdatedArgs ServerUpdated;
        public event ServerDeletedArgs ServerDeleted;

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

        /// <summary>
        /// Send a typing event to the Socket
        /// </summary>
        /// <param name="channelId">The Channel to Send the Type event for</param>
        /// <param name="time">How long to have the Type event go on for. Defaults to 10 Seconds</param>
        /// <returns></returns>
        public async Task TriggerTypingAsync(Ulid channelId, TimeSpan? time = null)
        {
            if (time == null) time = TimeSpan.FromSeconds(10);

            this.Socket.Send($@"{{""type"": ""BeginTyping"",""channel"": {channelId}}}");
            await Task.Delay(time.Value);
            this.Socket.Send($@"{{""type"": ""EndTyping"",""channel"": {channelId}}}");
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

                case "ChannelUpdate":
                    var channelUpdatePayload = JsonConvert.DeserializeObject<ChannelUpdatedPayload>(e.Message);
                    this.ChannelUpdated?.Invoke(channelUpdatePayload.Channel, channelUpdatePayload.Clear);

                    var channel = AvaliableChannels.FirstOrDefault(x => x.Id == channelUpdatePayload.ChannelId);
                    channel = channelUpdatePayload.Channel;
                    break;

                case "ChannelDelete":
                    var channelDeletedPayload = JsonConvert.DeserializeObject<ChannelDeletedPayload>(e.Message);
                    this.ChannelDeleted?.Invoke(channelDeletedPayload.ChannelId);

                    var channelToDelete = AvaliableChannels.FirstOrDefault(x => x.Id == channelDeletedPayload.ChannelId);
                    var channelsList = AvaliableChannels.ToList();

                    channelsList.Remove(channelToDelete);
                    AvaliableChannels = channelsList;
                    break;

                case "ChannelGroupJoin":
                    var groupJoinPayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupJoined?.Invoke(groupJoinPayload.ChannelId, groupJoinPayload.UserId);
                    break;

                case "ChannelGroupLeave":
                    var groupLeavePayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupLeft?.Invoke(groupLeavePayload.ChannelId, groupLeavePayload.UserId);
                    break;

                case "ChannelStartTyping":
                    var startTypingPayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupLeft?.Invoke(startTypingPayload.ChannelId, startTypingPayload.UserId);
                    break;

                case "ChannelStopTyping":
                    var endTypingPayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupLeft?.Invoke(endTypingPayload.ChannelId, endTypingPayload.UserId);
                    break;

                case "ChannelAck":
                    var channelAckPayload = JsonConvert.DeserializeObject<ChannelAckPayload>(e.Message);
                    this.ChannelAcknowledged?.Invoke(channelAckPayload.ChannelId, channelAckPayload.UserId, channelAckPayload.MessageId);
                    break;

                case "ServerUpdate":
                    var serverUpdatePayload = JsonConvert.DeserializeObject<ServerUpdatedPayload>(e.Message);
                    this.ServerUpdated?.Invoke(serverUpdatePayload.Server, serverUpdatePayload.Clear);

                    var server = AvaliableServers.FirstOrDefault(x => x.Id == serverUpdatePayload.ServerId);
                    server = serverUpdatePayload.Server;
                    break;

                case "ServerDelete":
                    var serverDeletedPayload = JsonConvert.DeserializeObject<ServerDeletedPayload>(e.Message);
                    this.ServerDeleted?.Invoke(serverDeletedPayload.ServerId);

                    var serverToDelete = AvaliableServers.FirstOrDefault(x => x.Id == serverDeletedPayload.ServerId);
                    var serverList = AvaliableServers.ToList();

                    serverList.Remove(serverToDelete);
                    AvaliableServers = serverList;
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
