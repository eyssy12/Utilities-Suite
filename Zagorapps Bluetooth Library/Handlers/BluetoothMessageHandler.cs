namespace Zagorapps.Bluetooth.Library.Handlers
{
    using Data;
    using Messaging;

    public class BluetoothMessageHandler : MessageHandlerBase<IMessage>, IMessageHandler<IMessage>
    {
        public BluetoothMessageHandler()
        {
        }

        protected override void HandleSend(IMessage message, ICommandOperation<IMessage> operation)
        {
            if (message == null)
            {
                // TODO: should throw an appropriate exception
                return;
            }

            if (operation == null)
            {
                return;
            }

            operation.Invoke(message);
        }
    }
}