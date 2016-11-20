namespace File.Organiser.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Controls;
    using EyssyApps.Core.Library.Extensions;
    using EyssyApps.Organiser.Library.Models.Organiser;
    using EyssyApps.Organiser.Library.Tasks;
    using OrganiseTypeEnum = EyssyApps.Organiser.Library.OrganiseType;
    using TaskTypeEnum = EyssyApps.Organiser.Library.TaskType;

    public class AddTaskViewModel : ViewModelBase
    {
        private string identity, name, description, scheduledTaskIdentity, rootPath;

        private double initialWaitTime, interval;

        private OrganiseTypeEnum organiseType;
        private TaskTypeEnum taskType;

        private IList<RootPathFileViewModel> rootPathFiles;
        private IList<FileExtensionViewModel> fileExtensions;
        private IList<FileExtensionViewModel> exmptedFileExtensions;
        private IList<CategoriesViewModel> categories;

        private ICommand selectRootPathCommand;
        private ICommand loadRootPathFilesCommand;

        private bool initialized = false;

        public AddTaskViewModel()
        {
            this.Reset();
        }

        public ICommand SelectRootPathCommand
        {
            get { return this.selectRootPathCommand; }
            set { this.selectRootPathCommand = value; }
        }

        public ICommand LoadRootPathFilesCommand
        {
            get { return this.loadRootPathFilesCommand; }
            set { this.loadRootPathFilesCommand = value; }
        }

        public IEnumerable<OrganiseTypeEnum> OrganiserTypes
        {
            get { return this.GetValues<OrganiseTypeEnum>(exclusions: OrganiseTypeEnum.None); }
        }

        public IEnumerable<TaskTypeEnum> TaskTypes
        {
            get { return this.GetValues<TaskTypeEnum>(); }
        }

        public string Identity
        {
            get { return this.identity; }
            set { this.SetFieldIfChanged(ref identity, value, nameof(this.Identity)); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetFieldIfChanged(ref name, value, nameof(this.Name)); }
        }

        public string Description
        {
            get { return this.description; }
            set { this.SetFieldIfChanged(ref description, value, nameof(this.Description)); }
        }

        public string RootPath
        {
            get { return this.rootPath; }
            set
            {
                this.SetFieldIfChanged(ref rootPath, value, nameof(this.RootPath));

                if (this.initialized)
                {
                    this.LoadRootPathFilesCommand.Execute(null);
                }
            }
        }

        public OrganiseTypeEnum OrganiseType
        {
            get { return this.organiseType; }
            set { this.SetFieldIfChanged(ref organiseType, value, nameof(this.OrganiseType)); }
        }

        public TaskTypeEnum TaskType
        {
            get { return this.taskType; }
            set { this.SetFieldIfChanged(ref taskType, value, nameof(this.TaskType)); }
        }

        public string ScheduledTaskIdentity
        {
            get { return this.scheduledTaskIdentity; }
            set { this.SetFieldIfChanged(ref scheduledTaskIdentity, value, nameof(this.ScheduledTaskIdentity)); }
        }

        public double InitialWaitTime
        {
            get { return this.initialWaitTime; }
            set { this.SetFieldIfChanged(ref initialWaitTime, value, nameof(this.InitialWaitTime)); }
        }

        public double Interval
        {
            get { return this.interval; }
            set { this.SetFieldIfChanged(ref interval, value, nameof(this.Interval)); }
        }

        public IList<RootPathFileViewModel> RootPathFiles
        {
            get { return this.rootPathFiles; }
            set { this.SetFieldIfChanged(ref rootPathFiles, value, nameof(this.RootPathFiles)); }
        }

        public IList<FileExtensionViewModel> FileExtensions
        {
            get { return this.fileExtensions; }
            set { this.SetFieldIfChanged(ref fileExtensions, value, nameof(this.FileExtensions)); }
        }

        public IList<FileExtensionViewModel> ExemptedFileExtensions
        {
            get { return this.exmptedFileExtensions; }
            set { this.SetFieldIfChanged(ref exmptedFileExtensions, value, nameof(this.ExemptedFileExtensions)); }
        }

        public IList<CategoriesViewModel> Categories
        {
            get { return this.categories; }
            set { this.SetFieldIfChanged(ref categories, value, nameof(this.Categories)); }
        }

        public void Reset()
        {
            this.Identity = Guid.NewGuid().ToString();
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.RootPath = string.Empty;
            this.OrganiseType = OrganiseTypeEnum.File;
            this.TaskType = TaskTypeEnum.Organiser;
            this.RootPathFiles = new List<RootPathFileViewModel>();
            this.ExemptedFileExtensions = new List<FileExtensionViewModel>();
            this.InitialWaitTime = new TimeSpan(0, 0, 0, 0, (ScheduledTask.MinimumInitialWaitTime)).TotalSeconds;
            this.Interval = new TimeSpan(0, 0, 0, 0, (ScheduledTask.MinimumTimerPeriod)).TotalSeconds;

            this.initialized = true;
        }
    }
}