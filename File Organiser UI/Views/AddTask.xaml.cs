namespace File.Organiser.UI.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Controls;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Core.Library.Managers;
    using EyssyApps.Core.Library.Native;
    using EyssyApps.Core.Library.Timing;
    using EyssyApps.Organiser.Library;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.Organiser.Library.Managers;
    using EyssyApps.Organiser.Library.Models.Settings;
    using EyssyApps.Organiser.Library.Providers;
    using EyssyApps.Organiser.Library.Tasks;
    using EyssyApps.UI.Library.Services;
    using ViewModels;

    public partial class AddTask : ViewControlBase
    {
        public const string ViewName = nameof(AddTask);

        protected readonly AddTaskViewModel Model;

        protected readonly ITaskManager Manager;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IList<ValidationError> Errors;

        public AddTask(IOrganiserFactory factory)
            : base(AddTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();

            this.Model = new AddTaskViewModel();

            this.Manager = this.Factory.Create<ITaskManager>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();

            this.DataContext = this.Model;

            this.Errors = new List<ValidationError>();
        }

        public override void InitialiseView(object arg)
        {
            this.Model.Identity = Guid.NewGuid().ToString();
            this.Model.Description = null;
            this.Model.OrganiseType = null;
        }

        private void ViewControlBase_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                this.Errors.Add(e.Error);
            }
            else
            {
                this.Errors.Remove(e.Error);
            }
        }

        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = sender as Button;

                if (button.Name == "Name_ButtonSave")
                {
                    if (this.Errors.Any())
                    {
                        this.Notifier.Notify("Cannot create new task - There are " + this.Errors.Count + " validation errors.");
                    }
                    else
                    {
                        bool immediateStart = false;

                        ITask task = this.CreateTask();
                        EventArgs<ITask, bool> args = new EventArgs<ITask, bool>(task, immediateStart);

                        this.OnViewChange(Home.ViewName, args);
                    }
                }
                else if (button.Name == "Name_ButtonDiscard")
                {
                    this.Notifier.Notify("Item discarded.");

                    this.OnViewChange(Home.ViewName);
                }
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }

        private ITask CreateTask()
        {
            TaskType taskType = TaskType.Scheduled;
            OrganiseType organiseType = OrganiseType.File;

            IOrganiseTask task = this.CreateTask(Guid.Parse(this.Model.Identity), organiseType);

            if (taskType == TaskType.Organiser)
            {
                return task;
            }

            return this.CreateScheduledTask(task, 5000, 7000);
        }

        private IOrganiseTask CreateTask(Guid identity, OrganiseType type)
        {
            // TODO: Implement Settings logic in UI
            if (type == OrganiseType.File)
            {
                FileOrganiserSettings fileSettings = new FileOrganiserSettings
                {
                    RootPath = KnownFolders.GetPath(KnownFolder.Downloads)
                };

                return this.CreateFileOrganiserTask(identity, this.Model.Description, fileSettings);
            }

            DirectoryOrganiserSettings directorySettings = new DirectoryOrganiserSettings
            {
                RootPath = KnownFolders.GetPath(KnownFolder.Downloads)
            };

            return this.CreateDirectoryOrganiserTask(identity, this.Model.Description, directorySettings);
        }

        private IOrganiseTask CreateFileOrganiserTask(Guid identity, string description, FileOrganiserSettings settings)
        {
            return new FileOrganiserTask(
                identity,
                description,
                settings, 
                this.Factory.Create<IFileExtensionProvider>(), 
                this.Factory.Create<IFileManager>(),
                this.Factory.Create<IDirectoryManager>());
        }

        private IOrganiseTask CreateDirectoryOrganiserTask(Guid identity, string description, DirectoryOrganiserSettings settings)
        {
            return new DirectoryOrganiserTask(
                identity,
                description, 
                settings, 
                this.Factory.Create<IDirectoryManager>());
        }

        private ITask CreateScheduledTask(ITask executable, int initialWaitTime, int timerPeriod)
        {
            return new ScheduledTask(
                Guid.NewGuid(),
                string.Format(ScheduledTask.DescriptionFormat, this.Model.Identity, this.Model.Description),
                this.Factory.Create<ITimer>(),
                executable,
                initialWaitTime,
                timerPeriod);
        }
    }
}