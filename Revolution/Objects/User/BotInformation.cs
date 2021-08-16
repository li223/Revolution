using Newtonsoft.Json;
using System;

namespace Revolution.Objects.User
{
    /// <summary>
    /// Provides data about the bot user
    /// </summary>
    public struct BotInformation
    {
        /// <summary>
        /// ULID of the user who owns the bot
        /// </summary>
        [JsonProperty("owner")]
        public Ulid Owner { get; private set; }
    }
}
