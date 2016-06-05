using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using Taz.Core.Models;
using Taz.Core.Slack;

namespace Taz.Core.Reply
{
    public static class ReplyHelper
    {
        #region Public Methods and Operators

        public static async Task BotReplyAsync(SlackClientFactory clientFactory, SlackCommand commandContext, Digest digest)
        {
            // Build Markup

            var attachments = new List<Attachment>();

            foreach (var section in digest.Sections)
            {
                var attachment = new Attachment();
                attachment.Color = section.Color;
                attachment.Title = section.Name;
                attachments.Add(attachment);

                foreach (var item in section.Items)
                {
                    var user = await GetUserAsync(clientFactory, item.UserId);
                    var attachmentItem = new Attachment();
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

                //attachment.Title = "Title";
                //attachment.Text = "https://tazmaniacs.slack.com/archives/general/p1465086364000113";
                //attachment.Fields = new List<Field> { new Field { Title = "field_title", Value = "field_value", Short = false} };
                //attachment.ImageUrl = "https://tazmaniacs.slack.com/archives/general/p1465086364000113";
                //attachment.ThumbUrl = "http://iconshow.me/media/images/halloween/halloween-icons-yoo/png/32/cheshire_cat.png";
                //attachment.Footer = "footer";
                //attachment.FooterIconUrl = "http://iconshow.me/media/images/halloween/halloween-icons-yoo/png/32/cheshire_cat.png";
                
            }

            await PostReplyTask(clientFactory, commandContext, attachments);
        }

        private static async Task<Models.User> GetUserAsync(SlackClientFactory clientFactory, string userId)
        {
            var client = clientFactory.CreateRestClient();
            var request = new RestRequest("users.info");
            request.AddQueryParameter("user", userId);

            var response = await client.ExecuteTaskAsync(request);
            var userResponse = JObject.Parse(response.Content);

            return userResponse.Property("user").Value.ToObject<Models.User>();
        }

        private static async Task PostReplyTask(SlackClientFactory clientFactory, SlackCommand commandContext, List<Attachment> attachments)
        {
            var tazImChannel = await GetBotImChannelAsync(clientFactory, "U1E4VMB0T");

            var client = clientFactory.CreateRestClient();
            var request = new RestRequest("chat.postMessage");
            request.AddQueryParameter("channel", tazImChannel.Id);
            request.AddQueryParameter("text", "*Taz Super Recap!*");
            request.AddQueryParameter("link_names", "1");
            request.AddQueryParameter("attachments", JsonConvert.SerializeObject(attachments));
            request.AddQueryParameter("username", "Taz");
            request.AddQueryParameter("as_user", "false");
            request.AddQueryParameter("mrkdwn", "true");

            var response = await client.ExecuteTaskAsync(request);
        }

        private static async Task<InstantMessagingChannel> GetBotImChannelAsync(SlackClientFactory clientFactory, string botUserId)
        {
            var client = clientFactory.CreateRestClient();
            var request = new RestRequest("im.list");
            var response = await client.ExecuteTaskAsync(request);

            var jContent = JObject.Parse(response.Content);
            var imChannels = jContent.Property("ims").Value.ToObject<List<InstantMessagingChannel>>();
            var tazImChannel = imChannels.Single(x => x.UserId == botUserId);
            return tazImChannel;
        }

        private static string CreateContent(Digest digest)
        {
            var sb = new StringBuilder();

            foreach (var section in digest.Sections)
            {
                sb.AppendLine($"# {section.IconEmoji} {section.Name}");

                foreach (var item in section.Items)
                {
                    sb.Append("&lt;h1&gt; " + item + "\n");
                }

                sb.AppendLine("---");
            }
            

            return sb.ToString();
        }

        #endregion
    }
}