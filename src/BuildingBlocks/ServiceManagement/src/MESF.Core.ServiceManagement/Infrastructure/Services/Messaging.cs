using MESF.Core.ServiceManagement.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MESF.Core.ServiceManagement.Services
{
    public class Messaging : IMessaging
    {

        private readonly IRabbitMQPersistentConnection _connection;
        private readonly ILogger<Messaging> _logger;
        private readonly int _retryCount = 5;

        public Messaging(IRabbitMQPersistentConnection connection, ILogger<Messaging> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public Boolean PublishMessage(IntegrationEvent @event)
        {
            //Ensure Connection to Broker
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            //Prepare policy for publishing
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                           .Or<SocketException>()
                           .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                           {
                               _logger.LogWarning(ex.ToString());
                           });

            using (var model = _connection.CreateModel())
            {

                var eventName = @event.GetType()
                                .Name;

                String exchangeName = "firstExchange";
                String queueName = "firstQueue";
                String routingKey = "service";

                //model.QueueDeclare(queueName, false, false, false, null);
                //model.QueueBind(queueName, exchangeName, routingKey, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = System.Text.Encoding.UTF8.GetBytes(message);
                //byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
                //IBasicProperties props = model.CreateBasicProperties();
                //props.ContentType = "text/plain";
                //props.DeliveryMode = 2;
                //props.Headers = new Dictionary<string, object>();
                //props.Headers.Add("latitude", 51.5252949);
                //props.Headers.Add("longitude", -0.0905493);
                //props.Expiration = "36000000";

                policy.Execute(() =>
                {
                    model.ExchangeDeclare(exchangeName, ExchangeType.Direct);

                    var properties = model.CreateBasicProperties();
                    properties.DeliveryMode = 2; //persistent

                    model.BasicPublish(exchange: exchangeName,
                                       routingKey: eventName,
                                       mandatory: true,
                                       basicProperties: properties,
                                       body: body);

                });
                model.Close();
            }


            return true;
        }


        public String ConsumeMessage()
        {
            String message = String.Empty;

            //Ensure Connection to Broker
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            //Prepare policy for publishing
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                           .Or<SocketException>()
                           .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                           {
                               _logger.LogWarning(ex.ToString());
                           });

            using (var model = _connection.CreateModel())
            {
                String exchangeName = "firstExchange";
                String queueName = "firstQueue";
                String routingKey = "service";

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

                return message;
            }
        }


        public List<String> ConsumeMessages(String queueName, String routingKey)
        {
            List<String> results = new List<String>();

            //Ensure Connection to Broker
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            //Prepare policy for publishing
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                           .Or<SocketException>()
                           .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                           {
                               _logger.LogWarning(ex.ToString());
                           });

            using (var model = _connection.CreateModel())
            {
                String exchangeName = "firstExchange";

                policy.Execute(() => 
                {
                    //model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                    model.QueueDeclare(queueName, false, false, false, null);
                    model.QueueBind(queueName, exchangeName, routingKey, null);

                    var @continue= true;
                    while(@continue)
                    {
                        bool noAck = false;
                        BasicGetResult result = model.BasicGet(queueName, noAck);
                        if (result != null)
                        {
                            IBasicProperties props = result.BasicProperties;
                            byte[] body = result.Body;

                            var message = System.Text.Encoding.UTF8.GetString(body);
                            results.Add(message);
                            //Confirm message read
                            model.BasicAck(result.DeliveryTag, false);
                        }
                        else
                        {
                            @continue = false;
                        }
                    }

                });

            }

            return results;
        }

    }
}
