namespace Zagorapps.Bluetooth.Library.Handlers
{
    using System;
    using Core.Library.Events;
    using Core.Library.Timing;
    using Events;
    using Messaging;
    using Networking;
    using Providers;

    public abstract class BluetoothConnectionHandlerBase : IBluetoothConnectionHandler
    {
        protected const int DefaultHeartbeatInterval = 30, // 30 seconds
            OneSecond = 1000; // 1 second

        protected readonly IBluetoothClient ConnectionClient;
        protected readonly IStreamProvider StreamProvider;
        protected readonly ITimer Timer;

        protected readonly IBinaryWriter Writer;
        protected readonly IBinaryReader Reader;

        private int currentTime = 30;

        protected BluetoothConnectionHandlerBase(IBluetoothClient connectionClient, IStreamProvider streamProvider, ITimer timer)
        {
            this.ConnectionClient = connectionClient;
            this.StreamProvider = streamProvider;
            this.Timer = timer;

            this.Writer = this.StreamProvider.CreateBinaryWriter(connectionClient.GetStream());
            this.Reader = this.StreamProvider.CreateBinaryReader(connectionClient.GetStream());
        }

        public event EventHandler<BluetoothConnectionEventArgs> DataReceived;

        public event EventHandler<EventArgs<Exception>> FailureRaised;

        public event EventHandler<EventArgs<string, int>> TimerTickSecond;

        public string ClientName
        {
            get { return this.ConnectionClient.RemoteMachineName; }
        }

        public int HeartbeatInterval
        {
            get { return BluetoothConnectionHandlerBase.DefaultHeartbeatInterval; }
        }

        public void Begin()
        {
            this.Timer.TimeElapsed += Timer_TimeElapsed;
            this.Timer.Start(BluetoothConnectionHandlerBase.OneSecond, BluetoothConnectionHandlerBase.OneSecond);

            this.HandleIncoming();
        }

        private void Timer_TimeElapsed(object sender, EventArgs<int> e)
        {
            // TODO: doesn't feel like it should be the responsibility of this class here to send "second" updates to whoever subscribes to it
            // just a whole heartbeat interval event instead seems more appropriate
            this.currentTime--;

            if (this.currentTime == 0)
            {
                this.currentTime = BluetoothConnectionHandlerBase.DefaultHeartbeatInterval;
            }

            Invoker.Raise(ref this.TimerTickSecond, this, this.ClientName, this.currentTime);
        }

        public void Finish()
        {
            this.Timer.TimeElapsed -= this.Timer_TimeElapsed;
            this.Timer.Stop();

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