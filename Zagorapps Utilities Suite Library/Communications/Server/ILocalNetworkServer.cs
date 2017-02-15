namespace Zagorapps.Utilities.Suite.Library.Communications.Server
{
    using System;
    using Core.Library.Communications;
    using Core.Library.Events;

    public interface ILocalNetworkServer : IDisposable
    {
        event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        event EventHandler<EventArgs<IDataMessage>> MessageSent;

        event EventHandler<EventArgs<ConnectionType, string>> ClientConnected;

        event EventHandler<EventArgs<ConnectionType, string>> ClientDisconnected;

        bool Start();

        bool Stop();

        void Broadcast(IDataMessage message);

        bool Send(string to, IDataMessage message);

        bool DisconnectClient(string who);
    }
}