namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Controls;
    using Core.Library.Communications;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Library;
    using Library.Communications;
    using Navigation;

    public abstract class DataFacilitatorSuiteBase : SuiteBase, IDataFacilitatorSuite
    {
        private readonly IEnumerable<IReceiveSuiteData> receivers;
        private readonly IEnumerable<ISendSuiteData> senders;
        private readonly IEnumerable<IDataFacilitatorViewControl> dataViews;

        private readonly SuiteRoute route;

        protected DataFacilitatorSuiteBase(string suiteName, SuiteRoute route, IEnumerable<IViewControl> views, IEnumerable<IReceiveSuiteData> receivers = null, IEnumerable<ISendSuiteData> senders = null) 
            : base(suiteName, views)
        {
            this.route = route;

            this.dataViews = this.Navigatables.OfType<IDataFacilitatorViewControl>();
            this.receivers = receivers ?? Enumerable.Empty<IReceiveSuiteData>();
            this.senders = senders ?? Enumerable.Empty<ISendSuiteData>();
        }

        public SuiteRoute Route
        {
            get { return this.route; }
        }

        public event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;

        public bool Start()
        {
            this.receivers.ForEach(e =>
            {
                e.MessageReceived += this.Receiver_MessageReceived;
                e.Start();
            });

            this.dataViews.ForEach(dataView => dataView.DataSendRequest += this.DataView_DataSendRequest);

            return true;
        }

        public bool Stop()
        {
            this.receivers.ForEach(e =>
            {
                e.Stop();
                e.MessageReceived -= this.Receiver_MessageReceived;
            });

            this.dataViews.ForEach(dataView => dataView.DataSendRequest -= this.DataView_DataSendRequest);

            return true;
        }

        public void Send(IDataMessage data)
        {
            IUtilitiesDataMessage message = data as IUtilitiesDataMessage;

            if (message != null)
            {
                if (message.SuiteDestination == this.Route)
                {
                    this.HandleNeighbouringSend(message);
                }
                else
                {
                    this.HandleExternalSend(message);
                }
            }
        }

        protected void HandleNeighbouringSend(IUtilitiesDataMessage message)
        {
            IDataFacilitatorViewControl view = this.dataViews.FirstOrDefault(d => d.Identifier == message.ViewDestination);

            if (view != null)
            {
                view.ProcessMessage(message);
            }
        }

        protected void HandleExternalSend(IUtilitiesDataMessage message)
        {
            ISendSuiteData sender = this.senders.FirstOrDefault(s => s.Route == message.SuiteDestination);

            if (sender != null)
            {
                sender.Send(message);
            }
        }

        private void Receiver_MessageReceived(object sender, EventArgs<IUtilitiesDataMessage> e)
        {
            this.HandleNeighbouringSend(e.First);
        }

        private void DataView_DataSendRequest(object sender, EventArgs<string, SuiteRoute, string, object> e)
        {
            IUtilitiesDataMessage message = new UtilitiesDataMessage(e.First, e.Second, e.Third, e.Fourth);

            this.Send(message);
        }
    }
}