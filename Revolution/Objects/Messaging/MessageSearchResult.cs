using Newtonsoft.Json;
using Revolution.Objects.User;
using System.Collections.Generic;

namespace Revolution.Objects.Messaging
{
    public class MessageSearchResult
    {
        /// <summary>
        /// Collection of Messages that were found
        /// </summary>
        [JsonProperty("messages")]
        public IEnumerable<Message> Messages { get; private set; }

        /// <summary>
        /// Collection of Users that were found
        /// </summary>
        [JsonProperty("users")]
        public IEnumerable<User.User> Users { get; private set; }

        /// <summary>
        /// Collection of Members that were found
        /// </summary>
        /// <remarks>Member is not a complete object of the user</remarks>
        [JsonProperty("members")]
        public IEnumerable<Member> Members { get; private set; }
    }
}
