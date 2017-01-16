namespace Zagorapps.Bluetooth.Configuration.Library
{
    using Bluetooth.Library.Handlers;
    using Bluetooth.Library.Messaging;
    using Bluetooth.Library.Providers;
    using SimpleInjector;
    using Zagorapps.Configuration.Library;

    public class BluetoothBindings : BindingsBase
    {
        protected override void RegisterBindings()
        {
            this.RegisterProviders();
            this.RegisterHandlers();
        }

        protected virtual void RegisterProviders()
        {
            this.Register<IBluetoothServicesProvider, BluetoothServicesProvider>();
            this.Register<IMessageProvider, MessageProvider>();
            this.Register<IStreamProvider, StreamProvider>();
            this.Register<ICommandOperationsProvider, CommandOperationsProvider>();
        }

        protected virtual void RegisterHandlers()
        {
            this.Register<IMessageHandler<IMessage>, BluetoothMessageHandler>();
        }
    }
}