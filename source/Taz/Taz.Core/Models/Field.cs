using System;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace Taz.Core.Models
{
    public class Field
    {
        #region Public Properties

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("short")]
        public bool Short { get; set; }

        #endregion
    }
}