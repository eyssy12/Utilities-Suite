namespace Zagorapps.Utilities.Suite.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using Tasks;

    public interface ITaskManager
    {
        bool Add(ITask task);

        bool Delete(ITask task);

        bool DeleteById(Guid id);

        ITask FindById(Guid id);

        IEnumerable<ITask> GetAll();

        void RunTaskById(Guid id);
    }
}