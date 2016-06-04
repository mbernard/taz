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
            
            // TODO add bot reply here
        }
    }
}
