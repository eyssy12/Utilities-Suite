namespace Zagorapps.Utilities.Suite.UI.Views.Organiser
{
    using System.Windows;
    using System.Windows.Input;
    using Commands;
    using Controls;
    using Services;
    using ViewModels;
    using Zagorapps.Core.Library.Windows;
    using Zagorapps.Organiser.Library.Factories;
    using Zagorapps.Organiser.Library.Providers;

    public partial class IndividualTask : ViewControlBase
    {
        public const string ViewName = nameof(IndividualTask);

        protected readonly ISnackbarNotificationService Notifer;
        protected readonly ITaskHistoryProvider Provider;
        protected readonly IWinSystemService WinSystem;

        private ICommand openHistoryFolderCommand;

        public IndividualTask(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(IndividualTask.ViewName, factory, commandProvider)
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

            this.openHistoryFolderCommand = this.CommandProvider.CreateRelayCommand(() => this.WinSystem.OpenFolder(this.Provider.GetStorePath(model.Reference)));
        }

        public override void FinaliseView()
        {
            
        }

        public override void SupplyData(object data)
        {
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