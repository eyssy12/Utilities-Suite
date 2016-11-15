namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using Models.Settings;

    public abstract class OrganiseTaskBase : TaskBase, IOrganiseTask
    {
        protected const string DefaultUnkownName = "Unknown";

        private readonly OrganiseType organiseType;

        protected OrganiseTaskBase(Guid id, string description, OrganiseType organiseType, TaskType taskType) 
            : base(id, description, taskType)
        {
            this.organiseType = organiseType;
        }

        public OrganiseType OrganiseType
        {
            get { return this.organiseType; }
        }
    }
}