namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Core.Library.Events;
    using Core.Library.Execution;

    public interface ITask : IExecute, ITerminate, IRaiseFailures
    {
        event EventHandler<EventArgs<TaskState>> StateChanged;

        Guid Id { get; }

        string Description { get; }

        TaskState State { get; }

        TaskType TaskType { get; }
    }
}