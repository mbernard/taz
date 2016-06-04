using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json.Linq;

using Taz.Core;
using Taz.Core.History;
using Taz.Core.Models;
using Taz.Core.Reply;
using Taz.Core.Slack;

namespace Taz.Controllers
{
    [RoutePrefix("api/taz")]
    public class TazController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task Post()
        {
            var user = Core.User.Miguel;
            var client = new SlackRestClient(user);

            dynamic obj = await this.Request.Content.ReadAsAsync<JObject>();
            var commandContext = obj.ToObject<SlackCommand>() as SlackCommand;

            // Get unread history
            var unreadMessages = await HistoryHelper.GetUnreadMessagesAsync(commandContext);

            // Filter/aggregate what's relevant
            var digest = new Digest();
            var section = new Section();
            section.Name="Unreads";
            section.IconEmojiName = ":heart:";
            section.Items = unreadMessages.Select(x => x.Text);
            digest.Sections.Add(section);

            // Reply
            ReplyHelper.BotReply(SlackClientFactory.CreateClient(user), commandContext, digest);
        }
    }
}
