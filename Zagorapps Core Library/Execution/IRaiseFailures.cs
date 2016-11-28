namespace Zagorapps.Core.Library.Execution
{
    using System;
    using Events;

    public interface IRaiseFailures
    {
        event EventHandler<EventArgs<Exception>> FailureRaised;
    }
}