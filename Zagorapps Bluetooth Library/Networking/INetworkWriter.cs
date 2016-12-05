namespace Zagorapps.Bluetooth.Library.Networking
{
    using System;

    public interface INetworkWriter : IDisposable
    {
        void Write(object contents);

        void Flush();

        void Close();
    }
}