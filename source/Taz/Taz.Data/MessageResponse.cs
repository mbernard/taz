using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taz.Data
{
    [RequestPath("chat.postMessage")]
    public class MessageResponse : Response
    {
        public string ts { get; set; }
    }
}
