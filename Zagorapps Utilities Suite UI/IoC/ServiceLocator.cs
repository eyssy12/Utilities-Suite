namespace Zagorapps.Utilities.Suite.UI.IoC
{
    using System;
    using SimpleInjector;
    using Views;
    using Zagorapps.Organiser.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Navigation;

    public static class ServiceLocator
    {
        private static readonly Container Container;
        private static bool locked;

        static ServiceLocator()
        {
            ServiceLocator.Container = new Container();

            UiBindings bindings = new UiBindings();
            bindings.RegisterBindingsToContainer(ServiceLocator.Container);

            // TODO: add to bindings
            ServiceLocator.Container.RegisterCollection<IViewControl>(new[] { typeof(Home), typeof(AddTask), typeof(IndividualTask) });

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