namespace Zagorapps.Utilities.Suite.Library.Providers
{
    using System;
    using Bluetooth.Library.Handlers;
    using Bluetooth.Library.Messaging;
    using Bluetooth.Library.Networking;
    using Bluetooth.Library.Providers;
    using Communications;
    using Core.Library.Communications;
    using Core.Library.Construction;
    using Core.Library.Timing;
    using Factories;

    public class NetworkConnectionProvider : INetworkConnectionProvider
    {
        protected readonly IOrganiserFactory Factory;

        public NetworkConnectionProvider(IOrganiserFactory factory)
        {
            this.Factory = factory;
        }

        public INetworkConnection CreateNetworkConnection(ConnectionType type, IContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Construction context not provided");
            }

            if (type == ConnectionType.Bluetooth)
            {
                IMessageHandler<IMessage> messageHandler = this.Factory.Create<IMessageHandler<IMessage>>();
                IMessageProvider messageProvider = this.Factory.Create<IMessageProvider>();
                IStreamProvider streamProvider = this.Factory.Create<IStreamProvider>();
                ICommandOperationsProvider operationProvider = this.Factory.Create<ICommandOperationsProvider>();
                ITimer timer = this.Factory.Create<ITimer>();

                // IContext to get what i need

                IBluetoothClient client = context.GetValue<IBluetoothClient>("connectionClient");

                IBluetoothConnectionHandler handler = this.Factory
                    .Create<IBluetoothServicesProvider>()
                    .CreateConnectionHandler(client, streamProvider, messageHandler, operationProvider, messageProvider, timer);

                return new BluetoothNetworkConnection(handler);
            }

            int port = context.GetValue<int>("endpointPort");

            return new UdpNetworkConnection(port);
        }
    }
}