namespace Zagorapps.Utilities.Library.Communications.Server
{
    using System;
    using Core.Library.Communications;
    using Core.Library.Events;

    public interface ILocalNetworkServer : IDisposable
    {
        event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        event EventHandler<EventArgs<ConnectionType, string>> ClientConnected;

        event EventHandler<EventArgs<ConnectionType, string>> ClientDisconnected;

        bool Start();

        bool Stop();

        void Broadcast(IDataMessage message);

        bool Send(IDataMessage message);
    }
}