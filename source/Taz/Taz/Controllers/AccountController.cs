using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json.Linq;

using RestSharp;

using SlackAPI;

namespace Taz.Controllers
{
    public class AccountController : Controller
    {
        public async Task<string> SignIn(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "authorization code cannot be null";
            }

            //var client = new RestClient("https://slack.com/api/");
            //var request = new RestRequest("oauth.access");
            //request.AddQueryParameter("client_id", "48203572272.48200981078");
            //request.AddQueryParameter("client_secret", "ab29427778b4fb9d8ea6c59aae50a8db");
            //request.AddQueryParameter("code", code);

            //var response = await client.ExecuteTaskAsync(request);

            //return response.Content;

            var token = "";
            var mre = new ManualResetEventSlim();
            SlackClient.GetAccessToken(
                x =>
                    {
                        token = x.access_token;
                        mre.Set();
                    }, "48203572272.48200981078", "ab29427778b4fb9d8ea6c59aae50a8db", string.Empty, code);

            mre.Wait(TimeSpan.FromSeconds(30));

            return "token: " + token;
        }
    }
}
