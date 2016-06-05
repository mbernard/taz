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

            var trendingMessages = unreadMessages.WhereNotBot().OrderByTrending();
            var mentionnedMessages = unreadMessages.WhereNotBot().WhereMentioned(commandContext);

            await digestProvider.MarkAllAsReadAsync(commandContext);

            var digest = new Digest();

            // Trending
            var trendingSection = new Section();
            trendingSection.Name = ":trending: Trending";
            trendingSection.Items = trendingMessages;
            trendingSection.Color = "#E01765";

            digest.Sections.Add(trendingSection);

            // Mentions
            var mentionSection = new Section();
            mentionSection.Name = ":mention: Mentions";
            mentionSection.Items = mentionnedMessages;
            mentionSection.Color = "#02D9CD";

            digest.Sections.Add(mentionSection);

            // Topic
            var topicSection = new Section();
            topicSection.Name = ":topic: Topics";
            topicSection.Items = mentionnedMessages;
            topicSection.Color = "#FAAD0F";

            // Reply
            await ReplyHelper.BotReplyAsync(clientFactory, commandContext, digest);
        }
    }
}
