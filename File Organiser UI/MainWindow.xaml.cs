namespace File_Organiser_UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.UI.Library.Controls;
    using File.Organiser.UI;
    using File.Organiser.UI.IoC;
    using File.Organiser.UI.Views;
    using Hardcodet.Wpf.TaskbarNotification;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        protected readonly TaskbarIcon TrayIcon;

        protected readonly IOrganiserFactory Factory;
        protected readonly IViewNavigator Navigator;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.InitializeMaterialDesign();
            this.InitializeComponent();

            this.Factory = DependencyProvider.Get<IOrganiserFactory>();

            // TODO: investigate ReactiveUI

            this.TrayIcon = new TaskbarIcon();
            this.TrayIcon.MenuActivation = PopupActivationMode.LeftOrRightClick;
            this.TrayIcon.Visibility = Visibility.Hidden;
            this.TrayIcon.ContextMenu = this.FindResource("TrayContextMenu") as ContextMenu;
            this.TrayIcon.Icon = UiResources.App;
            this.TrayIcon.ToolTipText = "File Organiser";
            this.TrayIcon.TrayMouseDoubleClick += TrayIcon_TrayMouseDoubleClick;

            // TODO: these should be bindings - figure out how to register a collection in simple injector the way i need it to
            IEnumerable<IViewControl> controls = new List<IViewControl>
            {
                new Home(),
                new AddTask(),
                new IndividualTask()
            };

            this.Navigator = new ViewNavigator(controls);
            this.Navigator.OnViewChanged += Navigator_OnViewChanged;

            this.DataContext = this;

            this.Navigator.Navigate(Home.ViewName);
        }

        private void Navigator_OnViewChanged(object sender, EventArgs<IViewControl> e)
        {
            this.OnPropertyChanged(nameof(this.ActiveView));
        }

        public IViewControl ActiveView
        {
            get { return this.Navigator.ActiveView; }
        }

        protected void InitializeMaterialDesign()
        {
            // Create dummy objects to force the MaterialDesign assemblies to be loaded
            // from this assembly, which causes the MaterialDesign assemblies to be searched
            // relative to this assembly's path. Otherwise, the MaterialDesign assemblies
            // are searched relative to Eclipse's path, so they're not found.
            var card = new Card();
            var hue = new Hue("Dummy", Colors.Black, Colors.White);
        }

        // minimize to system tray when main window is closed
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            this.Hide();

            this.TrayIcon.Visibility = Visibility.Visible;

            base.OnClosing(e);
        }

        private void TrayMenu_CloseFileOrganiser(object sender, EventArgs e)
        {
            this.TerminateApplication(sender, null);
        }

        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.ShowMainWindow();
        }

        private void TrayMenu_OpenFileOrganiser(object sender, RoutedEventArgs e)
        {
            this.ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            this.Show();
            this.BringIntoView();

            this.WindowState = WindowState.Normal;
            this.TrayIcon.Visibility = Visibility.Hidden;
        }
        
        private void TerminateApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}