namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Models.Settings;

    public abstract class OrganiseTaskBase : TaskBase, IOrganiseTask
    {
        protected const string DefaultUnkownName = "Unknown";

        private readonly OrganiseType organiseType;
        private FileOrganiserSettings settings;

        protected OrganiseTaskBase(Guid id, string description, FileOrganiserSettings settings, OrganiseType organiseType, TaskType taskType) 
            : base(id, description, taskType)
        {
            this.settings = settings;
            this.organiseType = organiseType;
        }

        public FileOrganiserSettings Settings
        {
            get { return this.settings; }
        }

        public OrganiseType OrganiseType
        {
            get { return this.organiseType; }
        }
    }
}