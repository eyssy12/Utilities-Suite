namespace File.Organiser.UI
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Organiser.Library.Factories;
    using File.Organiser.UI.Controls;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;
    using Views;
    using static Enumerations;

    public partial class MainWindow : MainWindowBase
    {
        public const string ElementMainSnackbar = "MainSnackbar",
            ElementTrayContextMenu = "TrayContextMenu";

        protected readonly ISystemTrayControl Tray;

        public MainWindow(IOrganiserFactory factory)
            : base(factory)
        {
            this.InitializeMaterialDesign();
            this.InitializeComponent();

            this.Tray = this.Factory.Create<ISystemTrayControl>();
            this.Tray.StateChanged += Tray_StateChanged;

            this.DataContext = this;

            this.Navigator.Navigate(Home.ViewName, null);

            this.Notifier.Notify("Hello, " + Environment.UserName + "!");
        }
        
        public override void ShowWindow()
        {
            base.ShowWindow();

            this.Tray.SetVisibility(Visibility.Hidden);
        }

        // minimize to system tray when main window is closed
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
            
            this.Tray.SetVisibility(Visibility.Visible);

            base.OnClosing(e);
        }

        private void Tray_StateChanged(object sender, EventArgs<TrayState> e)
        {
            if (e.First == TrayState.ShowApplication)
            {
                this.ShowWindow();
            }
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
    }
}