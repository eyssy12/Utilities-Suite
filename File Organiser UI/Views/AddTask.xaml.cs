namespace File.Organiser.UI.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using Controls;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.UI.Library.Services;

    public partial class AddTask : ViewControlBase
    {
        public const string ViewName = nameof(AddTask);

        protected readonly ISnackbarNotificationService Notifier;

        public AddTask(IOrganiserFactory factory)
            : base(AddTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();

            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = sender as Button;

                if (button.Name == "Name_ButtonSave")
                {
                    this.Notifier.Notify("New task created.");
                }
                else if (button.Name == "Name_ButtonDiscard")
                {
                    this.Notifier.Notify("Item discarded.");
                }

                this.OnViewChange(Home.ViewName);
            }
        }

        private void ResetFields()
        {

        }
    }
}