namespace EyssyApps.Organiser.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Library.Extensions;
    using Factories;
    using Tasks;

    public class SimpleTaskManager : ITaskManager
    {
        // TODO: add event raiser 

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
            try
            {
                this.Tasks.ForEach(t => Task.Run(() => t.Execute()));
            }
            catch (Exception ex)
            {
                // TODO: raise exception
            }
        }

        public void Terminate()
        {
            
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

        public IEnumerable<ITask> GetAll()
        {
            return this.Tasks;
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