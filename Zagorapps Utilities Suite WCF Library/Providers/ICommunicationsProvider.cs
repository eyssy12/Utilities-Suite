namespace Zagorapps.Utilities.Suite.WCF.Library.Providers
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public interface ICommunicationsProvider
    {
        ICommunicationObject CreateService(object singletonInstance, params Uri[] baseAddresses);

        IChannelFactory<TService> CreateClient<TService>(string endpointConfigurationName);
    }
}