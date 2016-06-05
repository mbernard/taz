using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public class MessageHistory
    {
        #region Properties

        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("unread_count_display")]
        public int UnreadCount { get; set; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        #endregion
    }
}