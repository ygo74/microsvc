using MESF.Core.ServiceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MESF.Core.ServiceManagement.Infrastructure
{
    public class ServiceSpecifications : BaseSpecification<HostedService>
    {
        public ServiceSpecifications(int serviceId) : base(s => s.Id == serviceId)
        {
        }
    }
}
