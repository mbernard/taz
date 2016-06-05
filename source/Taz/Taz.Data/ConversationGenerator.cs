using Newtonsoft.Json;
using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Taz.Core;

namespace Taz.Data
{
    public class ConversationGenerator
    {
        private SlackTaskClient Phil;
        private SlackTaskClient Philbot;
        private SlackTaskClient Yohan;
        private SlackTaskClient Mig;
        private SlackTaskClient Elodie;

        private Channel Channel;



        public ConversationGenerator()
        {

        }

        public async void Go()
        {
            this.Phil = await new SlackClientFactory(Core.User.User.Phil).CreateTaskClientAsync();
            this.Philbot = await new SlackClientFactory(Core.User.User.Philbot).CreateTaskClientAsync();
            this.Yohan = await new SlackClientFactory(Core.User.User.Yohan).CreateTaskClientAsync();
            this.Mig = await new SlackClientFactory(Core.User.User.Miguel).CreateTaskClientAsync();
            this.Elodie = await new SlackClientFactory(Core.User.User.Elodie).CreateTaskClientAsync();

            this.Channel = Phil.Channels.First(c => c.name == "test");

            var mention = $"<@{this.Phil.MyData.id}|Phil>";

            var postResponse = await this.PostMessage(this.Yohan, this.Channel.id, $"Hey {mention}, have you seen the last Games of Throne last night ?", "Yohan");
            await this.PostMessage(this.Mig, this.Channel.id, "I think he is away. But I have see it. Wow, incredible. I hate the Landcaster sooo much !", "Mig");
            await this.PostMessage(this.Yohan, this.Channel.id, "Yeah I totally agree.", "Yohan");
            var elodieToo = await this.PostMessage(this.Elodie, this.Channel.id, "Mee too !", "Elodie");
            var reactionResponse = await this.AddReaction(this.Mig, "thumbsup", this.Channel.id, elodieToo.ts);
        }

        public Task<ReactionAddedResponse> AddReaction(SlackTaskClient client,
            string name = null,
            string channel = null,
            string timestamp = null)
        {
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(name))
                parameters.Add(new Tuple<string, string>("name", name));

            if (!string.IsNullOrEmpty(channel))
                parameters.Add(new Tuple<string, string>("channel", channel));

            if (!string.IsNullOrEmpty(timestamp))
                parameters.Add(new Tuple<string, string>("timestamp", timestamp));

            return client.APIRequestWithTokenAsync<ReactionAddedResponse>(parameters.ToArray());
        }

        public Task<MessageResponse> PostMessage(SlackTaskClient client, string channelId,
            string text,
            string botName = null,
            string parse = null,
            bool linkNames = false,
            Attachment[] attachments = null,
            bool unfurl_links = false,
            string icon_url = null,
            string icon_emoji = null,
            bool as_user = true)
        {
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("text", text));

            if (!string.IsNullOrEmpty(botName))
                parameters.Add(new Tuple<string, string>("username", botName));

            if (!string.IsNullOrEmpty(parse))
                parameters.Add(new Tuple<string, string>("parse", parse));

            if (linkNames)
                parameters.Add(new Tuple<string, string>("link_names", "1"));

            if (attachments != null && attachments.Length > 0)
                parameters.Add(new Tuple<string, string>("attachments", JsonConvert.SerializeObject(attachments)));

            if (unfurl_links)
                parameters.Add(new Tuple<string, string>("unfurl_links", "1"));

            if (!string.IsNullOrEmpty(icon_url))
                parameters.Add(new Tuple<string, string>("icon_url", icon_url));

            if (!string.IsNullOrEmpty(icon_emoji))
                parameters.Add(new Tuple<string, string>("icon_emoji", icon_emoji));

            parameters.Add(new Tuple<string, string>("as_user", as_user.ToString()));

            return client.APIRequestWithTokenAsync<MessageResponse>(parameters.ToArray());
        }
    }
}
