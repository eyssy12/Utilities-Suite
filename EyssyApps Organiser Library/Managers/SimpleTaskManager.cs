namespace EyssyApps.Organiser.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Exceptions;
    using Factories;
    using Providers;
    using Tasks;

    public class SimpleTaskManager : ITaskManager
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly ITaskHistoryProvider Provider;
        protected readonly IList<TaskMetadata> Tasks;

        public SimpleTaskManager(IOrganiserFactory factory, ITaskHistoryProvider provider)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory), "factory missing");
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "task logger missing");
            }

            this.Factory = factory;
            this.Provider = provider;

            this.Tasks = new List<TaskMetadata>();
        }

        public void Execute()
        {
            this.Tasks.ForEach(t => this.RunTask(t.Task));
        }

        public void Terminate()
        {
            // TODO: how to do this if possible...
        }

        public bool Add(ITask task)
        {
            if (this.DoesNotContain(task))
            {
                TaskMetadata metadata = new TaskMetadata(
                    task,
                    new EventHandler<EventArgs<TaskState>>((sender, e) => this.HandleStateChanged(sender, e, task)),
                    new EventHandler<EventArgs<Exception>>((sender, e) => this.HandleFailureRaised(sender, e, task)));

                this.Tasks.Add(metadata);

                this.Provider.Log(task, LogTaskType.Created, "Task created.");

                return true;
            }

            return false;
        }

        public bool Delete(ITask task)
        {
            TaskMetadata metadata = this.GetAssociatedMetadata(task);

            if (metadata == null)
            {
                return false;
            }

            this.Tasks.Remove(metadata);

            metadata.Dispose();

            this.Provider.Log(task, LogTaskType.Deleted, "Task was removed.");

            return true;
        }

        public ITask FindById(Guid identity)
        {
            return this.Tasks.FirstOrDefault(t => t.Task.Identity == identity).Task;
        }

        public IEnumerable<ITask> GetAll()
        {
            return this.Tasks.Select(t => t.Task).ToArray();
        }

        public bool DeleteById(Guid id)
        {
            return this.Delete(this.FindById(id));
        }

        public void RunTaskById(Guid id)
        {
            this.RunTask(this.FindById(id));
        }

        protected TaskMetadata GetAssociatedMetadata(ITask task)
        {
            return this.Tasks.FirstOrDefault(t => t.Task.Identity == task.Identity);
        }

        protected bool Contains(ITask task)
        {
            if (task == null)
            {
                return false;
            }

            return this.Tasks.Any(t => t.Task.Identity == task.Identity);
        }

        protected bool DoesNotContain(ITask task)
        {
            return !this.Contains(task);
        }

        protected void RunTask(ITask task)
        {
            if (task == null)
            {
                throw new UnknownTaskException("Provided task '" +nameof(task) + "' is missing.");
            }

            task.Execute();
        }

        private void HandleStateChanged(object sender, EventArgs<TaskState> e, ITask task)
        {
            this.Provider.Log(task, LogTaskType.StateChanged, string.Empty);
        }

        private void HandleFailureRaised(object sender, EventArgs<Exception> e, ITask task)
        {
            this.Provider.Log(task, LogTaskType.FailureRaised, e.First.Message);
        }

        protected class TaskMetadata : IDisposable
        {
            private readonly ITask task;
            private readonly EventHandler<EventArgs<TaskState>> StateHandler;
            private readonly EventHandler<EventArgs<Exception>> FailureHandler;

            public TaskMetadata(
                ITask task, 
                EventHandler<EventArgs<TaskState>> stateHandler,
                EventHandler<EventArgs<Exception>> failureHandler)
            {
                this.task = task;
                this.StateHandler = stateHandler;
                this.FailureHandler = failureHandler;

                this.task.StateChanged += stateHandler;
                this.task.FailureRaised += failureHandler;
            }

            public ITask Task
            {
                get { return this.task; }
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
                    this.task.StateChanged -= this.StateHandler;
                    this.task.FailureRaised -= this.FailureHandler;
                }
            }
        }
    }
}