using Newtonsoft.Json;
using Revolution.Client.Logging;
using Revolution.Internal;
using Revolution.Objects.Channel;
using Revolution.Objects.ModelActions;
using Revolution.Objects.WebSocket;
using Revolution.Objects.WebSocket.Enums;
using Revolution.Objects.WebSocket.Response;
using Revolution.Objects.WebSocket.Response.Channels;
using Revolution.Objects.WebSocket.Response.Messages;
using Revolution.Objects.WebSocket.Response.Servers;
using Revolution.Objects.WebSocket.Response.User;
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

        public event ServerMemberUpdatedArgs MemberUpdated;
        public event ServerMemberJoinLeaveArgs MemberJoined;
        public event ServerMemberJoinLeaveArgs MemberLeft;

        public event ServerRoleUpdatedArgs RoleUpdated;
        public event ServerRoleDeletedArgs RoleDeleted;

        public event UserUpdatedArgs UserUpdated;
        public event RelationshipUpdatedArgs RelationshipUpdated;

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
            this.Logger.Log("Socket URL Received", LogLevel.Debug);
            this.Socket = new WebSocket(data.WebSocketUrl);

            this.Socket.Closed += this.Websocket_Closed;
            this.Socket.Opened += this.Websocket_Opened;
            this.Socket.Error += this.Websocket_Error;
            this.Socket.MessageReceived += this.Socket_MessageReceived;
            this.Socket.EnableAutoSendPing = true;

            this.Logger.Log("Opening Socket", LogLevel.Debug);
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
            this.Logger.Log($"Socket Errored: {e.Exception.Message}", LogLevel.Error);

            this.ClientErrored?.Invoke(e.Exception.Message);
        }

        private void Websocket_Opened(object sender, EventArgs e)
        {
            this.Logger.Log("Socket Opened", LogLevel.Info);

            this.Socket.Send(JsonConvert.SerializeObject(new Authenticate()
            {
                Token = this.Token
            }));
        }

        private void Websocket_Closed(object sender, EventArgs e) => this.Logger.Log("Socket Closed", LogLevel.Error);

        private void Socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var baseType = JsonConvert.DeserializeObject<SocketResponse>(e.Message);

            switch (baseType.Type)
            {
                case WebSocketEventConstants.Ready:
                    var readyPayload = JsonConvert.DeserializeObject<ReadyPayload>(e.Message);
                    this.Ready?.Invoke(readyPayload.Users, readyPayload.Servers, readyPayload.Channels);

                    this.AvaliableUsers = readyPayload.Users;
                    this.AvaliableServers = readyPayload.Servers;
                    this.AvaliableChannels = readyPayload.Channels;

                    this.Logger.Log("Ready Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.Pong:
                    var pongPayload = JsonConvert.DeserializeObject<PongPayload>(e.Message);
                    this.Pinged?.Invoke(pongPayload.Timestamp);

                    this.Logger.Log("Pong Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.Authenticated:
                    this.Logger.Log("Authentication Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.Error:
                    var errorPayload = JsonConvert.DeserializeObject<ErrorPayload>(e.Message);
                    this.ClientErrored?.Invoke(typeof(ErrorType).GetProperty(errorPayload.Error).GetCustomAttribute<DescriptionAttribute>().Description);

                    this.Logger.Log($"Error Received: {errorPayload.Error}", LogLevel.Error);
                    break;

                case WebSocketEventConstants.Message:
                    var messagePayload = JsonConvert.DeserializeObject<MessagePayload>(e.Message);
                    this.MessageReceived?.Invoke(messagePayload.Messages);

                    this.Logger.Log("Message Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.MessageUpdated:
                    var messageUpdatePayload = JsonConvert.DeserializeObject<MessageUpdatePayload>(e.Message);
                    this.MessageUpdated?.Invoke(messageUpdatePayload.Message, messageUpdatePayload.MessageId);

                    this.Logger.Log("Message Update Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.MessageDeleted:
                    var messageDeletedPayload = JsonConvert.DeserializeObject<MessageDeletedPayload>(e.Message);
                    this.MessageDeleted?.Invoke(messageDeletedPayload.MessageId, messageDeletedPayload.ChannelId);

                    this.Logger.Log("Message Delete Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelCreated:
                    var channelCreatedPayload = JsonConvert.DeserializeObject<ChannelCreatedPayload>(e.Message);
                    this.ChannelCreated?.Invoke(channelCreatedPayload.Channels);

                    var avaliableChannels = new List<IChannel>(AvaliableChannels);
                    avaliableChannels.AddRange(channelCreatedPayload.Channels);

                    AvaliableChannels = avaliableChannels;

                    this.Logger.Log("Channel Create Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelUpdated:
                    var channelUpdatePayload = JsonConvert.DeserializeObject<ChannelUpdatedPayload>(e.Message);
                    this.ChannelUpdated?.Invoke(channelUpdatePayload.Channel, channelUpdatePayload.Clear);

                    var channel = AvaliableChannels.FirstOrDefault(x => x.Id == channelUpdatePayload.ChannelId);
                    channel = channelUpdatePayload.Channel;

                    this.Logger.Log("Channel Update Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelDeleted:
                    var channelDeletedPayload = JsonConvert.DeserializeObject<ChannelDeletedPayload>(e.Message);
                    this.ChannelDeleted?.Invoke(channelDeletedPayload.ChannelId);

                    var channelToDelete = AvaliableChannels.FirstOrDefault(x => x.Id == channelDeletedPayload.ChannelId);
                    var channelsList = AvaliableChannels.ToList();

                    channelsList.Remove(channelToDelete);
                    AvaliableChannels = channelsList;

                    this.Logger.Log("Channel Delete Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelGroupJoined:
                    var groupJoinPayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupJoined?.Invoke(groupJoinPayload.ChannelId, groupJoinPayload.UserId);

                    this.Logger.Log("Channel Group Join Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelGroupLeft:
                    var groupLeavePayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupLeft?.Invoke(groupLeavePayload.ChannelId, groupLeavePayload.UserId);

                    this.Logger.Log("Channel Group Left Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelStartTyping:
                    var startTypingPayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupLeft?.Invoke(startTypingPayload.ChannelId, startTypingPayload.UserId);

                    this.Logger.Log("Channel Start Typing Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelStopTyping:
                    var endTypingPayload = JsonConvert.DeserializeObject<GenericChannelUserPayload>(e.Message);
                    this.GroupLeft?.Invoke(endTypingPayload.ChannelId, endTypingPayload.UserId);

                    this.Logger.Log("Channel Stop Typing Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ChannelAcked:
                    var channelAckPayload = JsonConvert.DeserializeObject<ChannelAckPayload>(e.Message);
                    this.ChannelAcknowledged?.Invoke(channelAckPayload.ChannelId, channelAckPayload.UserId, channelAckPayload.MessageId);

                    this.Logger.Log("Channel Ack Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ServerUpdated:
                    var serverUpdatePayload = JsonConvert.DeserializeObject<ServerUpdatedPayload>(e.Message);
                    this.ServerUpdated?.Invoke(serverUpdatePayload.Server, serverUpdatePayload.Clear);

                    var server = AvaliableServers.FirstOrDefault(x => x.Id == serverUpdatePayload.ServerId);
                    server = serverUpdatePayload.Server;

                    this.Logger.Log("Server Update Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ServerDeleted:
                    var serverDeletedPayload = JsonConvert.DeserializeObject<ServerDeletedPayload>(e.Message);
                    this.ServerDeleted?.Invoke(serverDeletedPayload.ServerId);

                    var serverToDelete = AvaliableServers.FirstOrDefault(x => x.Id == serverDeletedPayload.ServerId);
                    var serverList = AvaliableServers.ToList();

                    serverList.Remove(serverToDelete);
                    AvaliableServers = serverList;

                    this.Logger.Log("Server Delete Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ServerMemberUpdated:
                    var serverMemberUpdatePayload = JsonConvert.DeserializeObject<ServerMemberUpdatePayload>(e.Message);
                    this.MemberUpdated?.Invoke(serverMemberUpdatePayload.Member, serverMemberUpdatePayload.Id.ServerId);

                    this.Logger.Log("Member Update Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ServerMemberJoined:
                    var memberJoin = JsonConvert.DeserializeObject<ServerMemberJoinLeavePayload>(e.Message);
                    this.MemberJoined?.Invoke(memberJoin.UserId, memberJoin.ServerId);

                    this.Logger.Log("Member Join Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ServerMemberLeft:
                    var memberLeave = JsonConvert.DeserializeObject<ServerMemberJoinLeavePayload>(e.Message);
                    this.MemberLeft?.Invoke(memberLeave.UserId, memberLeave.ServerId);

                    this.Logger.Log("Member Left Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ServerRoleUpdated:
                    var roleUpdate = JsonConvert.DeserializeObject<ServerRoleUpdatePayload>(e.Message);
                    this.RoleUpdated?.Invoke(roleUpdate.Role, roleUpdate.ServerId);

                    this.Logger.Log("Role Update Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.ServerRoleDeleted:
                    var roleDelete = JsonConvert.DeserializeObject<ServerRoleDeletePayload>(e.Message);
                    this.RoleDeleted?.Invoke(roleDelete.RoleId, roleDelete.ServerId);

                    this.Logger.Log("Role Delete Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.UserUpdated:
                    var userUpdate = JsonConvert.DeserializeObject<UserUpdatedPayload>(e.Message);
                    this.UserUpdated?.Invoke(userUpdate.User);

                    var userToUpdate = AvaliableUsers.FirstOrDefault(x => x.Id == userUpdate.UserId);
                    var userList = AvaliableUsers.ToList();

                    userList.Remove(userToUpdate);
                    userToUpdate = userUpdate.User;
                    
                    userList.Add(userToUpdate);
                    AvaliableUsers = userList;

                    this.Logger.Log("User Update Received", LogLevel.Debug);
                    break;

                case WebSocketEventConstants.UserRelationship:
                    var relationUpdate = JsonConvert.DeserializeObject<UserRelationshipUpdated>(e.Message);
                    this.RelationshipUpdated?.Invoke(relationUpdate.TargetUserId, relationUpdate.Status);

                    var relationToUpdate = AvaliableUsers.FirstOrDefault(x => x.Id == relationUpdate.TargetUserId);
                    var usersList = AvaliableUsers.ToList();

                    usersList.Remove(relationToUpdate);
                    relationToUpdate.Relationship = relationUpdate.Status;

                    usersList.Add(relationToUpdate);
                    AvaliableUsers = usersList;

                    this.Logger.Log("User Relationship Update Received", LogLevel.Debug);
                    break;

                default:
                    this.Logger.Log($"Unknown Event: {baseType.Type}", LogLevel.Debug);
                    break;
            }
        }
    }
}
