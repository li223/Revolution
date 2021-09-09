namespace Revolution.Objects.Messaging
{
    public enum MessageSort : int
    {
        /// <summary>
        /// Get the latest messages first
        /// </summary>
        Latest = 0,

        /// <summary>
        /// Get the oldest messages first
        /// </summary>
        Oldest = 1
    }
}
