namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Models.Settings;

    public abstract class OrganiseTaskBase : TaskBase, IOrganiseTask
    {
        protected const string DefaultUnkownName = "Unknown";

        private readonly OrganiseType type;
        private FileOrganiserSettings settings;

        protected OrganiseTaskBase(Guid id, string description, FileOrganiserSettings settings, OrganiseType type) 
            : base(id, description)
        {
            this.settings = settings;
            this.type = type;
        }

        public FileOrganiserSettings Settings
        {
            get { return this.settings; }
        }

        public OrganiseType Type
        {
            get { return this.type; }
        }
    }
}