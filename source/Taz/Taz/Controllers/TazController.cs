using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json.Linq;

namespace Taz.Controllers
{
    [RoutePrefix("api/taz")]
    public class TazController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task Post()
        {
            // https://api.slack.com/slash-commands#triggering_a_command
            dynamic command = await this.Request.Content.ReadAsAsync<JObject>();
        }
    }
}
