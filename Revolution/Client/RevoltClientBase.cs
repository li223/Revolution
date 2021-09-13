﻿using Revolution.Objects.Channel;
using Revolution.Objects.Messaging;
using Revolution.Objects.Server;
using Revolution.Objects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revolution.Client
{
    public abstract class RevoltClientBase
    {
        internal const string _baseUrl = "https://api.revolt.chat";

        public string Token { get; private set; }

        internal RestClient Rest { get; private set; }

        public IEnumerable<User> AvaliableUsers { get; internal set; }

        public IEnumerable<Server> AvaliableServers { get; internal set; }

        public IEnumerable<IChannel> AvaliableChannels { get; internal set; }

        #region Event Delegates
        /// <summary>
        /// Represents a Ready Event
        /// </summary>
        /// <param name="users">List of the avaliable users</param>
        /// <param name="servers">List of the avaliable servers</param>
        /// <param name="channels">List of the avaliable channel</param>
        /// <returns></returns>
        public delegate Task ClientReadyArgs(IEnumerable<User> users, IEnumerable<Server> servers, IEnumerable<IChannel> channels);
        
        /// <summary>
        /// Represents an Error Event
        /// </summary>
        /// <param name="message">The error message that was received</param>
        /// <returns></returns>
        public delegate Task ClientErrorArgs(string message);

        /// <summary>
        /// Represents a Pong Event
        /// </summary>
        /// <param name="timestamp">Timestamp in Unix that was requested when the ping was sent</param>
        /// <returns></returns>
        public delegate Task ClientPongArgs(ulong timestamp);

        /// <summary>
        /// Represents a Message Event
        /// </summary>
        /// <param name="messages">List of Messages that were sent</param>
        /// <returns></returns>
        public delegate Task MessageReceivedArgs(IEnumerable<Message> messages);

        /// <summary>
        /// Represents an Update Message Event
        /// </summary>
        /// <param name="updatedMessage">A Partial Message Object which represents the Message that was updated</param>
        /// <param name="messageId">ULID of the Message that was Updated</param>
        /// <returns></returns>
        public delegate Task MessageUpdatedArgs(PartialMessage updatedMessage, Ulid messageId);

        /// <summary>
        /// Represents a Message Delete Event
        /// </summary>
        /// <param name="messageId">ULID of the Message that was deleted</param>
        /// <param name="channelId">ULID of the channel the message was in</param>
        /// <returns></returns>
        public delegate Task MessageDeletedArgs(Ulid messageId, Ulid channelId);

        /// <summary>
        /// Represents a Channel Create Event
        /// </summary>
        /// <param name="channels">The list of Channels that were created</param>
        /// <returns></returns>
        public delegate Task ChannelCreatedArgs(IEnumerable<IChannel> channels);

        /// <summary>
        /// Represents a Channel Update Event
        /// </summary>
        /// <param name="channel">Partial object of the Channel that was updated</param>
        /// <param name="clear">The object to remove (optional)</param>
        /// <returns></returns>
        public delegate Task ChannelUpdatedArgs(IChannel channel, string clear);

        /// <summary>
        /// Represents a Channel Delete Event
        /// </summary>
        /// <param name="channelId">ULID of the Channel that was deleted</param>
        /// <returns></returns>
        public delegate Task ChannelDeletedArgs(Ulid channelId);

        /// <summary>
        /// Represents Events that return a Channel and User Id
        /// </summary>
        /// <param name="channelId">Id of the Channel the Event was triggered in</param>
        /// <param name="userId">Id of the user who instigated the action</param>
        /// <returns></returns>
        public delegate Task GenericChannelUserArgs(Ulid channelId, Ulid userId);

        /// <summary>
        /// Represents a Channel Acknowledge Event
        /// </summary>
        /// <param name="channelId">Id of the Channel that was acknowledged</param>
        /// <param name="userId">Id of the user who instigated the action</param>
        /// <param name="messageId">Id of the message that was acknowledged</param>
        /// <returns></returns>
        public delegate Task ChannelAckArgs(Ulid channelId, Ulid userId, Ulid messageId);

        /// <summary>
        /// Represents a Server Update Event
        /// </summary>
        /// <param name="server">Partial object of the Server that was updated</param>
        /// <param name="clear">The object to remove (optional)</param>
        /// <returns></returns>
        public delegate Task ServerUpdatedArgs(Server server, string clear);

        /// <summary>
        /// Represents a Server Delete Event
        /// </summary>
        /// <param name="serverId">ULID of the Server that was deleted</param>
        /// <returns></returns>
        public delegate Task ServerDeletedArgs(Ulid serverId);

        /// <summary>
        /// Represents an Event that has no payload
        /// </summary>
        /// <returns></returns>
        public delegate Task NoArgs();
        #endregion

        public RevoltClientBase(string token)
        {
            Token = token;
            Rest = new RestClient(_baseUrl, token);
        }

        public RevoltClientBase(string email, string password)
        {
            Rest = new RestClient(_baseUrl, email, password);
            var sessionData = Rest.GetSessionData();
            this.Token = sessionData.Token;
        }

        public async Task<IChannel> GetChannelAsync(Ulid channelId)
            => AvaliableChannels?.FirstOrDefault(x => x.Id == channelId) 
            ?? await this.Rest.GetChannelAsync(channelId).ConfigureAwait(false);
    }
}
