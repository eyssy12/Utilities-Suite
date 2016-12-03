namespace Zagorapps.Utilities.Library.Tasks
{
    using System;
    using Core.Library.Events;
    using Core.Library.Execution;

    public interface ITask : IExecute, ITerminate, IRaiseFailures
    {
        event EventHandler<EventArgs<TaskState>> StateChanged;

        Guid Identity { get; }

        string Name { get; }

        string Description { get; }

        TaskState State { get; }

        TaskType TaskType { get; }
    }
}