namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Core.Library.Events;
    using Library;
    using Library.Communications;
    using Navigation;

    public interface IDataFacilitatorViewControl : IViewControl
    {
        event EventHandler<EventArgs<SuiteRoute, string, object>> DataSendRequest;

        void ProcessMessage(IUtilitiesDataMessage data);
    }
}