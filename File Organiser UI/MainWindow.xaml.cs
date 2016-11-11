namespace File_Organiser_UI
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using File.Organiser.UI;
    using File.Organiser.UI.Controls;
    using Hardcodet.Wpf.TaskbarNotification;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;

    public partial class MainWindow : MainWindowBase
    {
        protected readonly TaskbarIcon TrayIcon;

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

            this.DataContext = this;

            this.MainSnackbar.MessageQueue.Enqueue("Hello, user.");
        }

        private void InitializeMaterialDesign()
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

        protected override void ShowWindow()
        {
            base.ShowWindow();

            this.TrayIcon.Visibility = Visibility.Hidden;
        }

        private void TrayMenu_CloseFileOrganiser(object sender, EventArgs e)
        {
            this.TerminateApplication(sender, null);
        }

        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.ShowWindow();
        }

        private void TrayMenu_OpenFileOrganiser(object sender, RoutedEventArgs e)
        {
            this.ShowWindow();
        }
    }
}