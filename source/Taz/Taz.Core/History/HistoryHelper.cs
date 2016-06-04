using System;
using System.Threading.Tasks;

using RestSharp;

using Taz.Core.Slack;

namespace Taz.Core.History
{
    public static class HistoryHelper
    {
        #region Methods

        public static async Task<IRestResponse<dynamic>> DigestHistory(User user)
        {
            var client = new SlackRestClient(user);

            // hardcoded on general 
            return await client.GetAsync<dynamic>(
                "channels.history",
                new[]
                {
                    new Tuple<string, object>("channel", "C1E5VFXPY")
                });
        }

        #endregion
    }
}