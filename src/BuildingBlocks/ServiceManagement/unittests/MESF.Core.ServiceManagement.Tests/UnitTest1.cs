using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;

namespace MESF.Core.ServiceManagement.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PublishMessage()
        {
            var messaging = new MESF.Core.ServiceManagement.Messaging();
            var result = messaging.PublishMessage();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ConsumeMessage()
        {
            var messaging = new MESF.Core.ServiceManagement.Messaging();
            var result = messaging.ConsumeMessage();
            Assert.IsNotNull(result);
        }

    }
}
