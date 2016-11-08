namespace File_Organiser_UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Core.Library.Extensions;
    using EyssyApps.Organiser.Library.Managers;
    using EyssyApps.UI.Library.Controls;
    using File.Organiser.UI;
    using File.Organiser.UI.Views;
    using Hardcodet.Wpf.TaskbarNotification;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        protected readonly TaskbarIcon TrayIcon;

        protected readonly ITaskManager Manager;
        protected readonly IEnumerable<UserControl> Views;

        protected UserControl homeView;
        protected UserControl individualTaskView;
        protected UserControl addTaskView;

        protected UserControl activeView;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.InitializeMaterialDesign();
            this.InitializeComponent();

            // TODO: investigate ReactiveUI

            this.TrayIcon = new TaskbarIcon();
            this.TrayIcon.MenuActivation = PopupActivationMode.LeftOrRightClick;
            this.TrayIcon.Visibility = Visibility.Hidden;
            this.TrayIcon.ContextMenu = this.FindResource("TrayContextMenu") as ContextMenu;
            this.TrayIcon.Icon = UiResources.App;
            this.TrayIcon.ToolTipText = "File Organiser";
            this.TrayIcon.TrayMouseDoubleClick += TrayIcon_TrayMouseDoubleClick;

            // TODO: this is shit implementation and needs rework, but serves as a first step to achieve the desired goal
            this.Views = new List<UserControl>
            {
                new Home(),
                new IndividualTask(),
                new AddTask()
            };

            this.Views.ForEach(v => 
            {
                (v as IViewControl).ChangeView += MainWindow_ChangeView;
            });

            this.homeView = this.Views.ElementAt(0);
            this.individualTaskView = this.Views.ElementAt(1);
            this.addTaskView = this.Views.ElementAt(2);

            this.activeView = this.homeView;

            this.DataContext = this;
        }

        public UserControl ActiveView
        {
            get { return this.activeView; }
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

        private void MainWindow_ChangeView(object sender, EventArgs<string> e)
        {
            if (e.First == "HomeView")
            {
                this.activeView = this.homeView;
            }
            else if (e.First == "AddTaskView")
            {
                this.activeView = this.addTaskView;
            }

            this.OnPropertyChanged(nameof(this.ActiveView));
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
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}