namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Controls;
    using Core.Library.Communications;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Library.Communications;
    using Library.Communications.Suite;
    using Navigation;
    using Utilities.Suite.Library;

    public abstract class DataFacilitatorSuiteBase : SuiteBase, IDataFacilitatorSuite
    {
        private readonly IEnumerable<IReceiveSuiteData> receivers;
        private readonly IEnumerable<ISendSuiteData> senders;
        private readonly IEnumerable<IDataFacilitatorViewControl> dataFacilitatorViews;

        private readonly SuiteRoute route;

        protected DataFacilitatorSuiteBase(string suiteName, SuiteRoute route, IEnumerable<IViewControl> views, IEnumerable<IReceiveSuiteData> receivers = null, IEnumerable<ISendSuiteData> senders = null) 
            : base(suiteName, views)
        {
            this.route = route;

            this.dataFacilitatorViews = this.Navigatables.OfType<IDataFacilitatorViewControl>();
            this.receivers = receivers ?? Enumerable.Empty<IReceiveSuiteData>();
            this.senders = senders ?? Enumerable.Empty<ISendSuiteData>();
        }

        public event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;

        public SuiteRoute Route
        {
            get { return this.route; }
        }

        public bool Start()
        {
            if (this.dataFacilitatorViews.Any())
            {
                this.receivers.ForEach(e =>
                {
                    e.MessageReceived += this.Receiver_MessageReceived;
                    e.Start();
                });

                this.dataFacilitatorViews.ForEach(dataView => dataView.DataSendRequest += this.DataView_DataSendRequest);

                return true;
            }

            return false;
        }

        public bool Stop()
        {
            if (this.dataFacilitatorViews.Any())
            {
                this.receivers.ForEach(e =>
                {
                    e.Stop();
                    e.MessageReceived -= this.Receiver_MessageReceived;
                });

                this.dataFacilitatorViews.ForEach(dataView => dataView.DataSendRequest -= this.DataView_DataSendRequest);

                return true;
            }

            return false;
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
            IDataFacilitatorViewControl view = this.dataFacilitatorViews.FirstOrDefault(d => d.Identifier == message.ViewDestination);

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