using Newtonsoft.Json;
using Revolution.Objects;
using Revolution.Objects.Channel;
using Revolution.Objects.Messaging;
using Revolution.Objects.ModelActions;
using Revolution.Objects.User;
using System;
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
                HttpClient.DefaultRequestHeaders.Add("x-user-id", loginData.UserId.ToString());
                HttpClient.DefaultRequestHeaders.Add("x-session-token", loginData.SessionToken);

                this.SessionData = loginData;
            }
        }

        protected internal LoginSessionData GetSessionData() => this.SessionData;

        protected internal async Task<LoginSessionData> LoginAsync(string email, string password)
        {
            var serverQuery = await this.GetServerDetailsAsync();

            var response = await HttpClient.PostAsync(new Uri($"{_baseUrl}/auth/login"),
                new StringContent(JsonConvert.SerializeObject(new
                {
                    email,
                    password,
                    Captcha = serverQuery.Features.Captcha.Key
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
            var content = await response.Content.ReadAsStringAsync();
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

        protected internal async Task<(IChannel, ChannelType)> GetChannelAsync(Ulid channelId)
        {
            var response = await HttpClient.GetAsync(new Uri($"{_baseUrl}/channels/{channelId}")).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return (null, ChannelType.None);
            }

            var baseChannel = JsonConvert.DeserializeObject<ChannelBase>(content);
            return (baseChannel.ChannelType switch
            {
                ChannelType.TextChannel => JsonConvert.DeserializeObject<ServerChannel>(content),
                ChannelType.DirectMessage => JsonConvert.DeserializeObject<PrivateChannel>(content),
                _ => baseChannel
            }, baseChannel.ChannelType);
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
                    Remove = update.Remove?.ToString()
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
    }
}
