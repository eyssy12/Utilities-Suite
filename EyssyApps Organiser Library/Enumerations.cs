namespace EyssyApps.Organiser.Library
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

    public enum TaskState : byte
    {
        NotStarted = 0,
        Running,
        Finished,
        Cancelled,
        Failed
    }
}