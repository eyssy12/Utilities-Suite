namespace EyssyApps.Core.Library.Messaging
{
    public interface INotificationService<TContent>
    {
        void Notify(TContent contents);
    }
}