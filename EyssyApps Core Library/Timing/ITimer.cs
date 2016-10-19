namespace EyssyApps.Core.Library.Timing
{
    using System;
    using Events;

    public interface ITimer
    {
        event EventHandler<EventArgs<int>> TimeElapsed;

        int CurrentTime { get; }

        bool IsStarted { get; }

        bool Start(int dueTime, int period);

        bool Stop();
    }
}