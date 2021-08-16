using Newtonsoft.Json;
using Revolution.Objects;
using Revolution.Objects.ModelActions;
using Revolution.Objects.User;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Revolution.Client
{
    public class RestClient
    {
        private readonly string _baseUrl;
        private HttpClient HttpClient { get; set; }

        public delegate Task RevoltClientErrorArgs(string errorMessage);
        public event RevoltClientErrorArgs RevoltClientError;

        private LoginSessionData SessionData { get; set; }

        public RestClient(string baseUrl, string token)
        {
            _baseUrl = baseUrl;
            this.HttpClient = new HttpClient();
            this.HttpClient.DefaultRequestHeaders.Add("x-bot-token", token);
        }

        public RestClient(string baseUrl, string email, string password)
        {
            _baseUrl = baseUrl;
            this.HttpClient = new HttpClient();
            var loginData = this.LoginAsync(email, password).ConfigureAwait(false).GetAwaiter().GetResult();
            if(loginData != null)
            {
                this.HttpClient.DefaultRequestHeaders.Add("x-user-id", loginData.UserId.ToString());
                this.HttpClient.DefaultRequestHeaders.Add("x-session-token", loginData.SessionToken);

                this.SessionData = loginData;
            }
        }

        public LoginSessionData GetSessionData() => this.SessionData;

        public async Task<LoginSessionData> LoginAsync(string email, string password)
        {
            var serverQuery = await this.GetServerDetailsAsync();

            var response = await this.HttpClient.PostAsync(new Uri($"{_baseUrl}/auth/login"),
                new StringContent(JsonConvert.SerializeObject(new
                {
                    email,
                    password
                })));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<LoginSessionData>(content);
        }

        public async Task<User?> GetUserAsync(string userId)
        {
            var response = await this.HttpClient.GetAsync(new Uri($"{_baseUrl}/users/{userId}")).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<User>(content);
        }

        public async Task<bool> UpdateSelfAsync(Action<SelfUpdateModel> action)
        {
            var model = new SelfUpdateModel();
            action(model);

            var request = new HttpRequestMessage(HttpMethod.Patch, new Uri($"{_baseUrl}/users/@me"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(model))
            };
            var response = await this.HttpClient.SendAsync(request).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        internal async Task<ServerDetails> GetServerDetailsAsync()
        {
            var response = await this.HttpClient.GetAsync(new Uri(_baseUrl));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                RevoltClientError?.Invoke("Api Returned a non 2xx result");
                return null;
            }
            else return JsonConvert.DeserializeObject<ServerDetails>(content);
        }
    }
}
