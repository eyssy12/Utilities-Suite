namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Core.Library.Events;

    public abstract class TaskBase : ITask
    {
        private readonly Guid identity;
        private readonly string description;
        private readonly TaskType taskType;

        private TaskState state;

        protected TaskBase(Guid identity, string description, TaskType taskType)
        {
            this.identity = identity;
            this.description = description;
            this.taskType = taskType;

            this.state = TaskState.NotStarted;
        }

        public event EventHandler<EventArgs<TaskState>> StateChanged;

        public event EventHandler<EventArgs<Exception>> FailureRaised;

        public Guid Identity
        {
            get { return this.identity; }
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

        public void Execute()
        {
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