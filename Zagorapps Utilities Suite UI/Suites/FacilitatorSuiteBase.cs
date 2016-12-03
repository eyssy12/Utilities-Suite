namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Core.Library.Communications;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Library.Facilitators;
    using Navigation;

    public abstract class FacilitatorSuiteBase : SuiteBase
    {
        protected readonly IFacilitateDataProcessing Facilitator;

        protected FacilitatorSuiteBase(string suiteName, IEnumerable<IViewControl> views, IFacilitateDataProcessing facilitator) 
            : base(suiteName, views)
        {

            this.Facilitator = facilitator;
            this.Facilitator.Receivers.ForEach(e =>
            {
                e.MessageReceived += this.Receiver_MessageReceived;
            });
        }

        private void Receiver_MessageReceived(object sender, EventArgs<IDataMessage> e)
        {

        }
    }
}