using Newtonsoft.Json;

namespace Revolution.Objects
{
    /// <summary>
    /// Provides metadata on images
    /// </summary>
    public class ImageMetadata
    {
        /// <summary>
        /// Type of file
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; private set; }
    }
}
