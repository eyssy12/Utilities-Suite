namespace Zagorapps.Utilities.Suite.WCF.Library.Receivers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Core.Library.Events;
    using Providers;
    using Services;
    using Suite.Library.Communications;
    using Utilities.Library.Factories;

    public class WcfReceiveSuiteData : IReceiveSuiteData
    {
        protected readonly IEnumerable<Uri> Addresses;
        protected readonly IOrganiserFactory Factory;
        protected readonly ICommunicationsProvider Provider;

        private IUtilitiesSuiteService service;
        private ICommunicationObject host;

        public WcfReceiveSuiteData(IOrganiserFactory factory, ICommunicationsProvider provider, params string[] addresses)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory), "factory missing");
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "provider missing");
            }

            this.Factory = factory;
            this.Provider = provider;
            this.Addresses = addresses == null
                ? Enumerable.Empty<Uri>()
                : addresses.Select(address => new Uri(address)).ToArray();
        }

        public event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;

        public bool Start()
        {
            this.service = this.Factory.Create<IUtilitiesSuiteService>();

            this.service.MessageReceived += this.Service_MessageReceived;
            this.host = this.Provider.CreateService(this.service, this.Addresses.ToArray());

            this.host.Open();

            return true;
        }

        public bool Stop()
        {
            this.service.MessageReceived -= this.Service_MessageReceived;

            if (this.host != null)
            {
                this.host.Close();
                this.host = null;
            }

            this.service = null;

            return true;
        }

        private void Service_MessageReceived(object sender, EventArgs<IUtilitiesDataMessage> e)
        {
            Invoker.Raise(ref this.MessageReceived, this, e.First);
        }
    }
}