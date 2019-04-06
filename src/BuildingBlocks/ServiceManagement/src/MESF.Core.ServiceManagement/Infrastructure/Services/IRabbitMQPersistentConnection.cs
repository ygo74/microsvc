using RabbitMQ.Client;
using System;

namespace MESF.Core.ServiceManagement.Infrastructure.Services
{
    public interface IRabbitMQPersistentConnection
         : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
