namespace Zagorapps.Utilities.Suite.Library.Tasks
{
    using System;
    using Core.Library.Events;
    using NodaTime;

    public abstract class TaskBase : ITask
    {
        private readonly Guid identity;
        private readonly string name, description;
        private readonly TaskType taskType;

        private Instant? lastRan;
        private TaskState state;

        protected TaskBase(Guid? identity, string name, string description, TaskType taskType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "No name has been provided");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException(nameof(description), "No description has been provided");
            }

            this.identity = identity.GetValueOrDefault(Guid.NewGuid());
            this.name = name;
            this.description = description;
            this.taskType = taskType;

            this.state = TaskState.NotStarted;
            this.lastRan = null;
        }

        public event EventHandler<EventArgs<TaskState>> StateChanged;

        public event EventHandler<EventArgs<Exception>> FailureRaised;

        public Guid Identity
        {
            get { return this.identity; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public TaskState State
        {
            get { return this.state; }
        }

        public TaskType TaskType
        {
            get { return this.taskType; }
        }

        public Instant? LastRan
        {
            get { return this.lastRan; }
        }

        public void Execute()
        {
            this.lastRan = SystemClock.Instance.GetCurrentInstant();

            this.OnStateChanged(TaskState.Started);

            try
            {
                this.HandleExecute();
            }
            catch (Exception ex)
            {
                this.OnStateChanged(TaskState.Failed);
                this.OnFailureRaised(ex);
            }
        }

        public void Terminate()
        {
            this.HandleTerminate();

            this.OnStateChanged(TaskState.Cancelled);
        }

        protected abstract void HandleExecute();

        protected abstract void HandleTerminate();

        protected void OnStateChanged(TaskState state)
        {
            this.state = state;
            this.OnStateChanged();
        }

        private void OnStateChanged()
        {
            Invoker.Raise(ref this.StateChanged, this, this.State);
        }

        private void OnFailureRaised(Exception ex)
        {
            Invoker.Raise(ref this.FailureRaised, this, ex);
        }
    }
}