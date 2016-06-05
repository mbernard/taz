using System;

using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public class InstantMessagingChannel
    {
        #region Properties

        public string Id { get; set; }

        [JsonProperty("user")]
        public string UserId { get; set; }

        #endregion
    }
}