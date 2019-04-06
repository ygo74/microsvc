using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MESF.Core.ServiceManagement.Infrastructure.Services;
using MESF.Core.ServiceManagement.Services;

namespace MESF.Core.ServiceManagement.Tests.Infrastructure.Services
{
    [TestClass]
    public class DefaultRabbitMQPersisterConnectionTest
    {

        ConnectionFactory _rabbitMqConnection;

        public DefaultRabbitMQPersisterConnectionTest()
        {
            var config = new ConfigurationBuilder()
                             .AddJsonFile("appsettings.json")
                             .Build();


            Console.WriteLine(config["Messaging:HostName"]);

            _rabbitMqConnection = new ConnectionFactory()
            {
                UserName = config.GetValue<String>("Messaging:UserName", "guest"),
                Password = config.GetValue<String>("Messaging:Password", "guest"),
                VirtualHost = config.GetValue<String>("Messaging:VirtualHost", "/"),
                HostName = config.GetValue<String>("Messaging:HostName")
            };
            
        }

        [TestMethod]
        public void TryConnect()
        {
            Mock<ILogger<DefaultRabbitMQPersistentConnection>> logger = new Mock<ILogger<DefaultRabbitMQPersistentConnection>>();

            using (DefaultRabbitMQPersistentConnection con = new DefaultRabbitMQPersistentConnection(_rabbitMqConnection, logger.Object))
            {
                var result = con.TryConnect();
                Assert.IsTrue(result);
            }              
        }

        [TestMethod]
        public void PublishMessage()
        {
            Mock<ILogger<DefaultRabbitMQPersistentConnection>> logger = new Mock<ILogger<DefaultRabbitMQPersistentConnection>>();
            Mock<ILogger<Messaging>> logger2 = new Mock<ILogger<Messaging>>();

            using (DefaultRabbitMQPersistentConnection con = new DefaultRabbitMQPersistentConnection(_rabbitMqConnection, logger.Object))            
            {
                Messaging msg = new Messaging(con, logger2.Object);

                MessageBusTest message = new MessageBusTest()
                {
                    Name = "test"
                };

                var result = msg.PublishMessage(message);
                
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ConsumeMessage()
        {
            Mock<ILogger<DefaultRabbitMQPersistentConnection>> logger = new Mock<ILogger<DefaultRabbitMQPersistentConnection>>();
            Mock<ILogger<Messaging>> logger2 = new Mock<ILogger<Messaging>>();

            using (DefaultRabbitMQPersistentConnection con = new DefaultRabbitMQPersistentConnection(_rabbitMqConnection, logger.Object))
            {
                Messaging msg = new Messaging(con, logger2.Object);

                //msg.PublishMessage();
                var result = msg.ConsumeMessage();

                Assert.AreEqual("new message", result);
            }
        }

        [TestMethod]
        public void ConsumeMessages()
        {
            Mock<ILogger<DefaultRabbitMQPersistentConnection>> logger = new Mock<ILogger<DefaultRabbitMQPersistentConnection>>();
            Mock<ILogger<Messaging>> logger2 = new Mock<ILogger<Messaging>>();

            using (DefaultRabbitMQPersistentConnection con = new DefaultRabbitMQPersistentConnection(_rabbitMqConnection, logger.Object))
            {
                Messaging msg = new Messaging(con, logger2.Object);

                //msg.PublishMessage();
                var result = msg.ConsumeMessages("testQueue", "MessageBusTest");

                Assert.AreEqual("new message", result);
            }
        }


    }

}
