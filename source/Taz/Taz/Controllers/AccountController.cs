using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SlackAPI;

namespace Taz.Controllers
{
    public class AccountController : Controller
    {
        public string SignIn(string code)
        {
            SlackClient.GetAccessToken(this.OnResponse, "48203572272.48200981078", "ab29427778b4fb9d8ea6c59aae50a8db", null, code);

            return "bot token:";
        }

        private void OnResponse(AccessTokenResponse accessTokenResponse)
        {
            var accessToken = accessTokenResponse.access_token;
        }
    }
}
