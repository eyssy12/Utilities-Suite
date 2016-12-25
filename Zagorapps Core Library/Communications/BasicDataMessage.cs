namespace Zagorapps.Core.Library.Communications
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class BasicDataMessage : IDataMessage
    {
        protected const string FromKey = "from",
            DataKey = "data",
            CreateTimeKey = "createtime";

        private string from;
        private object data;
        private DateTime createdTime;

        public BasicDataMessage(string from, object data)
        {
            this.from = from;
            this.data = data;

            this.createdTime = DateTime.UtcNow;
        }

        protected BasicDataMessage(SerializationInfo info, StreamingContext context)
        {
            this.from = info.GetString(BasicDataMessage.FromKey);
            this.data = info.GetValue(BasicDataMessage.DataKey, typeof(object));
            this.createdTime = info.GetDateTime(BasicDataMessage.CreateTimeKey);
        }

        public DateTime CreatedTime
        {
            get { return this.createdTime; }
        }

        public object Data
        {
            get { return this.data; }
        }

        public string From
        {
            get { return this.from; }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(BasicDataMessage.CreateTimeKey, this.createdTime);
            info.AddValue(BasicDataMessage.DataKey, this.data);
            info.AddValue(BasicDataMessage.FromKey, this.from);
        }
    }
}