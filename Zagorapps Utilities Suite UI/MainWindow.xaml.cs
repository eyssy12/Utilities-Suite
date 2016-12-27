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
    using Controls;
    using Core.Library.Windows;
    using Extensions;
    using IoC;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;
    using ViewModels;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Utilities.Library.Factories;

    public partial class MainWindow : MainWindowBase
    {
        public const string ElementMainSnackbar = "MainSnackbar",
            ElementTrayContextMenu = "TrayContextMenu";

        protected readonly IApplicationRegistryManager RegistryManager;
        protected readonly IApplicationConfigurationManager ConfigManager;
        protected readonly ISystemTrayControl Tray;

        protected readonly MainWindowViewModel Model;

        public MainWindow(IOrganiserFactory factory)
            : base(factory)
        {
            this.InitializeMaterialDesign();
            this.InitializeComponent();
            
            this.RegistryManager = this.Factory.Create<IApplicationRegistryManager>();
            this.ConfigManager = this.Factory.Create<IApplicationConfigurationManager>();
            this.Tray = this.Factory.Create<ISystemTrayControl>();
            this.Tray.StateChanged += Tray_StateChanged;

            this.Model = new MainWindowViewModel();

            this.DataContext = this;

            this.Notifier.Notify("Hello, " + Environment.UserName + "!");

            this.Model.FormatAndSetMainColorzoneText(this.SuiteManager.ActiveSuite.Identifier);
        }

        public MainWindowViewModel ViewModel
        {
            get { return this.Model; }
        }

        public IEnumerable<SuiteViewModel> SuiteItems
        {
            get
            {
                return Assembly
                    .GetExecutingAssembly()
                    .GetAllSuitesOrderByDefaultNavigatable()
                    .Select((a, index) => new SuiteViewModel
                    { 
                        Identifier = a.Item1.Name,
                        FriendlyName = (index + 1) + " - " + a.Item1.FriendlyName
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
            ListBox listbox = sender as ListBox;
            ListBoxItem item = ItemsControl.ContainerFromElement(listbox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                // ListBox item clicked - do some cool things here

                SuiteViewModel suite = item.DataContext as SuiteViewModel;
                this.Model.SuiteIndex = listbox.SelectedIndex;

                this.SuiteManager.Navigate(suite.Identifier, null);
            }
        }

        protected void MainWindowBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Model.IsSuiteChangeWithKeyboardShortcutApplied(e, this.SuiteItems.Count() - 1))
            {
                this.SuiteManager.Navigate(this.SuiteItems.ElementAt(this.Model.SuiteIndex).Identifier, null);
                this.Model.FormatAndSetMainColorzoneText(this.SuiteManager.ActiveSuite.Identifier);
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