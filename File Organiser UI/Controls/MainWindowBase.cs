namespace File.Organiser.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.UI.Library.Services;
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

            //this.Navigator = this.Factory.Create<IViewNavigator>();

            //// TODO: these should be bindings - figure out how to register a collection in simple injector the way i need it to
            IEnumerable<IViewControl> controls = new List<IViewControl>
            {
                new Home(this.Factory),
                new AddTask(this.Factory),
                new IndividualTask(this.Factory)
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