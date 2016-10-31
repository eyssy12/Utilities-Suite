namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Runtime.Serialization;
    using Core.Library.Events;
    using Core.Library.Timing;

    public class ScheduledTask : ITask
    {
        protected readonly ITimer Timer;
        protected readonly ITask Executable;

        protected readonly int InitialWaitTime,
            TimerPeriod;

        private readonly Guid id;
        private readonly string description;

        public ScheduledTask(ITimer timer, ITask executable, int initialWaitTime, int timerPeriod)
        {
            if (timer == null)
            {
                throw new ArgumentNullException(nameof(timer), "Timer not provided"); // TODO: more verbose messages, also add to resources
            }

            if (executable == null)
            {
                throw new ArgumentNullException(nameof(executable), "Executable not provided");
            }

            this.id = Guid.NewGuid();

            this.Timer = timer;
            this.Executable = executable;
            this.InitialWaitTime = initialWaitTime;
            this.TimerPeriod = timerPeriod;

            this.description = "Scheduled task that executes another task containg the desciption: '" + executable.Description + "'"; // TODO: const format
        }

        public Guid Id
        {
            get { return this.id; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public void Execute()
        {
            this.Timer.TimeElapsed += this.Timer_TimeElapsed;

            this.Timer.Start(this.InitialWaitTime, this.TimerPeriod);
        }

        public void Terminate()
        {
            this.Timer.Stop();

            this.Timer.TimeElapsed -= this.Timer_TimeElapsed;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        private void Timer_TimeElapsed(object sender, EventArgs<int> e)
        {
            this.Executable.Execute();
        }
    }
}