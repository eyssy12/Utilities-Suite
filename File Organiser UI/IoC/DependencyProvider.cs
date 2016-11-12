namespace File.Organiser.UI.IoC
{
    using System;
    using File.Organiser.UI.Controls;
    using SimpleInjector;
    using Views;

    public static class DependencyProvider
    {
        private static readonly Container container;
        private static bool locked;

        static DependencyProvider()
        {
            DependencyProvider.container = new Container();

            UiBindings bindings = new UiBindings();
            bindings.RegisterBindingsToContainer(DependencyProvider.container);

            DependencyProvider.container.RegisterCollection<IViewControl>(new[] { typeof(Home), typeof(AddTask), typeof(IndividualTask) });
            DependencyProvider.container.Register<IViewNavigator, ViewNavigator>();

            DependencyProvider.locked = false;
        }

        public static TService Get<TService>()
            where TService : class
        {
            return DependencyProvider.container.GetInstance<TService>();
        }

        public static void Bind(Type service, Func<object> instanceCreator, Lifestyle lifestyle)
        {
            if (!DependencyProvider.IsLocked)
            {
                DependencyProvider.container.Register(service, instanceCreator, lifestyle);
            }
        }

        public static void LockContainer()
        {
            if (!DependencyProvider.IsLocked)
            {
                DependencyProvider.container.Verify();

                DependencyProvider.locked = true;
            }
        }

        public static bool IsLocked
        {
            get { return DependencyProvider.locked; }
        }
    }
}