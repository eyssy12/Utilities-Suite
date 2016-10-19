namespace EyssyApps.Core.Library.Timing
{
    using System;
    using System.Threading;
    using Events;
    using Extensions;

    public class ThreadedTimer : ITimer
    {
        private readonly Lazy<Timer> timer;

        public ThreadedTimer()
        {
            this.timer = new Lazy<Timer>(() => new Timer(this.OnTimeElapsed));
        }

        public event EventHandler<EventArgs<int>> TimeElapsed;

        public int CurrentTime
        {
            get { return Environment.TickCount; }
        }

        public bool IsStarted { get; private set; }

        protected Timer Timer
        {
            get { return this.timer.Value; }
        }

        public bool Start(int dueTime, int period)
        {
            return this.IsStarted = this.Timer.Change(dueTime, period);
        }

        public bool Stop()
        {
            if (this.IsStarted)
            {
                bool updated = this.Timer.Change(Timeout.Infinite, Timeout.Infinite);

                this.IsStarted = !updated;

                return updated;
            }
            else
            {
                throw new InvalidOperationException("stop cannot be done");
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.timer.TryDispose();
            }
        }

        private void OnTimeElapsed(object state)
        {
            Invoker.Raise(ref this.TimeElapsed, this, this.CurrentTime);
        }
    }
}