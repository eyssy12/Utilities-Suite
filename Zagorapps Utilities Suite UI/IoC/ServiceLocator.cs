namespace Zagorapps.Utilities.Suite.UI.IoC
{
    using System;
    using Audio.Configuration.Library;
    using Bluetooth.Configuration.Library;
    using Configuration.Library;
    using Configuration.Library.Extensions;
    using SimpleInjector;
    using Zagorapps.Utilities.Library.Factories;

    public static class ServiceLocator
    {
        private static readonly Container Container;
        private static bool locked;

        static ServiceLocator()
        {
            ServiceLocator.Container = new Container();

            BindingsBase[] bindings = new BindingsBase[]
            {
                new CommonBindings(),
                new UiBindings(),
                new BluetoothBindings(),
                new AudioBindings()
            };

            ServiceLocator.Container.RegisterBindings(bindings);

            ServiceLocator.locked = false;
        }

        public static bool IsLocked
        {
            get { return ServiceLocator.locked; }
        }

        public static IOrganiserFactory GetFactory()
        {
            return ServiceLocator.Get<IOrganiserFactory>();
        }

        public static void Bind(Type service, Func<object> instanceCreator, Lifestyle lifestyle)
        {
            if (!ServiceLocator.IsLocked)
            {
                ServiceLocator.Container.Register(service, instanceCreator, lifestyle);
            }
            else
            {
                throw new InvalidCastException("The container is locked - the binding cannot be applied during runtime.");
            }
        }

        public static void Lock()
        {
            if (!ServiceLocator.IsLocked)
            {
                ServiceLocator.Container.Verify();
                ServiceLocator.locked = true;
            }
        }

        private static TService Get<TService>()
            where TService : class
        {
            if (!ServiceLocator.IsLocked)
            {
                ServiceLocator.locked = true;
            }

            return ServiceLocator.Container.GetInstance<TService>();
        }
    }
}