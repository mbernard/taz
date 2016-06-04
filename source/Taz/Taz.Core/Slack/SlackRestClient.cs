using System;
using System.Threading.Tasks;

using RestSharp;

using Taz.Core.Tokens;

namespace Taz.Core.Slack
{
    public class SlackRestClient
    {
        #region Fields

        private readonly RestClient _client;

        #endregion

        #region Constructors

        public SlackRestClient(User user)
        {
            var token = TokenLoader.GetTokenFor(user);
            this._client = new RestClient("https://slack.com/api/");
            this._client.Authenticator = new SlackAuthenticator(token);
        }

        #endregion

        #region Methods

        public async Task<IRestResponse<T>> GetAsync<T>(string resource, params Tuple<string, object>[] parameters)
        {
            var request = new RestRequest($"api/{resource}", Method.GET);

            foreach (var parameter in parameters)
            {
                request.AddParameter(parameter.Item1, parameter.Item2);
            }

            return await this._client.ExecuteTaskAsync<T>(request);
        }

        #endregion
    }
}