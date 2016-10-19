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

        public ScheduledTask(ITimer timer, ITask executable, int initialWaitTime, int timerPeriod)
        {
            this.id = Guid.NewGuid();

            this.Timer = timer;
            this.Executable = executable;
            this.InitialWaitTime = initialWaitTime;
            this.TimerPeriod = timerPeriod;
        }

        public Guid Id
        {
            get { return this.id; }
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