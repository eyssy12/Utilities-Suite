namespace Zagorapps.Bluetooth.Library.Client
{
    using System;
    using Core.Library.Events;
    using InTheHand.Net.Sockets;
    using Models;
    using Networking;
    using Providers;

    public abstract class BluetoothClientReceiverBase : IBluetoothClientReceiver<BluetoothDeviceInfo>
    {
        protected readonly ConnectionSettings Settings;
        protected readonly IBluetoothServicesProvider Provider;

        private IBluetoothListener listener;

        protected BluetoothClientReceiverBase(ConnectionSettings settings, IBluetoothServicesProvider provider)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "message");
            }

            if (settings.ServiceID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(settings.ServiceID), "message");
            }

            if (string.IsNullOrWhiteSpace(settings.Pin))
            {
                throw new ArgumentNullException(nameof(settings.Pin), "message");
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "message");
            }

            this.Provider = provider;
            this.Settings = settings;
        }

        public event EventHandler<EventArgs<IBluetoothClient>> ClientReceived;

        protected IBluetoothListener Listener
        {
            get { return this.listener; }
        }

        public bool TryInitialise()
        {
            try
            {
                this.listener = this.Provider.CreateListener(this.Settings.ServiceID);

                if (this.listener != null)
                {
                    this.listener.Start();
                }
            }
            catch
            {
                // bluetooth not available or something else is wrong 
                return false;
            }

            return true;
        }

        public void Listen()
        {
            this.ListenForClients();
        }

        public void Stop()
        {
            this.InitiateStop();
        }

        protected virtual void InitiateStop()
        {
            if (!this.Provider.IsBluetoothAvailable || this.listener != null)
            {
                this.listener.Stop();

                this.listener = null;
            }
        }

        protected void OnClientReceived(object raiser, EventArgs<IBluetoothClient> args)
        {
            if (args != null)
            {
                Invoker.Raise(ref this.ClientReceived, raiser, args);
            }
        }

        protected abstract void ListenForClients();
    }
}