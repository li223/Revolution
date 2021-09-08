using Newtonsoft.Json;

namespace Revolution.Objects.User
{
    /// <summary>
    /// Provides data on the user's status
    /// </summary>
    public struct Status
    {
        public Status(string text, UserPresence presence)
        {
            Text = text;
            Presence = presence;
        }

        /// <summary>
        /// The text of the user's status
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; private set; }

        /// <summary>
        /// Current user status
        /// </summary>
        [JsonIgnore]
        public UserPresence Presence { get; private set; }

        [JsonProperty("presence")]
        internal string presence { get => Presence.ToString(); }
    }
}
