namespace Zagorapps.Utilities.Library.Communications.Suite
{
    using System;
    using Core.Library.Events;
    using Core.Library.Execution;

    public interface IReceiveSuiteData : IProcess
    {
        event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;
    }
}