namespace Zagorapps.Bluetooth.Library.Networking
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using InTheHand.Net.Sockets;

    public interface IBluetoothClient : IDisposable
    {
        string RemoteMachineName { get; }

        bool Connected { get; }

        Stream GetStream();

        void Close();

        IEnumerable<BluetoothDeviceInfo> DiscoverDevices();
    }
}