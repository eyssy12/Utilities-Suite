namespace Zagorapps.Utilities.Suite.WCF.Library.Senders
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Core.Library.Communications;
    using Providers;
    using Services;
    using Suite.Library;
    using Suite.Library.Communications;

    public class WcfSendSuiteData : ISendSuiteData
    {
        protected readonly ICommunicationsProvider Provider;

        protected readonly string ConfigurationName, EndpointAddress;

        private readonly SuiteRoute route;

        public WcfSendSuiteData(
            ICommunicationsProvider provider,
            SuiteRoute route, 
            string configurationName,
            string endpointAddress)
        {
            this.Provider = provider;
            this.route = route;
        }

        public SuiteRoute Route
        {
            get { return this.route; }
        }

        public void Send(IDataMessage data)
        {
            IUtilitiesDataMessage message = data as IUtilitiesDataMessage;

            if (message != null)
            {
                IChannelFactory<IUtilitiesSuiteService> client = this.Provider.CreateClient<IUtilitiesSuiteService>(this.ConfigurationName);

                try
                {
                    IUtilitiesSuiteService sender = client.CreateChannel(new EndpointAddress(this.EndpointAddress));

                    sender.Handle(message);
                }
                finally
                {
                    if (client.State == CommunicationState.Faulted)
                    {
                        client.Abort();
                    }

                    client.Close();
                }
            }
        }
    }
}