namespace File.Organiser.UI.IoC
{
    using System.Configuration;
    using System.Windows.Controls;
    using Controls;
    using EyssyApps.Configuration.Library;
    using EyssyApps.Organiser.Library.Managers;
    using EyssyApps.Organiser.Library.Models.Organiser;
    using EyssyApps.Organiser.Library.Providers;
    using Newtonsoft.Json;
    using SimpleInjector;
    using FileIO = System.IO.File;
    using PathIO = System.IO.Path;

    public class UiBindings : CommonBindings
    {
        protected const string KeyApplicationPath = "Path_Application",
            KeyFileExtensionJsonFile = "Name_FileExtensionJson",
            KeyConfigurationFileName = "Name_ConfigurationFile";

        protected override void LoadBindings()
        {
            base.LoadBindings();

            this.BindWindows();
            this.BindViews();
            this.BindControls();
            this.BindProviders();
        }

        private void BindProviders()
        {
            this.Bind<IFileExtensionProvider>((container) =>
            {
                string fileExtensionJson = ConfigurationManager.AppSettings[UiBindings.KeyFileExtensionJsonFile];
                string filePath = PathIO.Combine(this.GetApplicationPath(), fileExtensionJson);

                string data = FileIO.ReadAllText(filePath);

                FileExtensionDatabaseModel result = JsonConvert.DeserializeObject<FileExtensionDatabaseModel>(data);

                return new FileExtensionProvider(result);
            }, Lifestyle.Singleton);
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
                string configName = ConfigurationManager.AppSettings[UiBindings.KeyConfigurationFileName];

                return new ApplicationConfigurationManager(PathIO.Combine(this.GetApplicationPath(), configName));
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

        private string GetApplicationPath()
        {
            return PathIO.GetFullPath(ConfigurationManager.AppSettings[UiBindings.KeyApplicationPath]);
        }
    }
}