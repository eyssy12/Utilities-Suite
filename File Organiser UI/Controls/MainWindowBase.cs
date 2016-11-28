namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using Commands;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Organiser.Library.Factories;
    using Services;
    using Views;

    public abstract class MainWindowBase : Window, IMainWindow
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly IViewNavigator Navigator;
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

            this.Navigator = new ViewNavigator(controls);
            this.Navigator.OnViewChanged += Navigator_OnViewChanged;
        }

        public IViewControl ActiveView
        {
            get { return this.Navigator.ActiveView; }
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

        private void Navigator_OnViewChanged(object sender, EventArgs<IViewControl, object> e)
        {
            e.First.InitialiseView(e.Second);

            this.OnPropertyChanged(nameof(this.ActiveView));
        }
    }
}