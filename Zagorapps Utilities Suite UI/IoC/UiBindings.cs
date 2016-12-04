namespace Zagorapps.Utilities.Suite.UI.IoC
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Windows.Controls;
    using Commands;
    using Controls;
    using Library;
    using Library.Communications;
    using Managers;
    using Navigation;
    using Newtonsoft.Json;
    using Services;
    using SimpleInjector;
    using Suites;
    using Views.Connectivity;
    using Views.Dashboard;
    using Views.Organiser;
    using Views.SystemControl;
    using WCF.Library.Providers;
    using WCF.Library.Receivers;
    using WCF.Library.Senders;
    using WCF.Library.Services;
    using Zagorapps.Configuration.Library;
    using Zagorapps.Core.Library.Managers;
    using Zagorapps.Utilities.Library.Factories;
    using Zagorapps.Utilities.Library.Managers;
    using Zagorapps.Utilities.Library.Models.Organiser;
    using Zagorapps.Utilities.Library.Providers;
    using FileIO = System.IO.File;
    using PathIO = System.IO.Path;

    public class UiBindings : CommonBindings
    {
        protected const string KeyApplicationPath = "Path_Application",
            KeyFileExtensionJsonFile = "Name_FileExtensionJson",
            KeyConfigurationFileName = "Name_ConfigurationFile",
            KeyHistoryStore = "Store_History",
            KeySettingsStore = "Store_Settings",
            KeyTasksStore = "Store_Tasks",
            KeyOrganiserPipe = "Pipe_Organiser",
            KeyDashboardPipe = "Pipe_Dashboard",
            KeyConnectivityPipe = "Pipe_Connectivity",
            KeySystemControlPipe = "Pipe_SystemControl",
            KeyUtilitiesEndpoint = "Endpoint_Utilities";

        protected override void LoadBindings()
        {
            base.LoadBindings();

            this.BindWindows();
            this.BindViews();
            this.BindCommunications();
            this.BindControls();
            this.BindProviders();
            this.BindLoggers();
            this.BindDataFacilitatorManager();
        }

        private void BindDataFacilitatorManager()
        {
            this.Bind<IDataFacilitatorSuiteManager>(container =>
            {
                IOrganiserFactory factory = container.GetInstance<IOrganiserFactory>();
                ICommunicationsProvider commsProvider = container.GetInstance<ICommunicationsProvider>();
                ICommandProvider commandProvider = container.GetInstance<ICommandProvider>();

                IEnumerable<IViewControl> organiserControls = new List<IViewControl>
                {
                    new Home(factory, commandProvider),
                    new AddTask(factory, commandProvider),
                    new IndividualTask(factory, commandProvider)
                };

                IEnumerable<IViewControl> connectivityControls = new List<IViewControl>
                {
                    new NoBluetoothAvailable(factory, commandProvider)
                };

                IEnumerable<IViewControl> dashboardControls = new List<IViewControl>
                {
                    new Dashboard(factory, commandProvider)
                };

                IEnumerable<IViewControl> systemControls = new List<IViewControl>
                {
                    new First(factory, commandProvider),
                    new Second(factory, commandProvider)
                };

                IEnumerable<IReceiveSuiteData> systemReceivers = new List<IReceiveSuiteData>
                {
                    new WcfReceiveSuiteData(factory, commsProvider, this.GetValue(UiBindings.KeySystemControlPipe))
                };

                IEnumerable<ISendSuiteData> systemSenders = new List<ISendSuiteData>
                {
                    new WcfSendSuiteData(commsProvider, SuiteRoute.Dashboard, this.GetValue(UiBindings.KeyUtilitiesEndpoint), this.GetValue(UiBindings.KeyDashboardPipe))
                };

                IEnumerable<IReceiveSuiteData> dashboardReceivers = new List<IReceiveSuiteData>
                {
                    new WcfReceiveSuiteData(factory, commsProvider, this.GetValue(UiBindings.KeyDashboardPipe))
                };

                IEnumerable<ISendSuiteData> dashboardSenders = new List<ISendSuiteData>
                {
                    new WcfSendSuiteData(commsProvider, SuiteRoute.SystemControl, this.GetValue(UiBindings.KeyUtilitiesEndpoint), this.GetValue(UiBindings.KeySystemControlPipe))
                };

                IEnumerable<ISuite> suites = new List<ISuite>
                {
                    new DashboardSuite(dashboardControls, dashboardReceivers, dashboardSenders),
                    new FileOrganiserSuite(organiserControls),
                    new ConnectivitySuite(connectivityControls),
                    new SystemSuite(systemControls, systemReceivers, systemSenders)
                };

                return new DataFacilitatorSuiteManager(suites);
            }, lifestyle: Lifestyle.Transient);
        }

        protected virtual void BindCommunications()
        {
        }

        protected virtual void BindLoggers()
        {
        }

        protected virtual void BindProviders()
        {
            this.Bind<ICommandProvider, CommandProvider>();

            this.Bind<IFileExtensionProvider>((container) =>
            {
                string fileExtensionJson = ConfigurationManager.AppSettings[UiBindings.KeyFileExtensionJsonFile];
                string filePath = PathIO.Combine(this.GetApplicationPath(), fileExtensionJson);

                string data = FileIO.ReadAllText(filePath);

                FileExtensionDatabaseModel result = JsonConvert.DeserializeObject<FileExtensionDatabaseModel>(data);

                return new FileExtensionProvider(result);
            }, lifestyle: Lifestyle.Singleton);

            this.Bind<ITaskHistoryProvider>(container =>
            {
                return new TaskHistoryProvider(
                    ConfigurationManager.AppSettings[UiBindings.KeyHistoryStore],
                    container.GetInstance<IFileManager>(),
                    container.GetInstance<IDirectoryManager>());
            }, lifestyle: Lifestyle.Transient);

            this.Bind<IOrganiserSettingsProvider>(container =>
            {
                return new OrganiserSettingsProvider(
                    ConfigurationManager.AppSettings[UiBindings.KeySettingsStore],
                    container.GetInstance<IFileManager>(),
                    container.GetInstance<IDirectoryManager>());
            }, lifestyle: Lifestyle.Transient);

            this.Bind<ITaskProvider>(container =>
            {
                return new TaskProvider(
                    ConfigurationManager.AppSettings[UiBindings.KeyTasksStore],
                    container.GetInstance<IOrganiserFactory>(),
                    container.GetInstance<IFileManager>(),
                    container.GetInstance<IDirectoryManager>());
            }, lifestyle: Lifestyle.Transient);

            this.Bind<ICommunicationsProvider, CommunicationsProvider>(lifestyle: Lifestyle.Transient);
        }

        protected override void BindServices()
        {
            base.BindServices();

            this.Bind<IUtilitiesSuiteService, UtilitiesSuiteService>(lifestyle: Lifestyle.Transient);
            this.Bind<IFormsService, FormsService>(lifestyle: Lifestyle.Singleton);
            this.Bind<ISuiteService, SuiteService>(lifestyle: Lifestyle.Singleton);
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

            this.Bind<ITaskManager>(container =>
            {
                return new SimpleTaskManager(
                    container.GetInstance<IOrganiserFactory>(),
                    container.GetInstance<ITaskProvider>(),
                    container.GetInstance<ITaskHistoryProvider>());
            }, lifestyle: Lifestyle.Singleton);

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
            return ConfigurationManager.AppSettings[UiBindings.KeyApplicationPath];
        }

        private string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}