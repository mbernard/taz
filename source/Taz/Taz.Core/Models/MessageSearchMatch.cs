using System;

namespace Taz.Core.Models
{
    public class MessageSearchMatch : MessageBase
    {
        #region Properties

        public string UserName { get; set; }

        public string Team { get; set; }

        public Uri PermaLink { get; set; }

        #endregion
    }
}