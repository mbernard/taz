using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taz.Data
{
    class Program
    {
        private static SlackClient Client;

        static void Main(string[] args)
        {
            Console.WriteLine("Connecting...");

            var conversationGenerator = new ConversationGenerator();

            Console.WriteLine("Job's done!");
            Console.ReadKey();
        }

        public static void GenerateData(LoginResponse responses)
        {
            var general = Client.Channels.Single(channel => channel.name == "test");

            // Get History of #general
            Client.GetChannelHistory(ChannelHistoryCallback, general);

            // Create a POST in #general
            Client.UploadFile((fileUploadResponse) =>
            {
                Console.WriteLine(fileUploadResponse.error);
            },
            Encoding.UTF8.GetBytes("allo"),
            "MIGUEL_IS_COOL",
            new[] { general.id },
            "PHIL_IS_COOL_title_also",
            "init comment",
            false,
            "post");
        }


        public static void ChannelHistoryCallback(ChannelMessageHistory response)
        {

        }
    }
}
