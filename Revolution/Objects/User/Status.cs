using Newtonsoft.Json;

namespace Revolution.Objects.User
{
    /// <summary>
    /// Provides data on the user's status
    /// </summary>
    public struct Status
    {
        /// <summary>
        /// The text of the user's status
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; private set; }

        /// <summary>
        /// Current user status
        /// </summary>
        [JsonProperty("presence")]
        public string Presence { get; private set; }
    }
}
