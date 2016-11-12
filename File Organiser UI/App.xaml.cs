namespace File.Organiser.UI
{
    using System;
    using System.Windows;
    using Controls;
    using EyssyApps.UI.Library.Services;
    using File.Organiser.UI.IoC;
    using MaterialDesignThemes.Wpf;
    using SimpleInjector;
    using ApplicationMainWindow = File.Organiser.UI.MainWindow;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string Name = "File Organiser",
            ControlTrayContextMenu = "TrayContextMenu",
            MenuItemOpenApplication = "OpenApplicationMenuItem",
            MenuItemCloseApplication = "CloseApplicationMenuItem";

        public App()
        {
            DependencyProvider.Bind(typeof(ISnackbarNotificationService), () =>
            {
                Lazy<Snackbar> snackbar = new Lazy<Snackbar>(() => (Snackbar)Application.Current.MainWindow.FindName(ApplicationMainWindow.ElementMainSnackbar));

                return new SnackbarNotificationService(snackbar);
            }, Lifestyle.Singleton);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IMainWindow mainWindow = DependencyProvider.Get<IMainWindow>();
            mainWindow.ShowWindow();
        }
    }
}