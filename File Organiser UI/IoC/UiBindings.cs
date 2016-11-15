namespace File.Organiser.UI.IoC
{
    using System;
    using System.Windows.Controls;
    using Controls;
    using EyssyApps.Configuration.Library;
    using EyssyApps.Organiser.Library.Managers;
    using SimpleInjector;

    public class UiBindings : CommonBindings
    {
        protected override void LoadBindings()
        {
            base.LoadBindings();

            this.BindWindows();
            this.BindViews();
            this.BindControls();
        }

        protected override void BindServices()
        {
            base.BindServices();

            this.Bind<IViewNavigator, ViewNavigator>();
        }

        protected virtual void BindWindows()
        {
            this.Bind<IMainWindow, MainWindow>();
        }

        protected virtual void BindViews()
        {
            // TODO: bind views
        }

        protected override void BindManagers()
        {
            base.BindManagers();

            this.Bind<ITaskManager, SimpleTaskManager>(lifestyle: Lifestyle.Singleton);
            this.Bind<IApplicationConfigurationManager>(container =>
            {
                return new ApplicationConfigurationManager(App.ConfigurationFilePath);
            }, lifestyle: Lifestyle.Singleton);
        }

        protected virtual void BindControls()
        {
            this.Bind<ISystemTrayControl>(container =>
            {
                ContextMenu menu = App.Current.TryFindResource(App.ControlTrayContextMenu) as ContextMenu;

                return new SystemTrayControl(
                    menu,
                    UiResources.App,
                    App.Name);
            }, Lifestyle.Singleton);
        }
    }
}