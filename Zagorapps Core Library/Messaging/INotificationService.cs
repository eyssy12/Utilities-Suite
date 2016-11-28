namespace Zagorapps.Core.Library.Messaging
{
    public interface INotificationService<TContent>
    {
        void Notify(TContent contents);
    }
}