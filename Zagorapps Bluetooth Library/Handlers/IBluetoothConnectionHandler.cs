namespace Zagorapps.Bluetooth.Library.Handlers
{
    using System;
    using Core.Library.Events;
    using Core.Library.Execution;
    using Events;
    using Messaging;

    public interface IBluetoothConnectionHandler : ISimpleHandler, IRaiseFailures
    {
        event EventHandler<EventArgs<BluetoothConnectionEventArgs>> DataReceived;

        string ClientName { get; }

        bool SendMessage(IMessage message);
    }
}