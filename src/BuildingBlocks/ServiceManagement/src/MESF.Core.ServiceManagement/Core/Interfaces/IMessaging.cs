using System;

namespace MESF.Core.ServiceManagement
{
    public interface IMessaging
    {
        string ConsumeMessage();
        bool PublishMessage();
        String GetMessagingConfiguration(String configurationKey);

    }
}