using System;

using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public class Message
    {
        [JsonIgnore]
        public Channel Channel { get; set; }

        public string Type { get; set; }

        [JsonProperty("user")]
        public string UserId { get; set; }

        public string Text { get; set; }

        [JsonProperty("ts")]
        public double UnixTimeStamp { get; set; }

        public string SubType { get; set; }

        [JsonProperty("bot_id")]
        public string BotId { get; set; }

        public Reaction[] Reactions { get; set; }
    }
}
