using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RestSharp;

using Taz.Core.Extensions;
using Taz.Core.Models;
using Taz.Core.Slack;

namespace Taz.Core.History
{
    public static class HistoryHelper
    {
        #region Methods

        public static async Task<IEnumerable<Message>> GetUnreadMessagesAsync(SlackCommand command)
        {
            var client = new SlackRestClient(UserResolver.GetByUserId(command.UserId));
            var request = new RestRequest(new Uri("channels.history", UriKind.Relative));
            request.AddQueryParameter("channel", command.ChannelId);
            request.AddQueryParameter("unreads", 100.ToString());

            var response = await client.ExecuteTaskAsync(request);

            var messageHistory = JsonConvert.DeserializeObject<MessageHistory>(response.Content);

            return messageHistory.Messages.TakeLast(messageHistory.UnreadCount);
        }

        #endregion
    }
}