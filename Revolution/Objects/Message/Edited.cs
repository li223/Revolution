using Newtonsoft.Json;
using System;

namespace Revolution.Objects.Message
{
    public struct Edited
    {
        [JsonProperty("$date")]
        public DateTime? EditedDate { get; private set; }

        [JsonIgnore]
        public bool IsEdited { get => EditedDate.HasValue; }
    }
}
