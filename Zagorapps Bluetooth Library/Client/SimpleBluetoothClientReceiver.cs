namespace Zagorapps.Bluetooth.Library.Client
{
    using System.Threading.Tasks;
    using Core.Library.Events;
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
                        IBluetoothClient client = this.Provider.CreateClient(this.Listener.AcceptBluetoothClient());

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