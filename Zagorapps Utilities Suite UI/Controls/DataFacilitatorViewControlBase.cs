namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Commands;
    using Core.Library.Events;
    using Library;
    using Utilities.Library.Factories;
    using Zagorapps.Utilities.Suite.Library.Communications;

    public abstract class DataFacilitatorViewControlBase : ViewControlBase, IDataFacilitatorViewControl
    {
        protected DataFacilitatorViewControlBase(string viewName, IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(viewName, factory, commandProvider)
        {
        }

        public event EventHandler<EventArgs<SuiteRoute, string, object>> DataSendRequest;

        public abstract void ProcessMessage(IUtilitiesDataMessage data);

        protected void OnDataSendRequest(object sender, SuiteRoute suiteDestination, string viewDestination, object data)
        {
            Invoker.Raise(ref this.DataSendRequest, sender, suiteDestination, viewDestination, data);
        }
    }
}