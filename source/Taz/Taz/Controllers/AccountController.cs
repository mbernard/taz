using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

using SlackAPI;

namespace Taz.Controllers
{
    public class AccountController : Controller
    {
        public string SignIn(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "authorization code cannot be null";
            }

            var token = "";
            var mre = new ManualResetEventSlim();
            SlackClient.GetAccessToken(
                x =>
                    {
                        token = x.access_token;
                        mre.Set();
                    }, "48203572272.48200981078", "ab29427778b4fb9d8ea6c59aae50a8db", string.Empty, code);

            mre.Wait();

            return "bot token: " + token;
        }
    }
}
