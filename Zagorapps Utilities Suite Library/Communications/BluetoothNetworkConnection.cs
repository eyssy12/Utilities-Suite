namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using System;
    using System.Text;
    using Bluetooth.Library.Events;
    using Bluetooth.Library.Handlers;
    using Bluetooth.Library.Messaging;
    using Core.Library.Communications;
    using Core.Library.Events;

    public class BluetoothNetworkConnection : INetworkConnection
    {
        protected readonly IBluetoothConnectionHandler Handler;

        public BluetoothNetworkConnection(IBluetoothConnectionHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Bluetooth connection handler not provided");
            }

            this.Handler = handler;
        }

        public event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        public void Close()
        {
            this.Handler.Finish();
            this.Handler.DataReceived -= this.Handler_DataReceived;
            this.Handler.HeartbeatInitiated -= this.Handler_HeartbeatInitiated;
        }

        public void Start()
        {
            this.Handler.DataReceived += this.Handler_DataReceived;
            this.Handler.HeartbeatInitiated += this.Handler_HeartbeatInitiated;
            this.Handler.Begin();
        }

        public void Send(IDataMessage message)
        {
            this.Handler.SendMessage(new BasicStringMessage(message.Data.ToString()));
        }

        // TODO: this should be paseed in via the constructor as an IOperation or similar, we may want customized heatbeat handling for certain bluetooth connections
        private void Handler_HeartbeatInitiated(object sender, EventArgs<string, DateTime> e)
        {
            this.Handler.SendMessage(new BasicStringMessage("HBT_CHECK"));
        }

        private void Handler_DataReceived(object sender, BluetoothConnectionEventArgs e)
        {
            Invoker.Raise(ref this.MessageReceived, this, new BasicDataMessage(e.Raiser, Encoding.UTF8.GetString(e.Arg)));
        }
    }
}