namespace Zagorapps.Core.Library.Communications
{
    using System;
    using Events;

    public interface INetworkConnection
    {
        event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        void Start();

        void Close();

        void Send(IDataMessage message);
    }
}