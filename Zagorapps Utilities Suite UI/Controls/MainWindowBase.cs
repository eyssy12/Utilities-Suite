namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using Library.Interoperability;
    using Managers;
    using Navigation;
    using Services;
    using Suites;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Utilities.Suite.Library.Factories;

    public abstract class MainWindowBase : Window, IMainWindow
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly ISuiteService SuiteService;
        protected readonly IDataFacilitatorSuiteManager SuiteManager;
        protected readonly ISnackbarNotificationService Notifier;

        protected MainWindowBase(IOrganiserFactory factory)
        {
            this.Factory = factory;

            Application.Current.Exit += this.Application_Exiting;
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            AppDomain.CurrentDomain.UnhandledException += this.Domain_UnhandledException;

            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
            this.SuiteService = this.Factory.Create<ISuiteService>();
            this.SuiteManager = this.Factory.Create<IDataFacilitatorSuiteManager>();

            this.SuiteManager.OnSuiteChanged += this.SuiteManager_OnSuiteChanged;
            this.SuiteManager.OnSuiteViewChanged += this.SuiteManager_OnSuiteViewChanged;
            this.SuiteService.OnSuiteChangeRequested += this.SuiteService_OnSuiteChangeRequested;

            this.SuiteManager.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IViewControl ActiveView
        {
            get { return this.SuiteManager.ActiveSuiteView; }
        }

        public IInteropHandle InteropHandle
        {
            get { return this.Factory.Create<IInteropHandle>(); }
        }

        public object TryRetrieveResource(string name)
        {
            return this.TryFindResource(name);
        }

        public virtual void ShowWindow()
        {
            this.WindowState = WindowState.Normal;

            this.Show();
            this.BringIntoView();
        }

        public virtual void CloseWindow()
        {
            this.Close();
        }

        protected virtual void SuiteService_OnSuiteChangeRequested(object sender, EventArgs<string> e)
        {
            this.SuiteManager.Navigate(e.First, null);
        }

        protected virtual void SuiteManager_OnSuiteViewChanged(object sender, EventArgs<IViewControl, object> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }

        protected virtual void SuiteManager_OnSuiteChanged(object sender, EventArgs<ISuite, object> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }

        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void TerminateApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // TODO: initiate shutdown procedure on all modules in the application (e.g. close connectivity view and active connections)
            Console.WriteLine("exiting");
        }

        private void Application_Exiting(object sender, ExitEventArgs e)
        {
            // TODO: initiate shutdown procedure on all modules in the application
            Console.WriteLine("exiting");
        }
    }
}