namespace EyssyApps.Organiser.Library.Managers
{
    using Tasks;

    public interface ITaskLogger
    {
        void Failure(ITask task, string message);

        void StateChanged(ITask task);

        void TaskDeleted(ITask task, string message);

        void TaskCreated(ITask task, string message);

        string GetHistory(ITask task);
    }
}