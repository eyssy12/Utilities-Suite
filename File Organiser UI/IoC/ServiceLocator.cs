namespace File.Organiser.UI.IoC
{
    using System;
    using EyssyApps.Organiser.Library.Factories;
    using File.Organiser.UI.Controls;
    using SimpleInjector;
    using Views;

    public static class ServiceLocator
    {
        private static readonly Container container;
        private static bool locked;

        static ServiceLocator()
        {
            ServiceLocator.container = new Container();

            UiBindings bindings = new UiBindings();
            bindings.RegisterBindingsToContainer(ServiceLocator.container);

            // TODO: add to bindings
            ServiceLocator.container.RegisterCollection<IViewControl>(new[] { typeof(Home), typeof(AddTask), typeof(IndividualTask) });

            ServiceLocator.locked = false;
        }

        public static IOrganiserFactory GetFactory()
        {
            return ServiceLocator.Get<IOrganiserFactory>();
        }

        public static void Bind(Type service, Func<object> instanceCreator, Lifestyle lifestyle)
        {
            if (!ServiceLocator.IsLocked)
            {
                ServiceLocator.container.Register(service, instanceCreator, lifestyle);
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

        public static bool IsLocked
        {
            get { return ServiceLocator.locked; }
        }

        private static TService Get<TService>()
            where TService : class
        {
            if (!ServiceLocator.IsLocked)
            {
                ServiceLocator.locked = true;
            }

            return ServiceLocator.container.GetInstance<TService>();
        }
    }
}