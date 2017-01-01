namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Core.Library.Events;
    using Library.Communications;
    using Navigation;
    using Utilities.Suite.Library;

    public interface IDataFacilitatorViewControl : IViewControl
    {
        event EventHandler<EventArgs<string, SuiteRoute, string, object>> DataSendRequest;

        void ProcessMessage(IUtilitiesDataMessage data);
    }
}