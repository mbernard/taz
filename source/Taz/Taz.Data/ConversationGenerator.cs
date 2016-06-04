using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taz.Core;

namespace Taz.Data
{
   public class ConversationGenerator
    {
        private SlackClient Bot = SlackClientFactory.CreateClient(Core.User.Bot);
        private SlackClient Phil = SlackClientFactory.CreateClient(Core.User.Phil);
        private SlackClient Elodie = SlackClientFactory.CreateClient(Core.User.Elodie);
        private SlackClient Yohan = SlackClientFactory.CreateClient(Core.User.Yohan);
        private SlackClient Mig = SlackClientFactory.CreateClient(Core.User.Miguel);

        private Channel Channel;

        public ConversationGenerator()
        {
            this.Channel = Phil.Channels.First(c => c.name == "test");

            this.Post(Phil, "Allo");
        }

        public void Post(SlackClient client, string text)
        {
            client.PostMessage(x => { },
                this.Channel.id,
                text);
        }
    }
}
