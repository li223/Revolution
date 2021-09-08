using Newtonsoft.Json;
using Revolution.Objects.User;

namespace Revolution.Objects.ModelActions
{
    /// <summary>
    /// Model used when updating the current user
    /// </summary>
    public class SelfUpdateModel
    {
        /// <summary>
        /// The current status for the user
        /// </summary>
        [JsonProperty("status")]
        public Status Status { get; set; }

        /// <summary>
        /// The current profile data for the user
        /// </summary>
        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        /// <summary>
        /// Avatar Id - Uses Autumn file Id <see cref="https://developers.revolt.chat/api/#tag/User-Information/paths/~1users~1@me/patch"/>
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("remove")]
        internal string RemoveString { get => Remove.ToString(); }

        /// <summary>
        /// Enum which determines what to remove when updating the user
        /// </summary>
        [JsonIgnore]
        public RemoveEnum Remove { get; set; } = RemoveEnum.None;
    }
}
