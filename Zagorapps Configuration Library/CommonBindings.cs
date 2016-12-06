namespace Zagorapps.Configuration.Library
{
    using Core.Library.Factories;
    using Core.Library.Managers;
    using Core.Library.Timing;
    using Core.Library.Windows;
    using Core.Library.Windows.Registry;
    using SimpleInjector;
    using Utilities.Library.Factories;

    public class CommonBindings : BindingsBase
    {
        protected override void RegisterBindings()
        {
            this.RegisterFactories();
            this.RegisterTime();
            this.RegisterTimers();
            this.RegisterManagers();
            this.RegisterServices();
        }

        protected virtual void RegisterServices()
        {
            this.Register<IIniFileManager, IniFileManager>();
            this.Register<IWinSystemService, WinSystemService>();
        }

        protected virtual void RegisterManagers()
        {
            this.Register<IFileManager, LocalFileManager>();
            this.Register<IDirectoryManager, LocalDirectoryManager>();
            this.Register<IApplicationRegistryManager>(container =>
            {
                return new ApplicationRegistryManager("File-Organiser");
            }, Lifestyle.Singleton);
        }

        protected virtual void RegisterFactories()
        {
            this.Register<IFactory>(container => container.GetInstance<IOrganiserFactory>(), Lifestyle.Singleton);
            this.BindFactory<IOrganiserFactory>();
        }

        protected virtual void RegisterTime()
        {
            //this.Bind<IClock>
        }

        protected virtual void RegisterTimers()
        {
            this.Register<ITimer, ThreadedTimer>();
        }
    }
}
