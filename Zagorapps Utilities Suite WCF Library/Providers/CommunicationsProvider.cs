namespace Zagorapps.Utilities.Suite.WCF.Library.Providers
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class CommunicationsProvider : ICommunicationsProvider
    {
        public ICommunicationObject CreateService(object singletonInstance, params Uri[] baseAddresses)
        {
            return new ServiceHost(singletonInstance, baseAddresses);
        }

        public IChannelFactory<TService> CreateClient<TService>(string endpointConfigurationName)
        {
            return new ChannelFactory<TService>(endpointConfigurationName);
        }
    }
}