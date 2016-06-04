using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Taz.Core;
using Taz.Core.Tokens;

namespace Taz.Data
{
    class Program
    {
        private static SlackClient Client;

        static void Main(string[] args)
        {
            Console.WriteLine("Connecting...");

            var conversationGenerator = new ConversationGenerator();
            //conversationGenerator.Go();

            NewMethod();

            Console.WriteLine("Job's done!");
            Console.ReadKey();
        }

        private async static void NewMethod()
        {
            SlackTaskClient phil = await SlackClientFactory.CreateAsyncClient(Core.User.Phil);
            SlackTaskClient client = await SlackClientFactory.CreateAsyncClient(Core.User.Yohan);
            var channel = phil.Channels.First(c => c.name == "test");

            HistoryResponse response = await History(client, channel.id, null, null, null, 1);
        }

        private static async Task<HistoryResponse> History(SlackTaskClient client, string channel, DateTime? latest = null, DateTime? oldest = null, int? count = null, int? unread = null)
        {

            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("channel", channel));

            if (latest.HasValue)
                parameters.Add(new Tuple<string, string>("latest", latest.Value.ToProperTimeStamp()));
            if (oldest.HasValue)
                parameters.Add(new Tuple<string, string>("oldest", oldest.Value.ToProperTimeStamp()));
            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));
            if (unread.HasValue)
                parameters.Add(new Tuple<string, string>("unreads", unread.Value.ToString()));

            return await client.APIRequestWithTokenAsync<HistoryResponse>(parameters.ToArray());
        }
    }
}
