namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Core.Library.Events;

    public abstract class TaskBase : ITask
    {
        private readonly Guid id;
        private readonly string description;
        private readonly TaskType taskType;

        private TaskState state;

        protected TaskBase(Guid id, string description, TaskType taskType)
        {
            this.id = id;
            this.description = description;
            this.taskType = taskType;

            this.state = TaskState.NotStarted;
        }

        public event EventHandler<EventArgs<TaskState>> StateChanged;

        public event EventHandler<EventArgs<Exception>> FailureRaised;

        public Guid Id
        {
            get { return this.id; }
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
            this.OnStateChanged(TaskState.Running);

            try
            {
                this.HandleExecute();

                this.OnStateChanged(TaskState.Finished);
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

        private void OnStateChanged(TaskState state)
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