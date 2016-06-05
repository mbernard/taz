using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using SlackAPI;
using SlackAPI.WebSocketMessages;

using Taz.Core.Extensions;
using Taz.Core.Models;
using Taz.Core.Slack;

namespace Taz.Core
{
    public class DigestService
    {
        private static SlackSocketClient _socketClient;

        #region Fields

        private readonly SlackRestClient _client;

        private SlackClientFactory _clientFactory;

        #endregion

        #region Constructors

        public DigestService(User.User user)
        {
            this._clientFactory = new SlackClientFactory(user);
            this._client = this._clientFactory.CreateRestClient();
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Models.Message>> GetUnreadMessagesAsync(SlackCommand command)
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

        public async Task PostDigestAsync(SlackCommand command, Digest digest)
        {
            // Build Markup
            var attachments = new List<Models.Attachment>();

            foreach (var section in digest.Sections)
            {
                var attachment = new Models.Attachment();
                attachment.Color = section.Color;
                attachment.ImageUrl = section.TitleImageUrl;
                attachments.Add(attachment);

                foreach (var item in section.Items)
                {
                    var user = await this.GetUserAsync(item.UserId);
                    var attachmentItem = new Models.Attachment();
                    attachmentItem.Color = section.Color;
                    attachmentItem.ThumbUrl = user.Profile.Image48;
                    attachmentItem.Footer = $"in #{item.Channel.Name} - by {user.Profile.FullName}";

                    // Build text
                    var sb = new StringBuilder();
                    sb.AppendLine(item.Text);
                    foreach (var reaction in item.Reactions)
                    {
                        sb.Append($":{reaction.Name}: {reaction.Count}   ");
                    }

                    attachmentItem.Text = sb.ToString();
                    attachments.Add(attachmentItem);
                }
            }

            await this.PostReplyTask(attachments);
        }

        public async Task AskToReadAllAsync(SlackCommand command)
        {
            var tazImChannel = await this.GetBotImChannelAsync("U1E4VMB0T");

            var request = new RestRequest("chat.postMessage");
            request.AddQueryParameter("channel", tazImChannel.Id);
            request.AddQueryParameter("text", "Do you want to *mark all as read*?");
            request.AddQueryParameter("username", "Taz");
            request.AddQueryParameter("as_user", "false");
            request.AddQueryParameter("mrkdwn", "true");

            await this._client.ExecuteTaskAsync(request);

            _socketClient = this._clientFactory.CreateSocketClient();
            _socketClient.Connect(
                (connected) => { },
                () =>
                {
                    _socketClient.OnMessageReceived += (message) =>
                    {
                        if ((message.channel == tazImChannel.Id) &&
                        message.text.ToLowerInvariant().Contains("yes"))
                        {
                            this.MarkAllAsReadAsync(command).Wait();
                            _socketClient.SendMessage(
                                x =>
                                    {
                                        _socketClient.CloseSocket();
                                    }, tazImChannel.Id, "done!");

                        }
                    };
                });
        }

        private async Task<IEnumerable<Models.Message>> GetUnreadMessagesForChannelAsync(Models.Channel channel)
        {
            var request = new RestRequest(new Uri("channels.history", UriKind.Relative));
            request.AddQueryParameter("channel", channel.Id);
            request.AddQueryParameter("unreads", 100.ToString());

            var response = await this._client.ExecuteTaskAsync(request);

            var messageHistory = JsonConvert.DeserializeObject<Models.MessageHistory>(response.Content);
            var unreadMessages = messageHistory.Messages.Take(messageHistory.UnreadCount).ToList();
            unreadMessages.ForEach(x => x.Channel = channel);

            return unreadMessages;
        }

        private async Task<IEnumerable<Models.Message>> GetAllUnreadMessagesAsync(IEnumerable<Models.Channel> channels)
        {
            var responseTasks = new List<Tuple<Models.Channel, Task<IEnumerable<Models.Message>>>>();
            foreach (var channel in channels)
            {
                responseTasks.Add(new Tuple<Models.Channel, Task<IEnumerable<Models.Message>>>(channel, this.GetUnreadMessagesForChannelAsync(channel)));
            }

            await Task.WhenAll(responseTasks.Select(x => x.Item2));

            return responseTasks.SelectMany(x => x.Item2.Result);
        }

        private async Task<List<Models.Channel>> GetChannelsAsync()
        {
            var request = new RestRequest(new Uri("channels.list", UriKind.Relative));
            var response = await this._client.ExecuteTaskAsync(request);

            var jContent = JObject.Parse(response.Content);
            var channels = jContent.Property("channels").Value.ToObject<List<Models.Channel>>();
            return channels;
        }

        private async Task<Models.User> GetUserAsync(string userId)
        {
            var request = new RestRequest("users.info");
            request.AddQueryParameter("user", userId);

            var response = await this._client.ExecuteTaskAsync(request);
            var userResponse = JObject.Parse(response.Content);

            return userResponse.Property("user").Value.ToObject<Models.User>();
        }

        private async Task PostReplyTask(List<Models.Attachment> attachments)
        {
            var tazImChannel = await this.GetBotImChannelAsync("U1E4VMB0T");

            var request = new RestRequest("chat.postMessage");
            request.AddQueryParameter("channel", tazImChannel.Id);
            request.AddQueryParameter("text", "*Taz Super Recap!*");
            request.AddQueryParameter("link_names", "1");
            request.AddQueryParameter("attachments", JsonConvert.SerializeObject(attachments));
            request.AddQueryParameter("username", "Taz");
            request.AddQueryParameter("as_user", "false");
            request.AddQueryParameter("mrkdwn", "true");

            var response = await this._client.ExecuteTaskAsync(request);
        }

        private async Task<InstantMessagingChannel> GetBotImChannelAsync(string botUserId)
        {
            var request = new RestRequest("im.list");
            var response = await this._client.ExecuteTaskAsync(request);

            var jContent = JObject.Parse(response.Content);
            var imChannels = jContent.Property("ims").Value.ToObject<List<InstantMessagingChannel>>();
            var tazImChannel = imChannels.Single(x => x.UserId == botUserId);
            return tazImChannel;
        }

        #endregion
    }
}