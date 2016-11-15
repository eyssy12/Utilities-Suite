namespace File.Organiser.UI.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Controls;
    using EyssyApps.Organiser.Library;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.Organiser.Library.Managers;
    using EyssyApps.Organiser.Library.Tasks;
    using EyssyApps.UI.Library.Services;
    using ViewModels;

    public partial class AddTask : ViewControlBase
    {
        public const string ViewName = nameof(AddTask);

        protected readonly AddTaskViewModel Model;

        protected readonly ITaskManager Manager;
        protected readonly ISnackbarNotificationService Notifier;

        public AddTask(IOrganiserFactory factory)
            : base(AddTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();

            this.Model = new AddTaskViewModel();

            this.Manager = this.Factory.Create<ITaskManager>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();

            this.DataContext = this.Model;
        }

        public override void ActivateView()
        {
            this.Model.Identity = Guid.NewGuid().ToString();
            this.Model.Description = string.Empty;
            this.Model.OrganiseType = string.Empty;
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
            ITask task = this.CreateTask();

            this.Manager.Add(task);

            this.Notifier.Notify("New task created.");
        }

        private void PerformDiscard()
        {
            this.Notifier.Notify("Item discarded.");
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }

        private ITask CreateTask()
        {
            // TODO: refactor this 
            //TaskType enumTaskType = (TaskType)Enum.Parse(typeof(TaskType), this.Model.TaskType);
            //OrganiseType organiseType = (OrganiseType)Enum.Parse(typeof(OrganiseType), this.Model.OrganiseType);
            TaskType taskType = TaskType.Organiser;
            OrganiseType organiseType = OrganiseType.File;

            Guid guid = Guid.Parse(this.Model.Identity);

            if (taskType == TaskType.Organiser)
            {
                if (organiseType == OrganiseType.File)
                {
                    return new FileOrganiserTask(guid, this.Model.Description, null, null, null, null);
                }

                return new DirectoryOrganiserTask(guid, this.Model.Description, null, null);
            }

            return new ScheduledTask(guid, this.Model.Description, null, null, 1000, 1000);
        }
    }
}