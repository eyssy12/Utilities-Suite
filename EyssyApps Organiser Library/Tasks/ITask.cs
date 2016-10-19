namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Runtime.Serialization;

    public interface ITask : ISerializable
    {
        Guid Id { get; }

        void Execute();

        void Terminate();
    }
}