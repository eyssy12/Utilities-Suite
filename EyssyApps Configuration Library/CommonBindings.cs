namespace EyssyApps.Configuration.Library
{
    using System;
    using Core.Library.Factories;
    using Core.Library.Managers;
    using Core.Library.Timing;
    using Ninject;
    using Ninject.Extensions.Factory;
    using Ninject.Modules;
    using Organiser.Library.Factories;
    using Organiser.Library.Providers;

    public class CommonBindings : NinjectModule
    {
        public override void Load()
        {
            this.BindFactories();
            this.BindTimers();
            this.BindProviders();
            this.BindManagers();
            this.BindTasks();
        }

        protected virtual void BindTasks()
        {
            throw new NotImplementedException();
        }

        protected virtual void BindManagers()
        {
            this.Bind<IFileManager>().To<LocalFileManager>();
            this.Bind<IDirectoryManager>().To<LocalDirectoryManager>();
        }

        protected virtual void BindProviders()
        {
            this.Bind<IFileExtensionProvider>().To<FileExtensionProvider>();
        }

        protected virtual void BindFactories()
        {
            this.Bind<IFactory>().ToMethod(c => c.Kernel.Get<IOrganiserFactory>());

            this.Bind<IOrganiserFactory>().ToFactory().InSingletonScope();
        }

        protected virtual void BindTimers()
        {
            this.Bind<ITimer>().To<ThreadedTimer>();
        }
    }
}