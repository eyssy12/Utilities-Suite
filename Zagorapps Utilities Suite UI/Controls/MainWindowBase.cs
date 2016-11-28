namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using Commands;
    using Services;
    using Suites;
    using Views;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Organiser.Library.Factories;

    public abstract class MainWindowBase : Window, IMainWindow
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly ISuiteNavigator SuiteNavigator;
        protected readonly ISnackbarNotificationService Notifier;

        protected MainWindowBase(IOrganiserFactory factory)
        {
            this.Factory = factory;

            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();

            ICommandProvider provider = this.Factory.Create<ICommandProvider>();

            //this.Navigator = this.Factory.Create<IViewNavigator>();

            //// TODO: these should be bindings - figure out how to register a collection in simple injector the way i need it to
            IEnumerable<IViewControl> controls = new List<IViewControl>
            {
                new Home(this.Factory, provider),
                new AddTask(this.Factory, provider),
                new IndividualTask(this.Factory, provider)
            };

            IEnumerable<IViewControl> tempControls = new List<IViewControl>
            {
                new TempControl(this.Factory, provider)
            };

            this.SuiteNavigator = new SuiteNavigator(new List<ISuite>
            {
                new FileOrganiserSuite(controls),
                new TempSuite(tempControls)
            });

            this.SuiteNavigator.OnEntityChanged += SuiteNavigator_OnEntityChanged;
            this.SuiteNavigator.OnViewChanged += SuiteNavigator_OnViewChanged;
        }

        private void SuiteNavigator_OnViewChanged(object sender, EventArgs<IViewControl, object> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }

        private void SuiteNavigator_OnEntityChanged(object sender, EventArgs<ISuite, object> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }

        public IViewControl ActiveView
        {
            get { return this.SuiteNavigator.ActiveSuiteView; }
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