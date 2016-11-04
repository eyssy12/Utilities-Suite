namespace EyssyApps.Organiser.Library.Models
{
    using System;
    using Settings;

    [Serializable]
    public class TaskMetadata
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public FileOrganiserSettings Settings { get; set; }

        public TaskState State { get; set; }

        public OrganiseType Type { get; set; }
    }
}