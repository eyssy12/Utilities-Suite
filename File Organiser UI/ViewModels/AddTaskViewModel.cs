namespace File.Organiser.UI.ViewModels
{
    using System;
    using Controls;

    public class AddTaskViewModel : ViewModelBase
    {
        private string identity, description, organiseType, taskType, scheduledTaskIdentity;

        public AddTaskViewModel()
        {
            this.identity = Guid.NewGuid().ToString();
            this.description = string.Empty;
        }

        public string Identity
        {
            get { return this.identity; }
            set { this.SetFieldIfChanged(ref identity, value, nameof(this.Identity)); }
        }

        public string Description
        {
            get { return this.description; }
            set { this.SetFieldIfChanged(ref description, value, nameof(this.Description)); }
        }

        public string OrganiseType
        {
            get { return this.organiseType; }
            set { this.SetFieldIfChanged(ref organiseType, value, nameof(this.OrganiseType)); }
        }

        public string TaskType
        {
            get { return this.taskType; }
            set { this.SetFieldIfChanged(ref taskType, value, nameof(this.TaskType)); }
        }

        public string ScheduledTaskIdentity
        {
            get { return this.scheduledTaskIdentity; }
            set { this.SetFieldIfChanged(ref scheduledTaskIdentity, value, nameof(this.ScheduledTaskIdentity)); }
        }
    }
}