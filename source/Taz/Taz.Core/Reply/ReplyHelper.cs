using System;
using System.Linq;
using System.Text;

namespace Taz.Core.Reply
{
    public static class ReplyHelper
    {
        public static void BotReply(string data)
        {
            var botClient = SlackClientFactory.CreateClient(User.Bot);
            
            botClient.PostMessage(x=> {}, botClient.Channels.First(x=>x.name == "general").id, "test", "Taz");
            // TODO add bot reply here
        }
    }
}
