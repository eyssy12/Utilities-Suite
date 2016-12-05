namespace Zagorapps.Bluetooth.Library.Handlers
{
    using System;
    using Core.Library.Events;
    using Events;
    using Messaging;
    using Networking;
    using Providers;

    public abstract class BluetoothConnectionHandlerBase : IBluetoothConnectionHandler
    {
        protected readonly IBluetoothClient ConnectionClient;
        protected readonly IStreamProvider StreamProvider;

        protected readonly IBinaryWriter Writer;
        protected readonly IBinaryReader Reader;

        public event EventHandler<EventArgs<BluetoothConnectionEventArgs>> DataReceived;

        public event EventHandler<EventArgs<Exception>> FailureRaised;

        protected BluetoothConnectionHandlerBase(IBluetoothClient connectionClient, IStreamProvider streamProvider)
        {
            this.ConnectionClient = connectionClient;
            this.StreamProvider = streamProvider;

            this.Writer = this.StreamProvider.CreateBinaryWriter(connectionClient.GetStream());
            this.Reader = this.StreamProvider.CreateBinaryReader(connectionClient.GetStream());
        }

        public string ClientName
        {
            get { return this.ConnectionClient.RemoteMachineName; }
        }

        public void Begin()
        {
            this.HandleIncoming();
        }

        public void Finish()
        {
            this.InformConnectedClientOfClosure();
            this.CloseStreams();
            this.CloseConnectionClient();
        }

        public bool SendMessage(IMessage message)
        {
            if (message == null)
            {
                return false;
            }

            this.HandleMessageSending(message);

            return true;
        }

        protected virtual void CloseStreams()
        {
            this.Reader.Close();
            this.Writer.Close();
        }

        protected virtual void CloseConnectionClient()
        {
            this.ConnectionClient.Close();
            this.ConnectionClient.Dispose();
        }

        protected virtual void OnDataReceived(object sender, BluetoothConnectionEventArgs args)
        {
            Invoker.Raise(ref this.DataReceived, sender, args);
        }

        protected virtual void OnFailureRaised(object sender, Exception exception)
        {
            Invoker.Raise(ref this.FailureRaised, sender, exception);
        }

        protected abstract void InformConnectedClientOfClosure();

        protected abstract void HandleIncoming();

        protected abstract void HandleOutgoing();

        protected abstract void HandleMessageSending(IMessage message);
    }
}