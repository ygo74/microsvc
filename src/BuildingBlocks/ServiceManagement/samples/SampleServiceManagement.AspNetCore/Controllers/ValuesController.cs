using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MESF.Core.ServiceManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SampleServiceManagement.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private readonly IMessaging _messaging;
        public ValuesController(IMessaging messaging)
        {
            _messaging = messaging;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var response = _messaging.ConsumeMessage();

            return new string[] { "value1", response };
        }

        // GET api/values/5
        [HttpGet("{key}")]
        public string Get(String key)
        {
            return _messaging.GetMessagingConfiguration(key);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            _messaging.PublishMessage();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
