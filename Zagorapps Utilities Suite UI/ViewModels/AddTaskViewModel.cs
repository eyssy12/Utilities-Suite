namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Controls;
    using Zagorapps.Core.Library.Extensions;
    using Zagorapps.Organiser.Library.Tasks;
    using OrganiseTypeEnum = Zagorapps.Organiser.Library.OrganiseType;
    using TaskTypeEnum = Zagorapps.Organiser.Library.TaskType;

    public class AddTaskViewModel : ViewModelBase
    {
        private string identity, name, description, scheduledTaskIdentity, rootPath;

        private double initialWaitTime, interval;

        private OrganiseTypeEnum organiseType;
        private TaskTypeEnum taskType;

        private IList<RootPathFileViewModel> fileRootPathFiles, directoryRootPathFiles;
        private IList<FileExtensionViewModel> fileExtensions;
        private IList<FileExtensionViewModel> exmptedFileExtensions;
        private IList<CategoriesViewModel> categories;

        private ICommand selectRootPathCommand;
        private ICommand loadRootPathFilesCommand;
        private ICommand loadDirectoryRootPathFilesCommand;

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

        public ICommand LoadDirectoryRootPathFilesCommand
        {
            get { return this.loadDirectoryRootPathFilesCommand; }
            set { this.loadDirectoryRootPathFilesCommand = value; }
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
            set { this.SetField(ref this.identity, value, nameof(this.Identity)); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetField(ref this.name, value, nameof(this.Name)); }
        }

        public string Description
        {
            get { return this.description; }
            set { this.SetField(ref this.description, value, nameof(this.Description)); }
        }

        public string RootPath
        {
            get
            {
                return this.rootPath;
            }
            set
            {
                this.SetField(ref rootPath, value, nameof(this.RootPath));

                if (this.initialized)
                {
                    if (this.OrganiseType == OrganiseTypeEnum.File)
                    {
                        Task.Run(() => this.LoadRootPathFilesCommand.Execute(null));
                    }
                    else if (this.OrganiseType == OrganiseTypeEnum.Directory)
                    {
                        Task.Run(() => this.LoadDirectoryRootPathFilesCommand.Execute(null));
                    }
                    else if (this.OrganiseType == OrganiseTypeEnum.All)
                    {
                        Task.Run(() => this.LoadRootPathFilesCommand.Execute(null));
                        Task.Run(() => this.LoadDirectoryRootPathFilesCommand.Execute(null));
                    }
                }
            }
        }

        public OrganiseTypeEnum OrganiseType
        {
            get { return this.organiseType; }
            set { this.SetField(ref this.organiseType, value, nameof(this.OrganiseType)); }
        }

        public TaskTypeEnum TaskType
        {
            get { return this.taskType; }
            set { this.SetField(ref this.taskType, value, nameof(this.TaskType)); }
        }

        public string ScheduledTaskIdentity
        {
            get { return this.scheduledTaskIdentity; }
            set { this.SetField(ref this.scheduledTaskIdentity, value, nameof(this.ScheduledTaskIdentity)); }
        }

        public double InitialWaitTime
        {
            get { return this.initialWaitTime; }
            set { this.SetField(ref this.initialWaitTime, value, nameof(this.InitialWaitTime)); }
        }

        public double Interval
        {
            get { return this.interval; }
            set { this.SetField(ref this.interval, value, nameof(this.Interval)); }
        }

        public IList<RootPathFileViewModel> FileRootPathFiles
        {
            get { return this.fileRootPathFiles; }
            set { this.SetField(ref this.fileRootPathFiles, value, nameof(this.FileRootPathFiles)); }
        }

        public IList<RootPathFileViewModel> DirectoryRootPathFiles
        {
            get { return this.directoryRootPathFiles; }
            set { this.SetField(ref this.directoryRootPathFiles, value, nameof(this.DirectoryRootPathFiles)); }
        }

        public IList<FileExtensionViewModel> FileExtensions
        {
            get { return this.fileExtensions; }
            set { this.SetField(ref this.fileExtensions, value, nameof(this.FileExtensions)); }
        }

        public IList<FileExtensionViewModel> ExemptedFileExtensions
        {
            get { return this.exmptedFileExtensions; }
            set { this.SetField(ref this.exmptedFileExtensions, value, nameof(this.ExemptedFileExtensions)); }
        }

        public IList<CategoriesViewModel> Categories
        {
            get { return this.categories; }
            set { this.SetField(ref this.categories, value, nameof(this.Categories)); }
        }

        public void Reset()
        {
            this.Identity = Guid.NewGuid().ToString();
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.RootPath = string.Empty;
            this.OrganiseType = OrganiseTypeEnum.File;
            this.TaskType = TaskTypeEnum.Organiser;
            this.FileRootPathFiles = new List<RootPathFileViewModel>();
            this.DirectoryRootPathFiles = new List<RootPathFileViewModel>();
            this.ExemptedFileExtensions = new List<FileExtensionViewModel>();
            this.InitialWaitTime = new TimeSpan(0, 0, 0, 0, ScheduledTask.MinimumInitialWaitTime).TotalSeconds;
            this.Interval = new TimeSpan(0, 0, 0, 0, ScheduledTask.MinimumTimerPeriod).TotalSeconds;

            this.initialized = true;
        }
    }
}