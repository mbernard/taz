using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json.Linq;

using Taz.Models;

namespace Taz.Controllers
{
    [RoutePrefix("api/taz")]
    public class TazController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task Post()
        {
            dynamic commandObj = await this.Request.Content.ReadAsAsync<JObject>();
            var command = commandObj.ToObject<SlackCommand>();
        }
    }
}
