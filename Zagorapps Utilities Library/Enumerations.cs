namespace Zagorapps.Utilities.Suite.Library
{
    using System;

    [Flags]
    public enum OrganiseType : byte
    {
        None = 0,
        File = 1,
        Directory = 2,
        All = 4
    }

    public enum TaskType : byte
    {
        Scheduled = 0,
        Organiser
    }

    [Flags]
    public enum TaskState : byte
    {
        NotStarted = 0,
        Pending = 1,
        Started = 2,
        Running = 4,
        Finished = 8,
        Cancelled = 16,
        Failed = 32
    }

    public enum LogTaskType
    {
        FailureRaised,
        StateChanged,
        Created,
        Deleted
    }

    public enum ConnectionType
    {
        Bluetooth,
        Udp,
        Tcp
    }

    public enum SuiteRoute : int
    {
        Organiser = 1,
        Dashboard,
        Connectivity,
        SystemControl
    }
}