namespace Zagorapps.Bluetooth.Library.Messaging
{
    using Data;
    using Handlers;

    public abstract class MessageHandlerBase<TMessage> : IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        protected MessageHandlerBase()
        {
        }

        public void Send(TMessage message, ICommandOperation<TMessage> operation)
        {
            this.HandleSend(message, operation);
        }

        protected abstract void HandleSend(TMessage message, ICommandOperation<TMessage> operation);
    }
}