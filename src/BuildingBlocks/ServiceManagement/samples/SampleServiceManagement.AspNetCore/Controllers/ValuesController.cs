using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MESF.Core.ServiceManagement;
using MESF.Core.ServiceManagement.Core.Entities;
using MESF.Core.ServiceManagement.Core.Interfaces;
using MESF.Core.ServiceManagement.Core.Services;
using MESF.Core.ServiceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SampleServiceManagement.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly ILogger<ValuesController> _logger;
        private readonly IMessaging _messaging;
        private readonly IAsyncRepository<HostedService> _repo;
        private readonly ServiceManagementContext _dbContext;

        public ValuesController(ILogger<ValuesController> logger,
                                IMessaging messaging, 
                                IAsyncRepository<HostedService> repo,
                                ServiceManagementContext dbContext)
        {
            _logger = logger;
            _messaging = messaging;
            _repo = repo;
            _dbContext = dbContext;

            _logger.LogInformation("Start ValuesController");
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var testdb = new HostedServiceService(_repo);
            //await testdb.AddNewHostedService("test1");
            var services = await _repo.ListAllAsync();

            //return Ok(new string[] { "value1", response });
            return Ok(services);
        }

        [HttpGet("GetById/{id}", Name = "GetById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _repo.GetByIdAsync(id);
            if (null == service)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // GET api/values/5
        [HttpGet("{key}")]
        public string Get(String key)
        {
            return String.Format("value for key {0} : {1}", key, _messaging.GetMessagingConfiguration(key));
        }


        
        // POST api/values
        [HttpPost("New", Name = "NewService")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<HostedService> CreateService(HostedService service)
        {
            _dbContext.Services.Add(service);
            _dbContext.SaveChanges();

            return CreatedAtRoute("GetById", new { id = service.Id }, service);

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
