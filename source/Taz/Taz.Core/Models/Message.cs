using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public class Message
    {
        #region Constructors and Destructors

        public Message()
        {
            this.Reactions = new List<Reaction>();
        }

        #endregion

        #region Public Properties

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

        public List<Reaction> Reactions { get; set; }

        #endregion
    }
}