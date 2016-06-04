using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SlackAPI;

using Taz.Core.Models;

namespace Taz.Core.Reply
{
    public static class ReplyHelper
    {
        #region Public Methods and Operators

        public static void BotReply(SlackClient client, SlackCommand command, Digest digest)
        {
            // Build Markup
            var content = CreateContent(digest);

            CreatePost(client, command, content);
        }

        private static string CreateContent(Digest digest)
        {
            var sb = new StringBuilder();

            foreach (var section in digest.Sections)
            {
                sb.AppendLine("#" + section.Name);
                sb.AppendLine("---");
            }
            

            return sb.ToString();
        }

        private static void CreatePost(SlackClient client, SlackCommand command, string markDownReply)
        {
            var mre = new ManualResetEventSlim();
            client.UploadFile(
                x => { mre.Set(); },
                Encoding.UTF8.GetBytes(markDownReply),
                "TAZ " + DateTime.UtcNow.ToShortTimeString(),
                new[] { command.ChannelId },
                "TODO Title",
                string.Empty,
                false,
                "post");

            mre.Wait(TimeSpan.FromSeconds(10));
        }

        #endregion
    }
}