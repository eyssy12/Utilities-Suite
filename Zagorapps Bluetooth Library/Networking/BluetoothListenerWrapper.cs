namespace Zagorapps.Bluetooth.Library.Networking
{
    using System;
    using InTheHand.Net.Sockets;

    public class BluetoothListenerWrapper : IBluetoothListener
    {
        protected readonly Guid ServiceID;

        private BluetoothListener listener;

        public BluetoothListenerWrapper(Guid serviceId)
        {
            if (serviceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(serviceId), "message");
            }

            this.ServiceID = serviceId;
        }

        public BluetoothClient AcceptBluetoothClient()
        {
            return this.listener.AcceptBluetoothClient();
        }

        public void Start()
        {
            try
            {
                this.listener = new BluetoothListener(this.ServiceID);

                this.listener.Start();
            }
            catch
            {
                throw;
            }
        }

        public void Stop()
        {
            this.listener.Stop();
        }
    }
}