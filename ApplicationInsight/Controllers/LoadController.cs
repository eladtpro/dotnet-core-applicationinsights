using Dasync.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace App.Demo.ApplicationInsight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        private readonly int HitCount;
        private readonly int MaxDegreeOfParallelism;
        private readonly ILogger<NpmController> logger;
        private readonly VulnerabilityController vulnerability;
        private readonly NpmController npm;

        public LoadController(VulnerabilityController vulnerabilityController, NpmController npmController, IConfiguration onfig, ILogger<NpmController> logger)
        {
            this.logger = logger;
            HitCount = onfig.GetValue<int>("RemoteDependecies:DefaultLoadHitCount");
            MaxDegreeOfParallelism = onfig.GetValue<int>("RemoteDependecies:DefaultLoadMaxDegreeOfParallelism");
            vulnerability = vulnerabilityController;
            npm = npmController;
        }

        // GET: api/<NpmController>
        [HttpGet]
        public async Task<ContentResult> Get(int? count, int? maxDegreeOfParallelism)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            int success = 0, failure = 0;
            count ??= HitCount;
            maxDegreeOfParallelism ??= MaxDegreeOfParallelism;

            int[] calls = new int[count.Value];
            ConcurrentBag<object> bag = new ConcurrentBag<object>();
            await calls.ParallelForEachAsync(async item =>
            {
                try
                {
                    ContentResult result = await npm.Search("type");
                    ContentResult result2 = await vulnerability.Get();
                    success++;

                }
                catch (Exception)
                {
                    failure++;
                    throw;
                }
            }, maxDegreeOfParallelism.Value);

            timer.Stop();

            JObject results = new JObject
            {
                new JProperty("duration", timer.Elapsed),
                new JProperty("success", success),
                new JProperty("failure", failure)
            };

            return Content(JsonConvert.SerializeObject(results), "application/json");
        }
    }

}
