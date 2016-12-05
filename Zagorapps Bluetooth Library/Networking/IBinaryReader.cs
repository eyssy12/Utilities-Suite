namespace Zagorapps.Bluetooth.Library.Networking
{
    using System;

    public interface IBinaryReader : IDisposable
    {
        byte[] ReadBytes(int count);

        void Close();
    }
}