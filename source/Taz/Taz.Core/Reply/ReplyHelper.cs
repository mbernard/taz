using System;
using System.Linq;
using System.Text;

using RestSharp;

using Taz.Core.Models;
using Taz.Core.Slack;

namespace Taz.Core.Reply
{
    public static class ReplyHelper
    {
        public static async void BotReply(SlackRestClient client, SlackCommand command, string markDownReply)
        {
            var request = new RestRequest(new Uri("files.upload", UriKind.Relative), Method.POST);
            request.AddQueryParameter("content", "TODO filecontent");
            request.AddQueryParameter("filename", "TODO filename.txt");
            request.AddQueryParameter("title", "TODO file title");
            request.AddQueryParameter("initial_comment", "TODO initial comment");
            request.AddQueryParameter("channels", command.ChannelId);

            var response = await client.ExecuteTaskAsync(request);
        }
    }
}
