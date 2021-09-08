namespace Revolution.Objects.Channel
{
    /// <summary>
    /// Enum representing the different channel types
    /// </summary>
    public enum ChannelType : int
    {
        /// <summary>
        /// Server Text Channel
        /// </summary>
        TextChannel = 0,

        /// <summary>
        /// Voice Channel
        /// </summary>
        VoiceChannel = 1,

        /// <summary>
        /// Direct Message Text Channel
        /// </summary>
        DirectMessage = 2,

        /// <summary>
        /// Used in case of error during serialisation
        /// </summary>
        None = 3
    }
}
