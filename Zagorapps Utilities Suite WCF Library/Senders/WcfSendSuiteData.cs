namespace Zagorapps.Utilities.Suite.WCF.Library.Senders
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    using Core.Library.Communications;
    using Providers;
    using Services;
    using Utilities.Library;
    using Utilities.Library.Communications;
    using Utilities.Library.Communications.Suite;

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
            this.ConfigurationName = configurationName;
            this.EndpointAddress = endpointAddress;
            this.route = route;
        }

        public SuiteRoute Route
        {
            get { return this.route; }
        }

        public async void Send(IDataMessage data)
        {
            IUtilitiesDataMessage message = data as IUtilitiesDataMessage;

            if (message != null)
            {
                IChannelFactory<IUtilitiesSuiteService> client = this.Provider.CreateClient<IUtilitiesSuiteService>(this.ConfigurationName);

                try
                {
                    IUtilitiesSuiteService sender = client.CreateChannel(new EndpointAddress(this.EndpointAddress));

                    await Task.Run(() => sender.Handle(message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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