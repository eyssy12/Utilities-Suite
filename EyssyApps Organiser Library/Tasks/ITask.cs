namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Core.Library.Execution;

    public interface ITask : IExecute, ITerminate
    {
        Guid Id { get; }

        string Description { get; }

        TaskState State { get; }

        TaskType TaskType { get; }
    }
}