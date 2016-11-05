namespace EyssyApps.Organiser.Library.Tasks
{
    using System;

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
            this.state = TaskState.Running;

            this.HandleExecute();

            this.state = TaskState.Finished;
        }

        public void Terminate()
        {
            this.HandleTerminate();

            this.state = TaskState.Cancelled;
        }

        protected abstract void HandleExecute();

        protected abstract void HandleTerminate();
    }
}