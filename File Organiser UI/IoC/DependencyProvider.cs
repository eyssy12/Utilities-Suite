namespace File.Organiser.UI.IoC
{
    using System;
    using System.Collections.Generic;
    using EyssyApps.Configuration.Library;
    using EyssyApps.UI.Library.Controls;
    using SimpleInjector;
    using Views;

    public static class DependencyProvider
    {
        private static readonly Container container;
        private static bool locked;

        static DependencyProvider()
        {
            DependencyProvider.container = new Container();

            SimpleInjectorBindings bindings = new SimpleInjectorBindings();
            bindings.RegisterBindingsToContainer(DependencyProvider.container);

            //IEnumerable<IViewControl> views = new IViewControl[]
            //{
            //    new Home(),
            //    new AddTask(),
            //    new IndividualTask()
            //};

            //DependencyProvider.container.RegisterCollection<IViewControl>(views);
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