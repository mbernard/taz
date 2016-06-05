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
    public class SlackClientFactory
    {
        private readonly User.User _user;

        public SlackClientFactory(User.User user)
        {
            this._user = user;
        }

        public SlackRestClient CreateRestClient()
        {
            return new SlackRestClient(this._user);
        }
        public async Task<SlackTaskClient> CreateTaskClientAsync()
        {
            var token = TokenLoader.GetTokenFor(this._user);
            var client = new SlackTaskClient(token);

            await client.ConnectAsync();

            return client;
        }

        public SlackClient CreateClient()
        {
            var token = TokenLoader.GetTokenFor(this._user);
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
    }
}
