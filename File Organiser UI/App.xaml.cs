namespace File.Organiser.UI
{
    using System;
    using System.Windows;
    using Controls;
    using File.Organiser.UI.IoC;
    using MaterialDesignThemes.Wpf;
    using Services;
    using SimpleInjector;
    using ApplicationMainWindow = File.Organiser.UI.MainWindow;

    public partial class App : Application
    {
        public const string Name = "File Organiser",
            ControlTrayContextMenu = "TrayContextMenu",
            MenuItemOpenApplication = "OpenApplicationMenuItem",
            MenuItemCloseApplication = "CloseApplicationMenuItem";

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
            ServiceLocator.GetFactory().Create<IMainWindow>().ShowWindow();
        }
    }
}