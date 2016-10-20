namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class OrganiseTaskBase : IOrganiseTask
    {
        protected const string KeyId = "id",
            KeyType = "type";

        private readonly Guid id;
        private readonly OrganiseType type;

        protected OrganiseTaskBase(Guid id, OrganiseType type)
        {
            this.id = id;
            this.type = type;
        }

        public Guid Id
        {
            get { return this.id; }
        }

        public OrganiseType Type
        {
            get { return this.type; }
        }

        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        public abstract void Execute();

        public abstract void Terminate();
    }
}