using Newtonsoft.Json;
using Revolution.Objects;
using Revolution.Objects.Channel;
using Revolution.Objects.Messaging;
using Revolution.Objects.Messaging.Payloads;
using Revolution.Objects.ModelActions;
using Revolution.Objects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Revolution.Client
{
    public class RestClient
    {
        protected internal static string _baseUrl;
        protected internal static HttpClient HttpClient { get; set; }

        protected internal delegate Task RevoltClientErrorArgs(string errorMessage);
        protected internal event RevoltClientErrorArgs RevoltClientError;

        protected internal LoginSessionData SessionData { get; set; }

        protected internal RestClient() { }

        protected internal RestClient(string baseUrl, string token)
        {
            _baseUrl = baseUrl;
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("x-bot-token", token);
        }

        protected internal RestClient(string baseUrl, string email, string password)
        {
            _baseUrl = baseUrl;
            HttpClient = new HttpClient();
            var loginData = this.LoginAsync(email, password).ConfigureAwait(false).GetAwaiter().GetResult();
            if (loginData != null)
            {
                HttpClient.DefaultRequestHeaders.Add("x-session-token", loginData.Token);
                this.SessionData = loginData;
            }
        }

        protected internal LoginSessionData GetSessionData() => this.SessionData;

        [Obsolete("User login is not supported")]
        protected internal async Task<LoginSessionData> LoginAsync(string email, string password)
        {
            var serverQuery = await this.GetServerDetailsAsync();

            /*
             * I cannot, for the life of me, figure out how to do user login.
             * The docs went from /auth/login to /session/login. However both return 404.
             * The actual application then uses /session/auth/login but expects a recaptcha which I obviously dont have.
             * So if someone smarter than me could figure it out, I'd be most thankful.
            */

            var loginUri = new Uri($"{_baseUrl}/session/auth/login"); /*new Uri($"{_baseUrl}/session/login");*/ /*new Uri($"{_baseUrl}/auth/login");*/

            var response = await HttpClient.PostAsync(loginUri,
                new StringContent(JsonConvert.SerializeObject(new
                {
                    email,
                    password,
                    friendly_name = "Revolution C# Library",
                    //captcha = "",
                    //challenge = ""
                })));

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<LoginSessionData>(content);
        }

        protected internal async Task<User?> GetUserAsync(string userId)
        {
            var response = await HttpClient.GetAsync(new Uri($"{_baseUrl}/users/{userId}")).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<User>(content);
        }

        protected internal async Task<bool> UpdateSelfAsync(Action<SelfUpdateModel> action)
        {
            var model = new SelfUpdateModel();
            action(model);

            var request = new HttpRequestMessage(HttpMethod.Patch, new Uri($"{_baseUrl}/users/@me"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(model))
            };

            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        internal async Task<ServerDetails> GetServerDetailsAsync()
        {
            var response = await HttpClient.GetAsync(new Uri(_baseUrl));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<ServerDetails>(content);
        }

        protected internal async Task<IChannel> GetChannelAsync(Ulid channelId)
        {
            var response = await HttpClient.GetAsync(new Uri($"{_baseUrl}/channels/{channelId}")).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }

            var baseChannel = JsonConvert.DeserializeObject<ChannelBase>(content);
            return baseChannel.ChannelType switch
            {
                ChannelType.TextChannel => JsonConvert.DeserializeObject<ServerChannel>(content),
                ChannelType.DirectMessage => JsonConvert.DeserializeObject<PrivateChannel>(content),
                _ => baseChannel
            };
        }

        protected internal async Task<bool> CreateServerChannelAsync(Ulid serverId, ChannelCreate channel)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{_baseUrl}/servers/{serverId}/channels"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(channel))
            };
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                RevoltClientError?.Invoke("Api Returned a non 2xx result");

            return response.IsSuccessStatusCode;
        }

        protected internal async Task<bool> EditChannelAsync(Ulid channelId, ChannelUpdateModel update)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, new Uri($"{_baseUrl}/channels/{channelId}"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ChannelUpdate()
                {
                    Description = update.Description,
                    IconId = update.IconId,
                    Name = update.Name,
                    Remove = update.Remove == null || update.Remove == RemoveEnum.None ? null : update.Remove?.ToString()
                }))
            };
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                RevoltClientError?.Invoke("Api Returned a non 2xx result");

            return response.IsSuccessStatusCode;
        }

        protected internal async Task<bool> CloseChannelAsync(Ulid channelId)
        {
            var response = await HttpClient.DeleteAsync(new Uri($"{_baseUrl}/channels/{channelId}")).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                RevoltClientError?.Invoke("Api Returned a non 2xx result");

            return response.IsSuccessStatusCode;
        }

        protected internal async Task<CreatedMessage> SendMessageAsync(Ulid channelId, NewMessage message)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{_baseUrl}/channels/{channelId}/messages"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(message))
            };

            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<CreatedMessage>(content);
        }

        protected internal async Task<IEnumerable<PartialMessage>> GetMessagesAsync(Ulid channelId, MessageFetchPayload payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{_baseUrl}/channels/{channelId}/messages"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload))
            };

            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else
            {
                var baseData = JsonConvert.DeserializeObject<IEnumerable<PartialMessageBase>>(content);
                var newJsonData = JsonConvert.SerializeObject(baseData.Where(x => x.AuthorId != Ulid.Empty));
                return JsonConvert.DeserializeObject<IEnumerable<PartialMessage>>(newJsonData);
            }
        }

        protected internal async Task<ShortMessage> GetMessageAsync(Ulid channelId, Ulid messageId)
        {
            var response = await HttpClient.GetAsync(new Uri($"{_baseUrl}/channels/{channelId}/messages/{messageId}")).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<ShortMessage>(content);
        }

        protected internal async Task<bool> UpdateMessageAsync(Ulid channelId, Ulid messageId, string newContent)
        {
            var response = await HttpClient.PatchAsync(new Uri($"{_baseUrl}/channels/{channelId}/messages/{messageId}"), new StringContent($@"{{""content"": ""{newContent}""}}")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                RevoltClientError?.Invoke("Api Returned a non 2xx result");

            return response.IsSuccessStatusCode;
        }

        protected internal async Task<bool> DeleteMessageAsync(Ulid channelId, Ulid messageId)
        {
            var response = await HttpClient.DeleteAsync(new Uri($"{_baseUrl}/channels/{channelId}/messages/{messageId}")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                RevoltClientError?.Invoke("Api Returned a non 2xx result");

            return response.IsSuccessStatusCode;
        }

        [Obsolete("Endpoint always returns 404, use Channel#GetMessagesAsync")]
        protected internal async Task<MessageSearchResult> SearchMessagesAsync(Ulid channelId, MessageSearchPayload payload)
        {
            var data = JsonConvert.SerializeObject(payload);

            var response = await HttpClient.PostAsync(new Uri($"{_baseUrl}/channels/{channelId}/messages/search"), new StringContent(data)).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<MessageSearchResult>(content);
        }

        protected internal async Task<bool> AcknowledgeMessageAsync(Ulid channelId, Ulid messageId)
        {
            var response = await HttpClient.PutAsync(new Uri($"{_baseUrl}/channels/{channelId}/ack/{messageId}"), null).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                RevoltClientError?.Invoke("Api Returned a non 2xx result");

            return response.IsSuccessStatusCode;
        }
    }
}
