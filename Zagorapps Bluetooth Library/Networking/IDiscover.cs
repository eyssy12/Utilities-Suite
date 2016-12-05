namespace Zagorapps.Bluetooth.Library.Networking
{
    using System.Collections.Generic;

    public interface IDiscover<T>
    {
        IEnumerable<T> Discover();

        void CeiseDiscovery();
    }
}