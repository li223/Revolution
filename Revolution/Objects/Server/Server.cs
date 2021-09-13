using Newtonsoft.Json;
using Revolution.Client;
using Revolution.Objects.Channel;
using Revolution.Objects.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revolution.Objects.Server
{
    /// <summary>
    /// Represents a Server
    /// </summary>
    public partial class Server
    {
        /// <summary>
        /// ULID of the Server
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        /// <summary>
        /// The Security Nonce
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; private set; }

        /// <summary>
        /// The Id of the Server Owner
        /// </summary>
        [JsonProperty("owner")]
        public Ulid OwnerId { get; private set; }

        /// <summary>
        /// The Server's name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// The Server's description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// List of Channel Ids the Server has
        /// </summary>
        [JsonProperty("channels")]
        public IEnumerable<Ulid> ChannelIds { get; private set; }

        /// <summary>
        /// List of Channel Categories the Server has
        /// </summary>
        [JsonProperty("categories")]
        public IEnumerable<Category> Categories { get; private set; }

        /// <summary>
        /// List of Server System Messages
        /// </summary>
        [JsonProperty("system_messages")]
        public Dictionary<string, string> SystemMessages { get; private set; }

        /// <summary>
        /// The list of Roles the Server has
        /// </summary>
        [JsonProperty("roles")]
        public Dictionary<Ulid, Role> Roles { get; private set; }

        /// <summary>
        /// The default permissions users have on entering
        /// </summary>
        [JsonProperty("default_permissions")]
        public IEnumerable<int> DefaultPermissions { get; private set; }

        /// <summary>
        /// The Server's icon image
        /// </summary>
        [JsonProperty("icon")]
        public Image Icon { get; private set; }

        /// <summary>
        /// The Server's banner image
        /// </summary>
        [JsonProperty("banner")]
        public Image Banner { get; private set; }

        /// <summary>
        /// Whether or not the Server is NSFW
        /// </summary>
        [JsonProperty("nsfw")]
        public bool IsNSFW { get; private set; }
    }

    public partial class Server : RestClient
    {
        /// <summary>
        /// Creates a new Server Channel
        /// </summary>
        /// <param name="type">Type of Channel to Create</param>
        /// <param name="name">Name of the Channel</param>
        /// <param name="description">The Channel's description</param>
        /// <param name="isNSFW">Whether or not the Channel is NSFW (defaults to false)</param>
        /// <returns>True, if the Channel was created; otherwise, false</returns>
        public async Task<bool> CreateChannelAsync(ChannelType type, string name, string description = null, bool isNSFW = false)
            => await base.CreateServerChannelAsync(this.Id, new ChannelCreate()
            {
                Type = type.ToString(),
                Name = name,
                Description = description,
                IsNSFW = isNSFW
            }).ConfigureAwait(false);
    }
}
