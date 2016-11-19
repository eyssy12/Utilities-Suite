namespace File.Organiser.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Controls;
    using EyssyApps.Core.Library.Extensions;
    using EyssyApps.Organiser.Library.Models.Settings;
    using EyssyApps.Organiser.Library.Tasks;
    using OrganiseTypeEnum = EyssyApps.Organiser.Library.OrganiseType;
    using TaskTypeEnum = EyssyApps.Organiser.Library.TaskType;

    public class AddTaskViewModel : ViewModelBase
    {
        private string identity, name, description, scheduledTaskIdentity, rootPath;

        private double initialWaitTime, interval;

        private OrganiseTypeEnum organiseType;
        private TaskTypeEnum taskType;

        private ICommand selectRootPathCommand;

        private FileOrganiserSettings FileOrganiserSettings;
        private DirectoryOrganiserSettings DirectoryOrganiserSettings;

        public AddTaskViewModel()
        {
            this.Reset();
        }

        public ICommand SelectRootPathCommand
        {
            get { return this.selectRootPathCommand; }
            set { this.selectRootPathCommand = value; }
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

                if (this.FileSettings != null)
                {
                    this.FileSettings.RootPath = this.rootPath;
                }

                if (this.DirectorySettings != null)
                {
                    this.DirectorySettings.RootPath = this.rootPath;
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

        public FileOrganiserSettings FileSettings
        {
            get { return this.FileOrganiserSettings; }
        }

        public DirectoryOrganiserSettings DirectorySettings
        {
            get { return this.DirectoryOrganiserSettings; }
        }

        public IEnumerable<string> FileExemptions
        {
            get { return this.FileOrganiserSettings.FileExemptions; }
            set
            {
                this.FileOrganiserSettings.FileExemptions = value;
                this.OnPropertyChanged(nameof(FileExemptions));
            }
        }

        public void Reset()
        {
            this.Identity = Guid.NewGuid().ToString();
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.RootPath = string.Empty;
            this.OrganiseType = OrganiseTypeEnum.File;
            this.TaskType = TaskTypeEnum.Organiser;
            this.InitialWaitTime = new TimeSpan(0, 0, 0, 0, (ScheduledTask.MinimumInitialWaitTime)).TotalSeconds;
            this.Interval = new TimeSpan(0, 0, 0, 0, (ScheduledTask.MinimumTimerPeriod)).TotalSeconds;

            this.FileOrganiserSettings = new FileOrganiserSettings();
            this.DirectoryOrganiserSettings = new DirectoryOrganiserSettings();
        }
    }
}