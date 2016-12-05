namespace Zagorapps.Bluetooth.Library.Networking
{
    using System.Collections.Generic;
    using System.IO;
    using InTheHand.Net.Sockets;

    public class BluetoothClientWrapper : IBluetoothClient
    {
        protected readonly BluetoothClient Client;

        public BluetoothClientWrapper(BluetoothClient client)
        {
            this.Client = client;
        }

        public bool Connected
        {
            get { return this.Client.Connected; }
        }

        public string RemoteMachineName
        {
            get { return this.Client.RemoteMachineName; }
        }

        public void Close()
        {
            this.Client.Close();
        }

        public IEnumerable<BluetoothDeviceInfo> DiscoverDevices()
        {
            return this.Client.DiscoverDevices();
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }

        public Stream GetStream()
        {
            return this.Client.GetStream();
        }
    }
}