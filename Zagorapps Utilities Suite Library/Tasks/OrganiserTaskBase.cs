namespace Zagorapps.Utilities.Suite.Library.Tasks
{
    using System;

    public abstract class OrganiserTaskBase : TaskBase, IOrganiserTask
    {
        protected const string DefaultUnkownName = "Unknown";

        private readonly OrganiseType organiseType;

        protected OrganiserTaskBase(Guid? identity, string name, string description, OrganiseType organiseType, TaskType taskType) 
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