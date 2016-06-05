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
            var channels = await this.GetChannelsAsync();

            var messages = await this.GetUnreadMessagesForChannelsAsync(channels);

            // Order by timestamp and remove bot messages
            return messages.OrderByDescending(x => x.UnixTimeStamp).Where(x => x.UserId != null);
        }

        private async Task<IEnumerable<Message>> GetUnreadMessagesForChannelsAsync(List<Channel> channels)
        {
            var responseTasks = new List<Tuple<Channel, Task<IRestResponse>>>();
            foreach (var channel in channels)
            {
                var request = new RestRequest(new Uri("channels.history", UriKind.Relative));
                request.AddQueryParameter("channel", channel.Id);
                request.AddQueryParameter("unreads", 100.ToString());

                responseTasks.Add(new Tuple<Channel, Task<IRestResponse>>(channel, this._client.ExecuteTaskAsync(request)));
            }

            await Task.WhenAll(responseTasks.Select(x => x.Item2));

            var messages = responseTasks.SelectMany(x =>
            {
                var messageHistory = JsonConvert.DeserializeObject<MessageHistory>(x.Item2.Result.Content);
                var unreadMessages = messageHistory.Messages.Take(messageHistory.UnreadCount).ToList();
                unreadMessages.ForEach(y => y.Channel = x.Item1);

                return unreadMessages;
            });
            return messages;
        }

        private async Task<List<Channel>> GetChannelsAsync()
        {
            var request = new RestRequest(new Uri("channels.list", UriKind.Relative));
            var response = await this._client.ExecuteTaskAsync(request);

            var jContent = JObject.Parse(response.Content);
            var channels = jContent.Property("channels").Value.ToObject<List<Channel>>();
            return channels;
        }

        #endregion
    }
}