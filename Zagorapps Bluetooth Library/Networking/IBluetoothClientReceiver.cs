namespace Zagorapps.Bluetooth.Library.Networking
{
    using System;
    using Core.Library.Events;

    public interface IBluetoothClientReceiver<T> : IListen<T>
    {
        event EventHandler<EventArgs<IBluetoothClient>> ClientReceived;

        bool TryInitialise();
    }
}