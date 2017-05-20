namespace Zagorapps.Utilities.Suite.Library.Tasks
{
    using System;
    using Core.Library.Events;
    using Core.Library.Timing;
    using NodaTime;

    public class ScheduledTask : TaskBase, IScheduledTask
    {
        public const string DescriptionFormat = "Executes task '{0}' with description '{1}'";

        public const int MinimumInitialWaitTimeInMs = 10000,
            MinimumTimerPeriodInMs = 10000;

        protected readonly ITimer Timer;
        protected readonly ITask Executable;

        protected readonly int InitialWaitTime,
            TimerPeriod;

        private Instant? nextScheduled;

        public ScheduledTask(
            string name,
            string description,
            ITimer timer, 
            ITask executable,
            Guid? identity = null,
            int initialWaitTime = ScheduledTask.MinimumInitialWaitTimeInMs, 
            int timerPeriod = ScheduledTask.MinimumTimerPeriodInMs)
            : base(identity, name, description, TaskType.Scheduled)
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
            this.InitialWaitTime = Math.Max(ScheduledTask.MinimumInitialWaitTimeInMs, initialWaitTime);
            this.TimerPeriod = Math.Max(ScheduledTask.MinimumTimerPeriodInMs, timerPeriod);
        }

        public Instant? NextScheduled
        {
            get { return this.nextScheduled; }
        }

        protected override void HandleExecute()
        {
            this.Timer.TimeElapsed += this.Timer_TimeElapsed;

            this.nextScheduled = SystemClock.Instance.GetCurrentInstant().Plus(Duration.FromMilliseconds(this.TimerPeriod));

            this.Timer.Start(this.InitialWaitTime, this.TimerPeriod);

            this.OnStateChanged(TaskState.Pending);
        }

        protected override void HandleTerminate()
        {
            this.Timer.Stop();

            this.Timer.TimeElapsed -= this.Timer_TimeElapsed;

            this.nextScheduled = null;
        }

        private void Timer_TimeElapsed(object sender, EventArgs<int> e)
        {
            this.nextScheduled = SystemClock.Instance.GetCurrentInstant().Plus(Duration.FromMilliseconds(this.TimerPeriod));

            this.OnStateChanged(TaskState.Started);

            this.Executable.Execute();

            this.OnStateChanged(TaskState.Finished | TaskState.Pending);
        }
    }
}