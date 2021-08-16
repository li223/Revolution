using Newtonsoft.Json;

namespace Revolution.Objects.Shared
{
    internal class RevoltFeatures
    {
        [JsonProperty("registration")]
        public bool IsRegistration { get; private set; }

        [JsonProperty("captcha")]
        public CaptchaData Captcha { get; private set; }

        [JsonProperty("email")]
        public bool IsEmail { get; private set; }

        [JsonProperty("invite_only")]
        public string InviteOnly { get; private set; }
    }
}
