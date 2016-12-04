namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using System;
    using Core.Library.Events;
    using Core.Library.Execution;

    public interface IReceiveSuiteData : IProcess
    {
        event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;
    }
}