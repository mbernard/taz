using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(SlackCommand command)
        {
            var request = new RestRequest(new Uri("channels.list", UriKind.Relative));
            var response = await this._client.ExecuteTaskAsync(request);

            var jContent = JObject.Parse(response.Content);
            var channels = jContent.Property("channels").Value.ToObject<List<Channel>>();

            var responseTasks = new List<Task<IRestResponse>>();
            foreach (var channel in channels)
            {
                request = new RestRequest(new Uri("channels.history", UriKind.Relative));
                request.AddQueryParameter("channel", channel.Id);
                request.AddQueryParameter("unreads", 100.ToString());

                responseTasks.Add(this._client.ExecuteTaskAsync(request));
            }

            await Task.WhenAll(responseTasks);

            var messages = responseTasks.SelectMany(x =>
            {
                var messageHistory = JsonConvert.DeserializeObject<MessageHistory>(x.Result.Content);
                return messageHistory.Messages.Take(messageHistory.UnreadCount);

            });

            return messages.OrderByDescending(x => x.UnixTimeStamp).Where(x => x.UserId != null);
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