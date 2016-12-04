namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Windows;
    using Managers;
    using Navigation;
    using Services;
    using Suites;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Utilities.Library.Factories;

    public abstract class MainWindowBase : Window, IMainWindow
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly ISuiteService SuiteService;
        protected readonly IDataFacilitatorSuiteManager SuiteManager;
        protected readonly ISnackbarNotificationService Notifier;

        protected MainWindowBase(IOrganiserFactory factory)
        {
            this.Factory = factory;

            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
            this.SuiteService = this.Factory.Create<ISuiteService>();
            this.SuiteManager = this.Factory.Create<IDataFacilitatorSuiteManager>();

            this.SuiteManager.OnSuiteChanged += SuiteManager_OnSuiteChanged;
            this.SuiteManager.OnSuiteViewChanged += SuiteManager_OnSuiteViewChanged;
            this.SuiteService.OnSuiteChangeRequested += SuiteService_OnSuiteChangeRequested;

            this.SuiteManager.Start();
        }

        private void SuiteService_OnSuiteChangeRequested(object sender, EventArgs<string> e)
        {
            this.SuiteManager.Navigate(e.First, null);
        }

        private void SuiteManager_OnSuiteViewChanged(object sender, EventArgs<IViewControl, object> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }

        private void SuiteManager_OnSuiteChanged(object sender, EventArgs<ISuite, object> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }

        public IViewControl ActiveView
        {
            get { return this.SuiteManager.ActiveSuiteView; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void TerminateApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}