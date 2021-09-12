using Revolution.Objects.Channel;
using Revolution.Objects.Messaging;
using Revolution.Objects.Server;
using Revolution.Objects.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revolution.Client
{
    public abstract class RevoltClientBase
    {
        internal const string _baseUrl = "https://api.revolt.chat";
        public string Token { get; private set; }
        public Ulid UserId { get; private set; }

        internal RestClient Rest { get; private set; }

        public delegate Task ClientReadyArgs(IEnumerable<User> users, IEnumerable<Server> servers, IEnumerable<ServerChannel> channels);
        public delegate Task ClientErrorArgs(string message);
        public delegate Task MessageReceivedArgs(IEnumerable<Message> messages);
        public delegate Task NoArgs();

        public RevoltClientBase(string token)
        {
            Token = token;
            Rest = new RestClient(_baseUrl, token);
        }

        [Obsolete("User login is not supported")]
        public RevoltClientBase(string email, string password)
        {
            throw new NotImplementedException("User login is currently not supported");

            Rest = new RestClient(_baseUrl, email, password);
            var sessionData = Rest.GetSessionData();
            this.Token = sessionData.Token;
        }
    }
}
