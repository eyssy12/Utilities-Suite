namespace File.Organiser.UI.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Controls;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.UI.Library.Services;
    using ViewModels;

    public partial class AddTask : ViewControlBase
    {
        public const string ViewName = nameof(AddTask);

        protected readonly AddTaskViewModel TempModel;

        protected readonly ISnackbarNotificationService Notifier;

        public AddTask(IOrganiserFactory factory)
            : base(AddTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();

            this.TempModel = new AddTaskViewModel();

            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();

            this.DataContext = this.TempModel;
        }

        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = sender as Button;

                if (button.Name == "Name_ButtonSave")
                {
                    this.PerformSave();
                }
                else if (button.Name == "Name_ButtonDiscard")
                {
                    this.PerformDiscard();
                }

                this.OnViewChange(Home.ViewName);
            }
        }

        private void PerformSave()
        {
            this.Notifier.Notify("New task created.");
        }

        private void PerformDiscard()
        {
            this.ResetFields();

            this.Notifier.Notify("Item discarded.");
        }

        private void ResetFields()
        {
            this.TempModel.Description = string.Empty;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }
    }
}