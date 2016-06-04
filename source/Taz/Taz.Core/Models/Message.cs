using System;

using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public class Message
    {
        public string Type { get; set; }

        public string SubType { get; set; }

        public string User { get; set; }

        [JsonProperty("bot_id")]
        public string BotId { get; set; }

        public string Text { get; set; }

        [JsonProperty("ts")]
        public string UnixTimeStamp { get; set; }
    }
}
