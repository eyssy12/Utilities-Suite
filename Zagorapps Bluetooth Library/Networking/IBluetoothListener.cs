namespace Zagorapps.Bluetooth.Library.Networking
{
    using InTheHand.Net.Sockets;

    public interface IBluetoothListener
    {
        void Start();

        void Stop();

        BluetoothClient AcceptBluetoothClient();
    }
}