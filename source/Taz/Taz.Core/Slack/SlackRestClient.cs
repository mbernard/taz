using System;
using System.Threading.Tasks;

using RestSharp;

using Taz.Core.Tokens;

namespace Taz.Core.Slack
{
    public class SlackRestClient : RestClient
    {
        #region Constructors

        public SlackRestClient(User user)
            :base("https://slack.com/api/")
        {
            var token = TokenLoader.GetTokenFor(user);
            this.Authenticator = new SlackAuthenticator(token);
        }

        #endregion
        
    }
}