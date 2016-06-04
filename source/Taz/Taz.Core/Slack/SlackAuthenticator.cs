using System;
using System.Linq;
using System.Text;

using RestSharp;
using RestSharp.Authenticators;

namespace Taz.Core.Slack
{
    public class SlackAuthenticator : IAuthenticator
    {
        private readonly string _token;

        public SlackAuthenticator(string token)
        {
            this._token = token;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddQueryParameter("token", this._token);
        }
    }
}