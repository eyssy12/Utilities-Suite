namespace Zagorapps.Organiser.Library.Tasks
{
    using System;
    using Core.Library.Events;
    using Core.Library.Timing;

    public class ScheduledTask : TaskBase
    {
        public const string DescriptionFormat = "Executes task '{0}' with description '{1}'";

        public const int MinimumInitialWaitTime = 10000,
            MinimumTimerPeriod = 10000;

        protected readonly ITimer Timer;
        protected readonly ITask Executable;

        protected readonly int InitialWaitTime,
            TimerPeriod;

        public ScheduledTask(
            string name,
            string description,
            ITimer timer, 
            ITask executable,
            Guid? identity = null,
            int initialWaitTime = ScheduledTask.MinimumInitialWaitTime, 
            int timerPeriod = ScheduledTask.MinimumTimerPeriod)
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
            this.InitialWaitTime = Math.Max(ScheduledTask.MinimumInitialWaitTime, initialWaitTime);
            this.TimerPeriod = Math.Max(ScheduledTask.MinimumTimerPeriod, timerPeriod);
        }

        protected override void HandleExecute()
        {
            this.Timer.TimeElapsed += this.Timer_TimeElapsed;

            this.Timer.Start(this.InitialWaitTime, this.TimerPeriod);

            this.OnStateChanged(TaskState.Pending);
        }

        protected override void HandleTerminate()
        {
            this.Timer.Stop();

            this.Timer.TimeElapsed -= this.Timer_TimeElapsed;
        }

        private void Timer_TimeElapsed(object sender, EventArgs<int> e)
        {
            this.OnStateChanged(TaskState.Started);

            this.Executable.Execute();

            this.OnStateChanged(TaskState.Finished | TaskState.Pending);
        }
    }
}