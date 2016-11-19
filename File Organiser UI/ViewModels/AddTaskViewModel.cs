﻿namespace File.Organiser.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Controls;
    using EyssyApps.Core.Library.Extensions;
    using EyssyApps.Organiser.Library.Models.Settings;
    using EyssyApps.Organiser.Library.Tasks;
    using OrganiseTypeEnum = EyssyApps.Organiser.Library.OrganiseType;
    using TaskTypeEnum = EyssyApps.Organiser.Library.TaskType;

    public class AddTaskViewModel : ViewModelBase
    {
        private string identity, name, description, scheduledTaskIdentity;

        private double initialWaitTime, interval;

        private OrganiseTypeEnum organiseType;
        private TaskTypeEnum taskType;

        private FileOrganiserSettings FileOrganiserSettings;
        private DirectoryOrganiserSettings DirectoryOrganiserSettings;

        public AddTaskViewModel()
        {
            this.Reset();
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
            this.OrganiseType = OrganiseTypeEnum.File;
            this.TaskType = TaskTypeEnum.Organiser;
            this.InitialWaitTime = new TimeSpan(0, 0, 0, 0, (ScheduledTask.MinimumInitialWaitTime)).TotalSeconds + 1;
            this.Interval = new TimeSpan(0, 0, 0, 0, (ScheduledTask.MinimumTimerPeriod)).TotalSeconds + 1;

            this.FileOrganiserSettings = new FileOrganiserSettings();
            this.DirectoryOrganiserSettings = new DirectoryOrganiserSettings();
        }
    }
}