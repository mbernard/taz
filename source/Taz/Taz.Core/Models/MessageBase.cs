using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public abstract class MessageBase
    {
        public string Type { get; set; }

        [JsonProperty("user")]
        public string UserId { get; set; }

        public string Text { get; set; }

        [JsonProperty("ts")]
        public string UnixTimeStamp { get; set; }
    }
}