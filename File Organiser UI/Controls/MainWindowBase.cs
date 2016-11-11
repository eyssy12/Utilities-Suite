namespace File.Organiser.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.UI.Library.Controls;
    using IoC;
    using MaterialDesignThemes.Wpf;
    using Views;

    public abstract class MainWindowBase : Window, IMainWindow
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly IViewNavigator Navigator;

        private readonly Lazy<Snackbar> Snackbar;

        protected MainWindowBase()
        {
            this.Factory = DependencyProvider.Get<IOrganiserFactory>();

            this.Snackbar = new Lazy<Snackbar>(() => (Snackbar)Application.Current.MainWindow.FindName("MainSnackbar"));

            // TODO: these should be bindings - figure out how to register a collection in simple injector the way i need it to
            IEnumerable<IViewControl> controls = new List<IViewControl>
            {
                new Home(this.Factory),
                new AddTask(this.Factory),
                new IndividualTask(this.Factory)
            };

            this.Navigator = new ViewNavigator(controls);
            this.Navigator.OnViewChanged += Navigator_OnViewChanged;
            this.Navigator.Navigate(Home.ViewName);
        }

        public IViewControl ActiveView
        {
            get { return this.Navigator.ActiveView; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void ShowWindow()
        {
            this.Show();
            this.BringIntoView();

            this.WindowState = WindowState.Normal;
        }

        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void TerminateApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Navigator_OnViewChanged(object sender, EventArgs<IViewControl> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }
    }
}