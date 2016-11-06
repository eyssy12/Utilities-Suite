namespace File_Organiser_UI
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using EyssyApps.Organiser.Library.Managers;
    using File.Organiser.UI;
    using Hardcodet.Wpf.TaskbarNotification;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;

    public partial class MainWindow : Window
    {
        protected readonly TaskbarIcon TrayIcon;

        protected readonly ITaskManager Manager;

        public MainWindow()
        {
            this.InitializeMaterialDesign();
            this.InitializeComponent();

            // TODO: use modern ui wpf library to get a nice look
            // TODO: add functionality for registering the application as "run on startup" using registry keying
            // TODO: investigate ReactiveUI, Material Design for XAML

            this.TrayIcon = new TaskbarIcon();
            this.TrayIcon.MenuActivation = PopupActivationMode.LeftOrRightClick;
            this.TrayIcon.Visibility = Visibility.Hidden;
            this.TrayIcon.ContextMenu = this.FindResource("TrayContextMenu") as ContextMenu;
            this.TrayIcon.Icon = UiResources.App;
            this.TrayIcon.ToolTipText = "File Organiser";
            this.TrayIcon.TrayMouseDoubleClick += TrayIcon_TrayMouseDoubleClick;

            this.DataContext = this;
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

        //private void ImagePanel_Drop(object sender, WindowsDragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(WindowsDataFormats.FileDrop))
        //    {
        //        // Note that you can have more than one file.
        //        string[] files = (string[])e.Data.GetData(WindowsDataFormats.FileDrop);

        //        Console.WriteLine(files);

        //        this.FilesLabel.Content = files.Aggregate((a, b) => a + "\n" + b);
        //    }
        //}

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
    }
}