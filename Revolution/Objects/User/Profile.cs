using Newtonsoft.Json;

namespace Revolution.Objects.User
{
    /// <summary>
    /// Provides profile data for the current user
    /// </summary>
    public struct Profile
    {
        /// <summary>
        /// Text that is contained within the user's information box
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; private set; }

        /// <summary>
        /// ULID of the background image - Uses Autumn file Id
        /// </summary>
        [JsonProperty("background")]
        public string Background { get; private set; }
    }
}
