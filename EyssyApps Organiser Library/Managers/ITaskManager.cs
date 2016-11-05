namespace EyssyApps.Organiser.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using Core.Library.Execution;
    using Tasks;

    public interface ITaskManager : IExecute, ITerminate
    {
        bool Add(ITask task);

        bool Delete(ITask task);

        ITask FindById(Guid id);

        IEnumerable<ITask> GetAll();
    }
}