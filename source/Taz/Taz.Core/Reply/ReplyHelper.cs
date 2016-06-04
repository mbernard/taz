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

        public static void BotReply(SlackClient client, SlackCommand command, string markDownReply)
        {
            var mre = new ManualResetEventSlim();
            client.UploadFile(
                x =>
                    { mre.Set(); },
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