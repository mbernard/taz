using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RestSharp;

using Taz.Core.Extensions;
using Taz.Core.Models;
using Taz.Core.Slack;

namespace Taz.Core
{
    public class DigestProvider
    {
        #region Fields

        private readonly SlackRestClient _client;

        #endregion

        #region Constructors

        public DigestProvider(User.User user)
        {
            var clientFactory = new SlackClientFactory(user);
            this._client = clientFactory.CreateRestClient();
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<MessageHistoryMatch>> GetUnreadMessagesAsync(SlackCommand command)
        {
            var request = new RestRequest(new Uri("channels.history", UriKind.Relative));
            request.AddQueryParameter("channel", command.ChannelId);
            request.AddQueryParameter("unreads", 100.ToString());

            var response = await this._client.ExecuteTaskAsync(request);

            var messageHistory = JsonConvert.DeserializeObject<MessageHistory>(response.Content);

            return messageHistory.Messages.Take(messageHistory.UnreadCount);
        }

        #endregion

        //    request.AddQueryParameter("count", 100.ToString());
        //    var request = new RestRequest(new Uri("search.all", UriKind.Relative));
        //{

        //public async Task<IEnumerable<MessageSearchMatch>> GetLatestMentionsAsync(SlackCommand command)
        //    request.AddQueryParameter("query", $"<@{command.UserId}|{command.UserName}> <!channel>  in:#{command.ChannelName}");

        //    var response = await this._client.ExecuteTaskAsync(request);

        //    var searchResult = JsonConvert.DeserializeObject<SearchResult>(response.Content);

        //    return searchResult.Messages.Matches;
        //}
    }
}