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
                attachment.ImageUrl = section.TitleImageUrl;
                attachments.Add(attachment);

                foreach (var item in section.Items)
                {
                    var user = await GetUserAsync(clientFactory, item.UserId);
                    var attachmentItem = new Attachment();
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
            var client = clientFactory.CreateRestClient();
            
            var resquest = new RestRequest("chat.postMessage");
            resquest.AddQueryParameter("channel", commandContext.ChannelId);
            resquest.AddQueryParameter("text", "*Taz Super Recap!*");
            resquest.AddQueryParameter("link_names", "1");
            resquest.AddQueryParameter("attachments", JsonConvert.SerializeObject(attachments));
            resquest.AddQueryParameter("username", "Taz");
            resquest.AddQueryParameter("as_user", "false");
            resquest.AddQueryParameter("mrkdwn", "true");

            var response = await client.ExecuteTaskAsync(resquest);
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