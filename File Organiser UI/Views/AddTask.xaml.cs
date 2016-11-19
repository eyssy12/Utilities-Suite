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
            this.Model.Reset();
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
            TaskType taskType = this.Model.TaskType;
            OrganiseType organiseType = this.Model.OrganiseType;

            IOrganiseTask task = this.CreateTask(Guid.Parse(this.Model.Identity), organiseType);

            if (taskType == TaskType.Organiser)
            {
                return task;
            }

            return this.CreateScheduledTask("scheduled", task, 5000, 7000);
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

                return this.CreateFileOrganiserTask(identity, this.Model.Name, this.Model.Description, fileSettings);
            }

            DirectoryOrganiserSettings directorySettings = new DirectoryOrganiserSettings
            {
                RootPath = KnownFolders.GetPath(KnownFolder.Downloads)
            };

            return this.CreateDirectoryOrganiserTask(identity, this.Model.Name, this.Model.Description, directorySettings);
        }

        private IOrganiseTask CreateFileOrganiserTask(Guid identity, string name, string description, FileOrganiserSettings settings)
        {
            return new FileOrganiserTask(
                name,
                description,
                settings, 
                this.Factory.Create<IFileExtensionProvider>(), 
                this.Factory.Create<IFileManager>(),
                this.Factory.Create<IDirectoryManager>(),
                identity: identity);
        }

        private IOrganiseTask CreateDirectoryOrganiserTask(Guid identity, string name, string description, DirectoryOrganiserSettings settings)
        {
            return new DirectoryOrganiserTask(
                name,
                description, 
                settings, 
                this.Factory.Create<IDirectoryManager>(),
                identity: identity);
        }

        private ITask CreateScheduledTask(string name, ITask executable, int initialWaitTime, int timerPeriod)
        {
            return new ScheduledTask(
                name,
                string.Format(ScheduledTask.DescriptionFormat, executable.Identity, executable.Description),
                this.Factory.Create<ITimer>(),
                executable,
                initialWaitTime: initialWaitTime,
                timerPeriod: timerPeriod);
        }

        private void Panel_FileExemptions_Drop(object sender, DragEventArgs e)
        {

        }

        private void Panel_ExtensionExemptions_Drop(object sender, DragEventArgs e)
        {

        }
    }
}