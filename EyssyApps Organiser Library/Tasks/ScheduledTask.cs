namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Core.Library.Events;
    using Core.Library.Timing;

    public class ScheduledTask : TaskBase
    {
        protected readonly ITimer Timer;
        protected readonly ITask Executable;

        protected readonly int InitialWaitTime,
            TimerPeriod;

        public ScheduledTask(Guid id, string description, ITimer timer, ITask executable, int initialWaitTime, int timerPeriod)
            : base(id, description, TaskType.Scheduled)
        {
            if (timer == null)
            {
                throw new ArgumentNullException(nameof(timer), "Timer not provided"); // TODO: more verbose messages, also add to resources
            }

            if (executable == null)
            {
                throw new ArgumentNullException(nameof(executable), "Executable not provided");
            }

            this.Timer = timer;
            this.Executable = executable;
            this.InitialWaitTime = initialWaitTime;
            this.TimerPeriod = timerPeriod;
        }

        protected override void HandleExecute()
        {
            this.Timer.TimeElapsed += this.Timer_TimeElapsed;

            this.Timer.Start(this.InitialWaitTime, this.TimerPeriod);
        }

        protected override void HandleTerminate()
        {
            this.Timer.Stop();

            this.Timer.TimeElapsed -= this.Timer_TimeElapsed;
        }

        private void Timer_TimeElapsed(object sender, EventArgs<int> e)
        {
            this.Executable.Execute();
        }
    }
}