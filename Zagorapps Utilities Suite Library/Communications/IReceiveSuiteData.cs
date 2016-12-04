namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using System;
    using Core.Library.Events;

    public interface IReceiveSuiteData
    {
        event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;
    }
}