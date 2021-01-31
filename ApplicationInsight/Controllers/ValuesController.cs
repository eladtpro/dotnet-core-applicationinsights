using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Demo.ApplicationInsight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private const string Operation = "Operation";
        private static Lazy<ConcurrentDictionary<int, string>> data = new Lazy<ConcurrentDictionary<int, string>>();
        private static ConcurrentDictionary<int, string> Data => data.Value;

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IDictionary<int, string>> Get()
        {
            return await Task.Run(() =>
            {
                Activity.Current?.AddTag(Operation, $"{nameof(ValuesController)}.{MethodBase.GetCurrentMethod().Name}");
                return Data;
            });

        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return await Task.Run(() =>
            {
                Activity.Current?.AddTag(Operation, $"{nameof(ValuesController)}.{MethodBase.GetCurrentMethod().Name}");
                if (!Data.TryGetValue(id, out string value))
                    throw new ArgumentException($"Id {id} not exists in collection.", nameof(id));
                return value;

            });
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<int> Post([FromBody] string value)
        {
            return await Task.Run(() =>
            {
                int id = (Data.Keys.Count < 1) ? 0 : Data.Keys.Max(v => v) + 1;
                Activity.Current?.AddTag(Operation, $"{nameof(ValuesController)}.{MethodBase.GetCurrentMethod().Name}");
                if (!Data.TryAdd(id, value))
                    throw new ArgumentException($"Id {id} already exists in collection.", nameof(id));
                return id;
            });
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            await Task.Run(() =>
            {
                Activity.Current?.AddTag(Operation, $"{nameof(ValuesController)}.{MethodBase.GetCurrentMethod().Name}");
                if (!Data.ContainsKey(id))
                    throw new ArgumentException($"Id {id} not exists in collection.", nameof(id));
                Data[id] = value;
            });

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            return await Task.Run(() =>
            {
                Activity.Current?.AddTag(Operation, $"{nameof(ValuesController)}.{MethodBase.GetCurrentMethod().Name}");
                if (!Data.TryRemove(id, out string value))
                    throw new ArgumentException($"Could not find entry with Id {id}.", nameof(id));
                return value;
            });
        }
    }
}
