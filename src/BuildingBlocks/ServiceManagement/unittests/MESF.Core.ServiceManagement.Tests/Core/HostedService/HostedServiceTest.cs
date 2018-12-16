using MESF.Core.ServiceManagement.Core.Interfaces;
using MESF.Core.ServiceManagement.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using MESF.Core.ServiceManagement.Core.Services;
using System.Threading.Tasks;

namespace MESF.Core.ServiceManagement.Tests.Core.HostedServiceTest
{
    [TestClass]
    public class HostedServiceTest
    {
        private Mock<IAsyncRepository<HostedService>> _hostedServiceRepository;

        public HostedServiceTest()
        {
            _hostedServiceRepository = new Mock<IAsyncRepository<HostedService>>();
        }

        [TestMethod]
        public async Task AddNewHostedService()
        {

            var hostedServiceService = new HostedServiceService(_hostedServiceRepository.Object);
            await hostedServiceService.AddNewHostedService("test");

            Assert.IsTrue(true);
        }
    }
}
