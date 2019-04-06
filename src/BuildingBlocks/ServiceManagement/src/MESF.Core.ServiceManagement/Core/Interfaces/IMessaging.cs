using MESF.Core.ServiceManagement.Infrastructure.Services;
using System;
using System.Collections.Generic;

namespace MESF.Core.ServiceManagement
{
    public interface IMessaging
    {
        String ConsumeMessage();
        List<String> ConsumeMessages(String queueName, String routingKey);
        bool PublishMessage(IntegrationEvent @event);
        //String GetMessagingConfiguration(String configurationKey);
    }
}