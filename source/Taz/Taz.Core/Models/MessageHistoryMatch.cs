using System;

using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public class MessageHistoryMatch : MessageBase
    {
        public string SubType { get; set; }

        [JsonProperty("bot_id")]
        public string BotId { get; set; }

        public Reaction[] Reactions { get; set; }
    }
}
