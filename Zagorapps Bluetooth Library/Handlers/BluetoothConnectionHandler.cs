namespace Zagorapps.Bluetooth.Library.Handlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using Core.Library.Extensions;
    using Core.Library.Timing;
    using Events;
    using Extensions;
    using Messaging;
    using Networking;
    using Providers;

    public class BluetoothConnectionHandler : BluetoothConnectionHandlerBase
    {
        protected const int HeaderSize = 4;

        protected readonly IMessageHandler<IMessage> MessageHandler;
        protected readonly ICommandOperationsProvider OperationProvider;
        protected readonly IMessageProvider MessageProvider;

        private bool continueReadingData;

        public BluetoothConnectionHandler(
            IBluetoothClient client,
            IStreamProvider streamProvider,
            IMessageHandler<IMessage> messageHandler,
            ICommandOperationsProvider operationProvider,
            IMessageProvider messageProvider,
            ITimer timer)
            : base(client, streamProvider, timer)
        {
            this.MessageHandler = messageHandler;
            this.OperationProvider = operationProvider;
            this.MessageProvider = messageProvider;

            this.continueReadingData = true;
        }

        protected override void HandleIncoming()
        {
            Task.Run(() =>
            {
                while (this.continueReadingData)
                {
                    try
                    {
                        byte[] header = this.Reader.ReadAndTrimBytes(amountToRead: BluetoothConnectionHandler.HeaderSize);

                        if (header.Any())
                        {
                            int amountToRead = this.DetermineDataSize(header);

                            byte[] data = this.Reader.ReadAndTrimBytes(amountToRead: amountToRead);

                            this.OnDataReceived(this, new BluetoothConnectionEventArgs(this.ConnectionClient.RemoteMachineName, data));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.OnFailureRaised(this, ex);
                    }
                }
            });
        }

        protected override void HandleOutgoing()
        {
        }

        protected override void CloseStreams()
        {
            this.continueReadingData = false;

            base.CloseStreams();
        }

        protected override void InformConnectedClientOfClosure()
        {
            IBasicStringMessage message = this.MessageProvider.CreateBasicStringMessage(ServerCommand.Close.ToString());

            this.HandleMessageSending(message);
        }

        protected override void HandleMessageSending(IMessage message)
        {
            IBasicCommandOperation operation = this.OperationProvider.CreateBasicOperation(this.Writer);

            this.MessageHandler.Send(message, operation);
        }

        protected int DetermineDataSize(byte[] header)
        {
            string asString = string.Empty;

            header
                .Select(b => Convert.ToChar(b))
                .ForEach(c => asString += c.ToString());

            return int.Parse(asString);
        }
    }
}