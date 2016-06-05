using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public abstract class MessageBase
    {
        public string Type { get; set; }
        public string User { get; set; }
        public string Text { get; set; }

        [JsonProperty("ts")]
        public string UnixTimeStamp { get; set; }
    }
}