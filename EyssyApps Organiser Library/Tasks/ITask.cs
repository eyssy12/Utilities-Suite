namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Runtime.Serialization;
    using Core.Library.Execution;

    public interface ITask : IExecute, ISerializable
    {
        Guid Id { get; }

        string Description { get; }

        void Terminate();
    }
}