using System;
using System.Collections.Generic;
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
            // Parse command context.
            dynamic obj = await this.Request.Content.ReadAsAsync<JObject>();
            var commandContext = obj.ToObject<SlackCommand>() as SlackCommand;

            // Set user
            var user = UserResolver.GetByUserName(commandContext.UserName);
            var clientFactory = new SlackClientFactory(user);
            
            // Get unread history
            var unreadMessages = await HistoryHelper.GetUnreadMessagesAsync(clientFactory, commandContext);

            // Filter/aggregate what's relevant
            var digest = new Digest();
            var section = new Section();
            section.Name="Unreads";
            section.IconEmoji = ":heart:";
            section.Items = unreadMessages.Select(x => x.Text);
            section.Items = new List<string>() { "item1", "item2" };
            digest.Sections.Add(section);

            var section2 = new Section();
            section2.Name = "Unreads 2";
            section2.IconEmoji = ":heart:";
            section2.Items = new List<string>() { "item4", "item5" };
            digest.Sections.Add(section2);

            // Reply
            await ReplyHelper.BotReplyAsync(clientFactory, commandContext, digest);
        }
    }
}
