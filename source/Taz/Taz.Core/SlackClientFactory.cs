﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SlackAPI;

using Taz.Core.Tokens;

namespace Taz.Core
{
    public static class SlackClientFactory
    {
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
    }
}
