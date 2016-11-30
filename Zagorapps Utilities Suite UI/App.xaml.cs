namespace Zagorapps.Utilities.Suite.UI
{
    using System;
    using System.Threading;
    using System.Windows;
    using Controls;
    using MaterialDesignThemes.Wpf;
    using Services;
    using SimpleInjector;
    using Zagorapps.Organiser.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.IoC;
    using ApplicationMainWindow = Zagorapps.Utilities.Suite.UI.MainWindow;
    public partial class App : Application
    {
        public const string Name = "File Organiser",
            ControlTrayContextMenu = "TrayContextMenu",
            MenuItemOpenApplication = "OpenApplicationMenuItem",
            MenuItemCloseApplication = "CloseApplicationMenuItem";

        private static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

        public App()
        {
            ServiceLocator.Bind(
                typeof(ISnackbarNotificationService),
                () =>
                {
                    // Inideal but unfortunately haven't been able to find a different way around this yet.
                    // By using Lazy here we're trusting the main window that there is a snackbar component that will be available once initialization of components is done.
                    Lazy<Snackbar> snackbar = new Lazy<Snackbar>(() => (Snackbar)Application.Current.MainWindow.FindName(ApplicationMainWindow.ElementMainSnackbar));

                    return new SnackbarNotificationService(snackbar);
                },
                Lifestyle.Singleton);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                IOrganiserFactory factory = ServiceLocator.GetFactory();

                IMainWindow mainWindow = factory.Create<IMainWindow>();

                IApplicationConfigurationManager config = factory.Create<IApplicationConfigurationManager>();

                if (config.ReadBoolean(ApplicationConfigurationManager.SectionSettings, ApplicationConfigurationManager.KeyRunOnStartup, false))
                {
                    mainWindow.CloseWindow();
                }
                else
                {
                    mainWindow.ShowWindow();
                }

                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("An instance of the application is already running!", "Application Running");
                Environment.Exit(0);
            }
        }
    }
}