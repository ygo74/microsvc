using MESF.Core.ServiceManagement.Core.Entities;
using MESF.Core.ServiceManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MESF.Core.ServiceManagement.Core.Services
{
    public class HostedServiceService
    {

        private readonly IAsyncRepository<HostedService> _hostedServiceRepository;

        public HostedServiceService(IAsyncRepository<HostedService> hostedServiceRepository)
        {
            _hostedServiceRepository = hostedServiceRepository;
        }

        public async Task AddNewHostedService(string serviceName)
        {
            var svc = new HostedService()
            {
                Name = serviceName
            };

            await _hostedServiceRepository.AddAsync(svc);
        }
    }
}
