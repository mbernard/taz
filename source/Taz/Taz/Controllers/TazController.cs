using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json.Linq;

using Taz.Core;
using Taz.Core.Extensions;
using Taz.Core.Models;
using Taz.Core.Reply;
using Taz.Core.Slack;
using Taz.Core.User;

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
            var commandContext = (SlackCommand)obj.ToObject<SlackCommand>();

            // Set user
            var user = UserResolver.GetByUserName(commandContext.UserName);
            var clientFactory = new SlackClientFactory(user);

            // Get unread messages
            var digestProvider = new DigestProvider(user);
            var unreadMessages = await digestProvider.GetUnreadMessagesAsync(commandContext);

            var trendingMessages = unreadMessages.OrderByTrending();

            // Filter/aggregate what's relevant
            var digest = new Digest();
            var section = new Section();
            section.Name="Unreads";
            section.IconEmoji = ":heart:";
            section.Items = unreadMessages.Select(x => x.Text);
            section.Items = new List<string>() { "item1", "item2" };
            digest.Sections.Add(section);

            // Reply
            await ReplyHelper.BotReplyAsync(clientFactory, commandContext, digest);
        }
    }
}
