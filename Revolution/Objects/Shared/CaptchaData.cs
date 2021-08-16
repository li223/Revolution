using Newtonsoft.Json;

namespace Revolution.Objects.Shared
{
    internal class CaptchaData
    {
        [JsonProperty("enabled")]
        public bool IsEnabled { get; private set; }

        [JsonProperty("key")]
        public string Key { get; private set; }
    }
}
