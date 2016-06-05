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

            var messages = await this.GetAllUnreadMessagesAsync(channels);

            // Order by timestamp and remove bot messages
            return messages.OrderByDescending(x => x.UnixTimeStamp);
        }

        public async Task MarkAllAsReadAsync(SlackCommand command)
        {
            var channels = await this.GetChannelsAsync();

            var responseTasks = new List<Task<IRestResponse>>();
            foreach (var channel in channels)
            {
                var messages = await this.GetUnreadMessagesForChannelAsync(channel);
                if (messages.Any())
                {
                    var request = new RestRequest(new Uri("channels.mark", UriKind.Relative));
                    request.AddQueryParameter("channel", channel.Id);
                    request.AddQueryParameter("ts", messages.Take(1).Single().UnixTimeStamp.ToString("R"));

                    responseTasks.Add(this._client.ExecuteTaskAsync(request)); 
                }
            }

            await Task.WhenAll(responseTasks);
        }

        private async Task<IEnumerable<Message>> GetUnreadMessagesForChannelAsync(Channel channel)
        {
            var request = new RestRequest(new Uri("channels.history", UriKind.Relative));
            request.AddQueryParameter("channel", channel.Id);
            request.AddQueryParameter("unreads", 100.ToString());

            var response = await this._client.ExecuteTaskAsync(request);

            var messageHistory = JsonConvert.DeserializeObject<MessageHistory>(response.Content);
            var unreadMessages = messageHistory.Messages.Take(messageHistory.UnreadCount).ToList();
            unreadMessages.ForEach(x => x.Channel = channel);

            return unreadMessages;
        }

        private async Task<IEnumerable<Message>> GetAllUnreadMessagesAsync(IEnumerable<Channel> channels)
        {
            var responseTasks = new List<Tuple<Channel, Task<IEnumerable<Message>>>>();
            foreach (var channel in channels)
            {
                responseTasks.Add(new Tuple<Channel, Task<IEnumerable<Message>>>(channel, this.GetUnreadMessagesForChannelAsync(channel)));
            }

            await Task.WhenAll(responseTasks.Select(x => x.Item2));

            return responseTasks.SelectMany(x => x.Item2.Result);
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