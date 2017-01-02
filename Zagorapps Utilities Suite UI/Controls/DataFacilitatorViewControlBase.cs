namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Threading.Tasks;
    using Commands;
    using Core.Library.Events;
    using Library.Communications;
    using Utilities.Suite.Library;
    using Utilities.Suite.Library.Factories;

    public abstract class DataFacilitatorViewControlBase : ViewControlBase, IDataFacilitatorViewControl
    {
        protected DataFacilitatorViewControlBase(string viewName, IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(viewName, factory, commandProvider)
        {
        }

        public event EventHandler<EventArgs<string, SuiteRoute, string, object>> DataSendRequest;

        public void ProcessMessage(IUtilitiesDataMessage data)
        {
            Task.Run(() =>
            {
                this.HandleProcessMessage(data);
            });
        }

        protected abstract void HandleProcessMessage(IUtilitiesDataMessage data);

        protected void OnDataSendRequest(object sender, string from, SuiteRoute suiteDestination, string viewDestination, object data)
        {
            Invoker.Raise(ref this.DataSendRequest, sender, from, suiteDestination, viewDestination, data);
        }
    }
}