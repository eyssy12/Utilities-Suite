namespace File_Organiser_UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using EyssyApps.Configuration.Library;
    using EyssyApps.Core.Library.Managers;
    using EyssyApps.Core.Library.Native;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.Organiser.Library.Models.Settings;
    using EyssyApps.Organiser.Library.Providers;
    using EyssyApps.Organiser.Library.Tasks;
    using File.Organiser.UI;
    using Hardcodet.Wpf.TaskbarNotification;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;
    using SimpleInjectorContainer = SimpleInjector.Container;
    using WindowsDataFormats = System.Windows.DataFormats;
    using WindowsDragEventArgs = System.Windows.DragEventArgs;

    public partial class MainWindow : Window
    {
        protected readonly TaskbarIcon TrayIcon;

        protected readonly FileOrganiserTask FileTask;

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
            this.TrayIcon.Icon = UiResources.TrayIcon;
            this.TrayIcon.ToolTipText = "File Organiser";
            this.TrayIcon.TrayMouseDoubleClick += TrayIcon_TrayMouseDoubleClick;

            // TODO: Do proper refactoring of this logic here when the UI has functionality for task creation and management
            // 1. Create a new Simple Injector container
            SimpleInjectorContainer container = new SimpleInjectorContainer();

            // 2. Configure the container (register)
            SimpleInjectorBindings bindings = new SimpleInjectorBindings();
            bindings.RegisterBindingsToContainer(container);
            
            IOrganiserFactory factory = container.GetInstance<IOrganiserFactory>();
            IFileManager fileManager = factory.Create<IFileManager>();
            IDirectoryManager directoryManager = factory.Create<IDirectoryManager>();
            IFileExtensionProvider provider = factory.Create<IFileExtensionProvider>();

            // TODO: dont allow to create tasks of the same type for the same root path, i.e. Two seperate tasks for directory organiser with the same root path
            FileOrganiserSettings settings = new FileOrganiserSettings
            {
                RootPath = KnownFolders.GetPath(KnownFolder.Downloads),
                DirectoryExemptions = new List<string> { },
                ExtensionExemptions = new List<string> { },
                FileExemptions = new List<string>() { }
            };

            this.FileTask = new FileOrganiserTask(Guid.NewGuid(), "Sorts the files in the Downloads folder", settings, provider, fileManager, directoryManager);
            DirectoryOrganiserTask directoryTask = new DirectoryOrganiserTask(Guid.NewGuid(), "Sorts the individual directories in the Downloads folder", settings, directoryManager);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                this.FileTask.Execute();
            });
        }
    }
}
