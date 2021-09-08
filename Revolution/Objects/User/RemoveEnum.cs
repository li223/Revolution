namespace Revolution.Objects.User
{
    /// <summary>
    /// Enum that represents what to remove
    /// </summary>
    public enum RemoveEnum
    {
        /// <summary>
        /// Remove nothing
        /// </summary>
        None = 0,
        /// <summary>
        /// Remove user avatar
        /// </summary>
        Avatar = 1,

        /// <summary>
        /// Remove user background
        /// </summary>
        ProfileBackground = 2,

        /// <summary>
        /// Remove profile description
        /// </summary>
        ProfileContent = 3,

        /// <summary>
        /// Remove Status text
        /// </summary>
        StatusText = 4,

        /// <summary>
        /// Remove the Channel description
        /// </summary>
        Description = 5,

        /// <summary>
        /// Remove the Channel icon
        /// </summary>
        Icon = 6
    }
}
