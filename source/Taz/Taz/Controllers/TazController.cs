using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json.Linq;

using Taz.Core;
using Taz.Core.History;
using Taz.Core.Models;
using Taz.Core.Reply;

namespace Taz.Controllers
{
    [RoutePrefix("api/taz")]
    public class TazController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task Post()
        {
            dynamic obj = await this.Request.Content.ReadAsAsync<JObject>();
            var command = obj.ToObject<SlackCommand>() as SlackCommand;

            var unreadMessages = await HistoryHelper.DigestHistory(command);

            // Reply
            ReplyHelper.BotReply(command);
        }
    }
}
