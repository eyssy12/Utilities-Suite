namespace Zagorapps.Utilities.Suite.Library.Tasks
{
    using NodaTime;

    public interface IScheduledTask : ITask
    {
        Instant? NextScheduled{ get; }
    }
}