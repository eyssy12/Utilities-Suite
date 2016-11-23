namespace EyssyApps.Organiser.Library.Providers
{
    using Tasks;

    public interface ITaskHistoryProvider
    {
        void Failure(ITask task, string message);

        void StateChanged(ITask task);

        void TaskDeleted(ITask task, string message);

        void TaskCreated(ITask task, string message);

        string GetHistory(ITask task);

        string GetStorePath(ITask task);
    }
}