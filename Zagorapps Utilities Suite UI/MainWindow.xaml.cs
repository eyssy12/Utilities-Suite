namespace Zagorapps.Utilities.Suite.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using Attributes;
    using Controls;
    using Core.Library.Windows;
    using IoC;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;
    using Suites;
    using ViewModels;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Organiser.Library.Factories;
    using static Enumerations;

    public partial class MainWindow : MainWindowBase
    {
        public const string ElementMainSnackbar = "MainSnackbar",
            ElementTrayContextMenu = "TrayContextMenu";

        protected readonly IApplicationRegistryManager RegistryManager;
        protected readonly IApplicationConfigurationManager ConfigManager;
        protected readonly ISystemTrayControl Tray;

        public MainWindow(IOrganiserFactory factory)
            : base(factory)
        {
            this.InitializeMaterialDesign();
            this.InitializeComponent();
            
            this.RegistryManager = this.Factory.Create<IApplicationRegistryManager>();
            this.ConfigManager = this.Factory.Create<IApplicationConfigurationManager>();
            this.Tray = this.Factory.Create<ISystemTrayControl>();
            this.Tray.StateChanged += Tray_StateChanged;

            this.DataContext = this;

            this.Notifier.Notify("Hello, " + Environment.UserName + "!");

            this.SuiteManager.Navigate(FileOrganiserSuite.Name, null);
        }

        public string MainColorzoneText
        {
            get { return Environment.UserName + "'s Utilities Suite"; }
        }

        public IEnumerable<SuiteViewModel> SuiteItems
        {
            get
            {
                return Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsDefined(typeof(SuiteAttribute), false))
                    .Select(t => t.GetCustomAttribute<SuiteAttribute>())
                    .Select(a => new SuiteViewModel
                    { 
                        Identifier = a.Name,
                        FriendlyName = a.FriendlyName
                    })
                    .ToArray();
            }
        }

        public bool RunOnStartup
        {
            get { return this.ConfigManager.ReadBoolean(ApplicationConfigurationManager.SectionSettings, ApplicationConfigurationManager.KeyRunOnStartup, false); }
            set { this.ConfigManager.SetValue(ApplicationConfigurationManager.SectionSettings, ApplicationConfigurationManager.KeyRunOnStartup, value); }
        }

        public override void ShowWindow()
        {
            base.ShowWindow();

            this.Tray.SetVisibility(Visibility.Hidden);
        }

        protected void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar)
                {
                    return;
                }
                    
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        // minimize to system tray when main window is closed
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
            
            this.Tray.SetVisibility(Visibility.Visible);

            base.OnClosing(e);
        }

        protected void Tray_StateChanged(object sender, EventArgs<TrayState> e)
        {
            if (e.First == TrayState.ShowApplication)
            {
                this.ShowWindow();
            }
        }

        protected void StartupToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton)sender;

            this.ConfigManager.Save();

            this.RegistryManager.SetRunOnStartup(toggle.IsChecked.Value, Assembly.GetExecutingAssembly().Location);
        }

        protected void DemoItemsListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                // ListBox item clicked - do some cool things here

                SuiteViewModel suite = item.DataContext as SuiteViewModel;

                this.SuiteManager.Navigate(suite.Identifier, null);
            }
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
    }
}