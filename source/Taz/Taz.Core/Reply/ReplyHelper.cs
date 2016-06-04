using System;
using System.Linq;
using System.Text;

using Taz.Core.Models;

namespace Taz.Core.Reply
{
    public static class ReplyHelper
    {
        public static void BotReply(SlackCommand command)
        {
            var botClient = SlackClientFactory.CreateClient(User.Yohan);
            
            botClient.PostMessage(x=> {}, botClient.Channels.First(x=>x.name == "general").id, command.Text, "Taz");
            // TODO add bot reply here
        }
    }
}
