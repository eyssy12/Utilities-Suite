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
            this.Register<IBluetoothServicesProvider, BluetoothServicesProvider>(lifestyle: Lifestyle.Transient);
            this.Register<IMessageProvider, MessageProvider>(lifestyle: Lifestyle.Transient);
            this.Register<IStreamProvider, StreamProvider>(lifestyle: Lifestyle.Transient);
            this.Register<ICommandOperationsProvider, CommandOperationsProvider>(lifestyle: Lifestyle.Transient);
        }

        protected virtual void RegisterHandlers()
        {
            this.Register<IMessageHandler<IMessage>, BluetoothMessageHandler>(lifestyle: Lifestyle.Transient);
        }
    }
}