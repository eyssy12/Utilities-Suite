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
    using WindowsInput;
    using Zagorapps.Configuration.Library;
    using Zagorapps.Core.Library.Managers;
    using Zagorapps.Utilities.Library.Factories;
    using Zagorapps.Utilities.Library.Managers;
    using Zagorapps.Utilities.Library.Models.Organiser;
    using Zagorapps.Utilities.Library.Providers;
    using FileIO = System.IO.File;
    using PathIO = System.IO.Path;

    public class UiBindings : BindingsBase
    {
        protected const string KeyApplicationPath = "Path_Application",
            KeyFileExtensionJsonFile = "Name_FileExtensionJson",
            KeyConfigurationFileName = "Name_ConfigurationFile",
            KeyHistoryStore = "Store_History",
            KeySettingsStore = "Store_Settings",
            KeyTasksStore = "Store_Tasks",
            KeyOrganiserTcp = "Tcp_Organiser",
            KeyDashboardTcp = "Tcp_Dashboard",
            KeyConnectivityTcp = "Tcp_Connectivity",
            KeySystemControlTcp = "Tcp_SystemControl",
            KeyUtilitiesEndpoint = "Endpoint_Utilities";

        protected override void RegisterBindings()
        {
            this.RegisterWindows();
            this.RegisterViews();
            this.RegisterCommunications();
            this.RegisterControls();
            this.RegisterProviders();
            this.RegisterLoggers();
            this.RegisterDataFacilitatorManager();
            this.RegisterMisc();
            this.RegisterServices();
            this.RegisterManagers();
        }

        protected virtual void RegisterMisc()
        {
            this.Register<IInputSimulator>(container =>
            {
                return new InputSimulator();
            }, lifestyle: Lifestyle.Transient);
        }

        protected virtual void RegisterDataFacilitatorManager()
        {
            this.Register<IDataFacilitatorSuiteManager>(container =>
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
                    new NoBluetoothAvailable(factory, commandProvider),
                    new BluetoothInteraction(factory, commandProvider)
                };

                IEnumerable<IViewControl> dashboardControls = new List<IViewControl>
                {
                    new Dashboard(factory, commandProvider)
                };

                IEnumerable<IViewControl> systemControls = new List<IViewControl>
                {
                    new WindowsControls(factory, commandProvider)
                };

                IEnumerable<IReceiveSuiteData> systemReceivers = new List<IReceiveSuiteData>
                {
                    new WcfReceiveSuiteData(factory, commsProvider, this.GetValue(UiBindings.KeySystemControlTcp))
                };

                IEnumerable<ISendSuiteData> systemSenders = new List<ISendSuiteData>
                {
                    new WcfSendSuiteData(commsProvider, SuiteRoute.Dashboard, this.GetValue(UiBindings.KeyUtilitiesEndpoint), this.GetValue(UiBindings.KeyDashboardTcp))
                };

                IEnumerable<IReceiveSuiteData> dashboardReceivers = new List<IReceiveSuiteData>
                {
                    new WcfReceiveSuiteData(factory, commsProvider, this.GetValue(UiBindings.KeyDashboardTcp))
                };

                IEnumerable<ISendSuiteData> dashboardSenders = new List<ISendSuiteData>
                {
                    new WcfSendSuiteData(commsProvider, SuiteRoute.SystemControl, this.GetValue(UiBindings.KeyUtilitiesEndpoint), this.GetValue(UiBindings.KeySystemControlTcp))
                };

                IEnumerable<IReceiveSuiteData> connectivityReceivers = new List<IReceiveSuiteData>
                {
                    new WcfReceiveSuiteData(factory, commsProvider, this.GetValue(UiBindings.KeyConnectivityTcp))
                };

                IEnumerable<ISendSuiteData> connectivitySenders = new List<ISendSuiteData>
                {
                    new WcfSendSuiteData(commsProvider, SuiteRoute.SystemControl, this.GetValue(UiBindings.KeyUtilitiesEndpoint), this.GetValue(UiBindings.KeySystemControlTcp))
                };

                IEnumerable<ISuite> suites = new List<ISuite>
                {
                    new DashboardSuite(dashboardControls, dashboardReceivers, dashboardSenders),
                    new FileOrganiserSuite(organiserControls),
                    new ConnectivitySuite(connectivityControls, connectivityReceivers, connectivitySenders),
                    new SystemSuite(systemControls, systemReceivers, systemSenders)
                };

                return new DataFacilitatorSuiteManager(suites);
            }, lifestyle: Lifestyle.Transient);
        }

        protected virtual void RegisterCommunications()
        {
        }

        protected virtual void RegisterLoggers()
        {
        }

        protected virtual void RegisterProviders()
        {
            this.Register<ICommandProvider, CommandProvider>();

            this.Register<IFileExtensionProvider>((container) =>
            {
                string fileExtensionJson = ConfigurationManager.AppSettings[UiBindings.KeyFileExtensionJsonFile];
                string filePath = PathIO.Combine(this.GetApplicationPath(), fileExtensionJson);

                string data = FileIO.ReadAllText(filePath);

                FileExtensionDatabaseModel result = JsonConvert.DeserializeObject<FileExtensionDatabaseModel>(data);

                return new FileExtensionProvider(result);
            }, lifestyle: Lifestyle.Singleton);

            this.Register<ITaskHistoryProvider>(container =>
            {
                return new TaskHistoryProvider(
                    ConfigurationManager.AppSettings[UiBindings.KeyHistoryStore],
                    container.GetInstance<IFileManager>(),
                    container.GetInstance<IDirectoryManager>());
            }, lifestyle: Lifestyle.Transient);

            this.Register<IOrganiserSettingsProvider>(container =>
            {
                return new OrganiserSettingsProvider(
                    ConfigurationManager.AppSettings[UiBindings.KeySettingsStore],
                    container.GetInstance<IFileManager>(),
                    container.GetInstance<IDirectoryManager>());
            }, lifestyle: Lifestyle.Transient);

            this.Register<ITaskProvider>(container =>
            {
                return new TaskProvider(
                    ConfigurationManager.AppSettings[UiBindings.KeyTasksStore],
                    container.GetInstance<IOrganiserFactory>(),
                    container.GetInstance<IFileManager>(),
                    container.GetInstance<IDirectoryManager>());
            }, lifestyle: Lifestyle.Transient);

            this.Register<ICommunicationsProvider, CommunicationsProvider>(lifestyle: Lifestyle.Transient);
        }

        protected virtual void RegisterServices()
        {
            this.Register<IUtilitiesSuiteService, UtilitiesSuiteService>(lifestyle: Lifestyle.Transient);
            this.Register<IFormsService, FormsService>(lifestyle: Lifestyle.Singleton);
            this.Register<ISuiteService, SuiteService>(lifestyle: Lifestyle.Singleton);
        }

        protected virtual void RegisterWindows()
        {
            this.Register<IMainWindow, MainWindow>();
        }

        protected virtual void RegisterViews()
        {
            // TODO: bind views
        }

        protected virtual void RegisterManagers()
        {
            this.Register<ITaskManager>(container =>
            {
                return new SimpleTaskManager(
                    container.GetInstance<IOrganiserFactory>(),
                    container.GetInstance<ITaskProvider>(),
                    container.GetInstance<ITaskHistoryProvider>());
            }, lifestyle: Lifestyle.Singleton);

            this.Register<IApplicationConfigurationManager>(container =>
            {
                string configName = ConfigurationManager.AppSettings[UiBindings.KeyConfigurationFileName];

                return new ApplicationConfigurationManager(PathIO.Combine(this.GetApplicationPath(), configName));
            }, lifestyle: Lifestyle.Singleton);
        }

        protected virtual void RegisterControls()
        {
            this.Register<ISystemTrayControl>(container =>
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