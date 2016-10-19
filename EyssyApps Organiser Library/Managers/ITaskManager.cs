namespace EyssyApps.Organiser.Library.Managers
{
    using System;
    using Tasks;

    public interface ITaskManager
    {
        void Add(ITask task);

        void Modify(ITask task);

        void Delete(ITask task);

        ITask FindById(Guid id);
    }
}