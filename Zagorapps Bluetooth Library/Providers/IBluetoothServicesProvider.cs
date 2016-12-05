namespace Zagorapps.Bluetooth.Library.Providers
{
    using System;
    using Client;
    using Client.Models;
    using Handlers;
    using InTheHand.Net.Sockets;
    using Messaging;
    using Networking;

    public interface IBluetoothServicesProvider
    {
        bool IsBluetoothAvailable { get; }

        IBluetoothClient CreateServerClient();

        IBluetoothClient CreateClient(BluetoothClient client);

        IBluetoothListener CreateListener(Guid serviceID);

        ISimpleBluetoothClientReceiver CreateReceiver(ConnectionSettings settings, IBluetoothServicesProvider provider);

        IBluetoothConnectionHandler CreateConnectionHandler(
            IBluetoothClient client,
            IStreamProvider streamProvider,
            IMessageHandler<IMessage> messageHandler,
            ICommandOperationsProvider operationProvider,
            IMessageProvider messageProvider);
    }
}