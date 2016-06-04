using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taz.Data
{
    [RequestPath("channels.history")]
    class HistoryResponse: Response
    {
        public int unreads { get; set; }
    }
}
