using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlackAPI;

namespace Taz.Core
{
    public static class SlackClientFactory
    {
        public static SlackClient CreateClient(User user)
        {
            var token = TokenLoader.GetTokenFor(user);
            return new SlackClient(token);
        }
    }
}
