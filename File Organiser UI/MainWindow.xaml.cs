namespace File_Organiser_UI
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using File.Organiser.UI;
    using Hardcodet.Wpf.TaskbarNotification;
    using WindowsDataFormats = System.Windows.DataFormats;
    using WindowsDragEventArgs = System.Windows.DragEventArgs;

    public partial class MainWindow : Window
    {
        protected readonly TaskbarIcon TrayIcon;

        public MainWindow()
        {
            InitializeComponent();

            this.TrayIcon = new TaskbarIcon();
            this.TrayIcon.MenuActivation = PopupActivationMode.LeftOrRightClick;
            this.TrayIcon.Visibility = Visibility.Hidden;
            this.TrayIcon.ContextMenu = this.FindResource("TrayContextMenu") as ContextMenu;
            this.TrayIcon.Icon = UiResources.TrayIcon;
            this.TrayIcon.ToolTipText = "File Organiser";
            this.TrayIcon.TrayMouseDoubleClick += TrayIcon_TrayMouseDoubleClick;
        }

        private void ImagePanel_Drop(object sender, WindowsDragEventArgs e)
        {
            if (e.Data.GetDataPresent(WindowsDataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(WindowsDataFormats.FileDrop);

                Console.WriteLine(files);

                this.FilesLabel.Content = files.Aggregate((a, b) => a + "\n" + b);
            }
        }

        // minimize to system tray when applicaiton is closed
        protected override void OnClosing(CancelEventArgs e)
        {
            // setting cancel to true will cancel the close request
            // so the application is not closed
            e.Cancel = true;

            this.Hide();

            this.TrayIcon.Visibility = Visibility.Visible;

            base.OnClosing(e);
        }

        private void TrayMenu_CloseFileOrganiser(object sender, EventArgs e)
        {
            Environment.Exit(0);
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
    }
} 