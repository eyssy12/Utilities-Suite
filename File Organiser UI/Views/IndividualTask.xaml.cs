namespace File.Organiser.UI.Views
{
    using System.Windows;
    using Controls;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.UI.Library.Services;
    using ViewModels;

    public partial class IndividualTask : ViewControlBase
    {
        public const string ViewName = nameof(IndividualTask);

        protected readonly ISnackbarNotificationService Notifer;

        public IndividualTask(IOrganiserFactory factory)
            : base(IndividualTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();

            this.Notifer = this.Factory.Create<ISnackbarNotificationService>();
        }

        public override void InitialiseView(object arg)
        {
            TaskViewModel task = arg as TaskViewModel;

            this.Notifer.Notify("Viewing task " + task.Identity.ToString());
            // TODO: use values from task to display on the view model
            // Setup the view model 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName, null);
        }
    }
}