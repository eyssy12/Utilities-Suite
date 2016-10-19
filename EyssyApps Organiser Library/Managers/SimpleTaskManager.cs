namespace EyssyApps.Organiser.Library.Managers
{
    using System;
    using Factories;
    using Tasks;

    public class SimpleTaskManager : ITaskManager
    {
        protected readonly IOrganiserFactory Factory;

        public SimpleTaskManager(IOrganiserFactory factory)
        {
            this.Factory = factory;
        }
        
        public void Add(ITask task)
        {
            throw new NotImplementedException();
        }

        public void Delete(ITask task)
        {
            throw new NotImplementedException();
        }

        public ITask FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Modify(ITask task)
        {
            throw new NotImplementedException();
        }
    }
}