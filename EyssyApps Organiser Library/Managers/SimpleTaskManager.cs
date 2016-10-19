namespace EyssyApps.Organiser.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Extensions;
    using Factories;
    using Tasks;

    public class SimpleTaskManager : ITaskManager
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly IList<ITask> Tasks;

        public SimpleTaskManager(IOrganiserFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory), "factory missing");
            }

            this.Factory = factory;
            this.Tasks = new List<ITask>();
        }

        public void Execute()
        {
            this.Tasks.ForEach(t => t.Execute());
        }

        public bool Add(ITask task)
        {
            if (this.DoesNotContain(task))
            {
                this.Tasks.Add(task);

                return true;
            }

            return false;
        }

        public bool Delete(ITask task)
        {
            if (this.Contains(task))
            {
                this.Tasks.Remove(task);

                return true;
            }

            return false;
        }

        public ITask FindById(Guid id)
        {
            return this.Tasks.FirstOrDefault(t => t.Id == id);
        }

        protected bool Contains(ITask task)
        {
            return this.Tasks.Any(t => t.Id == task.Id);
        }

        protected bool DoesNotContain(ITask task)
        {
            return !this.Contains(task);
        }
    }
}