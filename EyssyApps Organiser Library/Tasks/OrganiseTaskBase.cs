namespace EyssyApps.Organiser.Library.Tasks
{
    using System;

    public abstract class OrganiseTaskBase : TaskBase, IOrganiseTask
    {
        protected const string DefaultUnkownName = "Unknown";

        private readonly OrganiseType organiseType;

        protected OrganiseTaskBase(Guid? identity, string name, string description, OrganiseType organiseType, TaskType taskType) 
            : base(identity, name, description, taskType)
        {
            this.organiseType = organiseType;
        }

        public OrganiseType OrganiseType
        {
            get { return this.organiseType; }
        }
    }
}