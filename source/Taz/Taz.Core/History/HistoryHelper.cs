using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using Taz.Core.Models;
using Taz.Core.Slack;

namespace Taz.Core.History
{
    public static class HistoryHelper
    {
        #region Methods

        public static async Task DigestHistory(User user)
        {
            var client = new SlackRestClient(user);
            var request = new RestRequest(new Uri("channels.history", UriKind.Relative));
            request.AddQueryParameter("channel", "C1E5VFXPY");

            var response = await client.ExecuteTaskAsync(request);

            var jobject = JObject.Parse(response.Content);
            var messagesJson = jobject.Property("messages").Value;

            var messages = messagesJson.ToObject<List<MessageHistory>>();
        }

        #endregion
    }
}