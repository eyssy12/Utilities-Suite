namespace Zagorapps.Bluetooth.Library.Handlers
{
    using System;
    using Core.Library.Events;
    using Core.Library.Execution;
    using Events;
    using Messaging;

    public interface IBluetoothConnectionHandler : ISimpleHandler, IRaiseFailures
    {
        event EventHandler<BluetoothConnectionEventArgs> DataReceived;

        event EventHandler<EventArgs<string, int>> TimerTickSecond;

        string ClientName { get; }

        int HeartbeatInterval { get; }

        bool SendMessage(IMessage message);
    }
}