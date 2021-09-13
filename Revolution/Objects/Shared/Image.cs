using Newtonsoft.Json;

namespace Revolution.Objects.Shared
{
    /// <summary>
    /// Provides data on the user's avatar
    /// </summary>
    public class Image
    {
        /// <summary>
        /// ULID of the user's avatar
        /// </summary>
        [JsonProperty("_id")]
        public string Id { get; private set; }

        /// <summary>
        /// The tag the current file has 
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; private set; }

        /// <summary>
        /// Total size in bytes of the file
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; private set; }
    
        /// <summary>
        /// The name of the file
        /// </summary>
        [JsonProperty("filename")]
        public string FileName { get; private set; }

        /// <summary>
        /// Metadata for the file
        /// </summary>
        [JsonProperty("metadata")]
        public ImageMetadata Metadata { get; private set; }

        /// <summary>
        /// File content type
        /// </summary>
        [JsonProperty("content_type")]
        public string ContentType { get; private set; }
    }
}
