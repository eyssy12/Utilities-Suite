namespace Zagorapps.Bluetooth.Library.Client
{
    using System.Threading.Tasks;
    using Core.Library.Events;
    using InTheHand.Net;
    using InTheHand.Net.Bluetooth;
    using InTheHand.Net.Sockets;
    using Models;
    using Networking;
    using Providers;

    public class SimpleBluetoothClientReceiver : BluetoothClientReceiverBase, ISimpleBluetoothClientReceiver
    {
        private bool doListen;

        public SimpleBluetoothClientReceiver(ConnectionSettings settings, IBluetoothServicesProvider provider)
            : base(settings, provider)
        {
            this.doListen = true;
        }

        protected override void ListenForClients()
        {
            Task.Run(() =>
            {
                while (this.doListen)
                {
                    try
                    {
                        IBluetoothClient client = this.Provider.CreateClient(this.listener.AcceptBluetoothClient());

                        this.OnClientReceived(this, new EventArgs<IBluetoothClient>(client));
                    }
                    catch
                    {
                        break;
                    }
                }
            });
        }

        protected override void InitiateStop()
        {
            this.doListen = false;

            base.InitiateStop();
        }
    }
}