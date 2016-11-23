namespace File.Organiser.UI.Views
{
    using System.Windows;
    using System.Windows.Input;
    using Commands;
    using Controls;
    using EyssyApps.Core.Library.Windows;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.Organiser.Library.Providers;
    using EyssyApps.UI.Library.Services;
    using ViewModels;

    public partial class IndividualTask : ViewControlBase
    {
        public const string ViewName = nameof(IndividualTask);

        protected readonly ISnackbarNotificationService Notifer;
        protected readonly ITaskHistoryProvider Provider;
        protected readonly IWinSystemService WinSystem;

        private ICommand openHistoryFolderCommand;

        public IndividualTask(IOrganiserFactory factory)
            : base(IndividualTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();

            this.Notifer = this.Factory.Create<ISnackbarNotificationService>();
            this.Provider = this.Factory.Create<ITaskHistoryProvider>();
            this.WinSystem = this.Factory.Create<IWinSystemService>();
        }

        public ICommand OpenHistoryFolderCommand
        {
            get { return this.openHistoryFolderCommand; }
            protected set { this.openHistoryFolderCommand = value; }
        }

        public override void InitialiseView(object arg)
        {
            TaskViewModel model = arg as TaskViewModel;

            this.Notifer.Notify("Viewing task " + model.Identity.ToString());

            this.openHistoryFolderCommand = new RelayCommand<object>(param => this.WinSystem.OpenFolder(this.Provider.GetStorePath(model.Reference)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName, null);

            this.openHistoryFolderCommand = null;
        }

        private void Button_OpenHistory_Click(object sender, RoutedEventArgs e)
        {
            this.OpenHistoryFolderCommand.Execute(null);
        }
    }
}