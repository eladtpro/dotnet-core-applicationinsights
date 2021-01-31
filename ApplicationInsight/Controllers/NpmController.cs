using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Demo.ApplicationInsight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NpmController : ControllerBase
    {
        private readonly ILogger<NpmController> logger;
        private readonly string baseUrl;
        public readonly int timeout;

        public NpmController(IConfiguration config, ILogger<NpmController> logger)
        {
            this.logger = logger;
            baseUrl = config["RemoteDependecies:NpmRepository:Url"];
            timeout = config.GetValue<int>("RemoteDependecies:Timeout");
        }

        // GET: api/<NpmController>
        [HttpGet("{query?}")]
        public async Task<ContentResult> Search(string query)
        {
            RestClient client = new RestClient($"{baseUrl}?q={query}") { Timeout = timeout };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = await client.ExecuteAsync(request);

            Activity.Current?.AddTag("Content", $"{nameof(NpmController)}.{MethodBase.GetCurrentMethod().Name}");
            logger.LogInformation(response.Content);

            JToken result = JToken.Parse(response.Content);

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
    }
}
