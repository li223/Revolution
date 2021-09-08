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
        [JsonProperty("status", NullValueHandling = NullValueHandling.Include)]
        public Status Status { get; set; }

        /// <summary>
        /// The current profile data for the user
        /// </summary>
        [JsonProperty("profile", NullValueHandling = NullValueHandling.Include)]
        public Profile Profile { get; set; }

        /// <summary>
        /// Avatar Id - Uses Autumn file Id <see cref="https://developers.revolt.chat/api/#tag/User-Information/paths/~1users~1@me/patch"/>
        /// </summary>
        [JsonProperty("avatar", NullValueHandling = NullValueHandling.Include)]
        public string Avatar { get; set; }

        [JsonProperty("remove", NullValueHandling = NullValueHandling.Include)]
        internal string RemoveString { get => Remove == null || Remove == RemoveEnum.None ? null : Remove.ToString(); }

        /// <summary>
        /// Enum which determines what to remove when updating the user
        /// </summary>
        [JsonIgnore]
        public RemoveEnum? Remove { get; set; }
    }
}
