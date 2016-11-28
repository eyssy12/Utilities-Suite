namespace Zagorapps.Organiser.Library.Providers
{
    using Tasks;

    public interface ITaskHistoryProvider
    {
        void Log(ITask task, LogTaskType logType, string message);

        string GetHistory(ITask task);

        string GetStorePath(ITask task);
    }
}