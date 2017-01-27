namespace Zagorapps.Core.Library.Timing
{
    using System;
    using Events;

    public interface ITimer : IDisposable
    {
        event EventHandler<EventArgs<int>> TimeElapsed;

        int CurrentTime { get; }

        bool IsStarted { get; }

        bool Start(int dueTime, int period);

        bool Stop();
    }
}