namespace Zagorapps.Bluetooth.Library.Networking
{
    public interface IListen<T>
    {
        void Listen();

        void Stop();
    }
}