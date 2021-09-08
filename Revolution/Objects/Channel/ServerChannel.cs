using Newtonsoft.Json;
using Revolution.Client;
using Revolution.Objects.Messaging;
using Revolution.Objects.ModelActions;
using Revolution.Objects.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revolution.Objects.Channel
{
    /// <summary>
    /// Object representing a Server Channel
    /// </summary>
    public class ServerChannel : RestClient, IChannel
    {
        /// <summary>
        /// Name of the Server this Channel belongs to
        /// </summary>
        [JsonProperty("server")]
        public string Server { get; private set; }

        /// <summary>
        /// Name of the Channel
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// The Channel's description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// The Channel's Icon
        /// </summary>
        [JsonProperty("icon")]
        public Image Icon { get; private set; }

        /// <summary>
        /// The default permissions for this Channel
        /// </summary>
        //Todo: Make this a flags enum
        [JsonProperty("default_permissions")]
        public uint DefaultPermissions { get; private set; }

        /// <summary>
        /// The role permissions set for this Channel
        /// </summary>
        //Todo: Make this a flags enum
        [JsonProperty("role_permissions")]
        public Dictionary<string, uint> RolePermissions { get; private set; }

        /// <summary>
        /// Security Nonce string
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; private set; }

        /// <summary>
        /// Id of the Channel
        /// </summary>
        [JsonProperty("_id")]
        public Ulid Id { get; private set; }

        [JsonProperty("channel_type")]
        internal string channelType { get; private set; }

        /// <summary>
        /// The Type of Channel
        /// </summary>
        [JsonIgnore]
        public ChannelType ChannelType { get => (ChannelType)Enum.Parse(typeof(ChannelType), channelType); }

        /// <summary>
        /// Performs an Update Action on the Channel
        /// </summary>
        /// <param name="action">Action Model for updating the Channel</param>
        /// <returns>True if the update was successful; otherwise, false</returns>
        public async Task<bool> UpdateAsync(Action<ChannelUpdateModel> action)
        {
            var baseModel = new ChannelUpdateModel()
            {
                Description = Description,
                IconId = Icon?.Id,
                Name = Name,
                Remove = null
            };

            action(baseModel);

           return await base.EditChannelAsync(this.Id, baseModel).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync() 
            => await base.CloseChannelAsync(this.Id).ConfigureAwait(false);

        public async Task<CreatedMessage> SendMessageAsync(NewMessage message)
            => await base.SendMessageAsync(this.Id, message).ConfigureAwait(false);

        public async Task<CreatedMessage> SendMessageAsync(string content, IEnumerable<Ulid> attachments = null, IEnumerable<MessageReply> replies = null)
            => await base.SendMessageAsync(this.Id, new NewMessage()
            {
                Content = content,
                Attachments = attachments,
                Replies = replies
            }).ConfigureAwait(false);
    }
}
