using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;


namespace MESF.Core.ServiceManagement.Tests
{
    [TestClass]
    public class UnitTest1
    {

        IConfiguration _config;

        [TestMethod]
        public void PublishMessage()
        {
            var messaging = new MESF.Core.ServiceManagement.Messaging(_config);
            var result = messaging.PublishMessage();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ConsumeMessage()
        {
            var messaging = new MESF.Core.ServiceManagement.Messaging(_config);
            var result = messaging.ConsumeMessage();
            Assert.IsNotNull(result);
        }

    }
}
