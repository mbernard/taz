using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using RestSharp;

using SlackAPI;

using Taz.Core.Slack;
using Taz.Core.Tokens;

namespace Taz.Core
{
    public static class SlackClientFactory
    {
        public static SlackRestClient CreateRestClient(User user)
        {
            return new SlackRestClient(user);
        }
        public static async Task<SlackTaskClient> CreateTaskClient(User user)
        {
            var token = TokenLoader.GetTokenFor(user);
            var client = new SlackTaskClient(token);

            await client.ConnectAsync();

            return client;
        }

        public static SlackClient CreateClient(User user)
        {
            var token = TokenLoader.GetTokenFor(user);
            var client =  new SlackClient(token);

            var mre = new ManualResetEventSlim();
            client.Connect(
                x =>
                    {
                        mre.Set();
                    });

            mre.Wait();

            return client;
        }

        public async static Task<SlackTaskClient> CreateAsyncClient(User user)
        {
            var token = TokenLoader.GetTokenFor(user);
            var client = new SlackTaskClient(token);

            await client.ConnectAsync();

            return client;
        }
    }
}
