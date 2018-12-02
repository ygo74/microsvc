using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;


namespace MESF.Core.ServiceManagement
{
    public class Messaging : IMessaging
    {

        private readonly IConfiguration _config;

        public Messaging(IConfiguration config)
        {
            _config = config;
        }

        public Boolean PublishMessage()
        {

            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = _config.GetValue<String>("Messaging:UserName", "guest"),
                Password = _config.GetValue<String>("Messaging:Password", "guest"),
                VirtualHost = _config.GetValue<String>("Messaging:VirtualHost", "/"),
                HostName = _config.GetValue<String>("Messaging:HostName")
            };

            using (IConnection conn = factory.CreateConnection())
            {
                IModel model = conn.CreateModel();

                String exchangeName = "firstExchange";
                String queueName = "firstQueue";
                String routingKey = "service";

                model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                model.QueueDeclare(queueName, false, false, false, null);
                model.QueueBind(queueName, exchangeName, routingKey, null);

                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
                IBasicProperties props = model.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2;
                props.Headers = new Dictionary<string, object>();
                props.Headers.Add("latitude", 51.5252949);
                props.Headers.Add("longitude", -0.0905493);
                props.Expiration = "36000000";


                model.BasicPublish(exchangeName, routingKey, props, messageBodyBytes);

                model.Close();
                conn.Close();
            }





            return true;
        }


        public String ConsumeMessage()
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = _config.GetValue<String>("Messaging:UserName", "guest"),
                Password = _config.GetValue<String>("Messaging:Password", "guest"),
                VirtualHost = _config.GetValue<String>("Messaging:VirtualHost", "/"),
                HostName = _config.GetValue<String>("Messaging:HostName")
            };

            String message = "No Message";

            using (IConnection conn = factory.CreateConnection())
            {
                IModel model = conn.CreateModel();

                String exchangeName = "firstExchange";
                String queueName = "firstQueue";
                String routingKey = "service";

                //var consumer = new EventingBasicConsumer(model);
                //consumer.Received += (ch, ea) =>
                //{
                //    var body = ea.Body;
                //    // ... process the message
                //    model.BasicAck(ea.DeliveryTag, false);
                //};
                //String consumerTag = model.BasicConsume(queueName, false, consumer);

                bool noAck = false;
                BasicGetResult result = model.BasicGet(queueName, noAck);
                if (result != null)
                {
                    IBasicProperties props = result.BasicProperties;
                    byte[] body = result.Body;

                    message = System.Text.Encoding.UTF8.GetString(body);
                    //Confirm message read
                    model.BasicAck(result.DeliveryTag, false);
                }


                model.Close();
                conn.Close();

            }

            return message;


        }
    }
}
