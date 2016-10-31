namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class OrganiseTaskBase : IOrganiseTask
    {
        protected const string DefaultUnkownName = "Unknown",
            KeyId = "id",
            KeyType = "type";

        private readonly Guid id;
        private readonly OrganiseType type;
        private readonly string description;

        protected OrganiseTaskBase(Guid id, OrganiseType type, string description)
        {
            this.id = id;
            this.type = type;
            this.description = description;
        }

        public Guid Id
        {
            get { return this.id; }
        }

        public OrganiseType Type
        {
            get { return this.type; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        public abstract void Execute();

        public abstract void Terminate();
    }
}