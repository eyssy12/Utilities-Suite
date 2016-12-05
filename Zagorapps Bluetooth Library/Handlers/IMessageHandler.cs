namespace Zagorapps.Bluetooth.Library.Handlers
{
    using Data;

    public interface IMessageHandler<TMessage>
    {
        void Send(TMessage message, ICommandOperation<TMessage> operation);
    }
}