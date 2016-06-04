using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using Taz.Core.Slack;

namespace Taz.Core.History
{
    public static class HistoryHelper
    {
        #region Methods

        public static async Task DigestHistory(User user)
        {
            var client = new SlackRestClient(user);

            // hardcoded on general 
            var response = await client.GetAsync(
                "channels.history",
                new[]
                {
                    new Tuple<string, object>("channel", "C1E5VFXPY")
                });

            return JsonConvert.DeserializeObject<dynamic>(response.Content);

            var data = historyResponse.Data.messages;
        }

        #endregion
    }
}