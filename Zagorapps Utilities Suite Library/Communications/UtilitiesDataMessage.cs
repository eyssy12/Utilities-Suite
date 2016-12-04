namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class UtilitiesDataMessage : IUtilitiesDataMessage
    {
        protected const string KeyCreatedTime = "createdtime",
            KeySuiteDestination = "suitedestination",
            KeyViewDestination = "viewdestination",
            KeyData = "data";

        private readonly DateTime createdTime;
        private readonly SuiteRoute suiteDestination;
        private readonly string viewDestination;
        private readonly object data;

        public UtilitiesDataMessage(DateTime createdTime, SuiteRoute suiteDestination, string viewDestination, object data)
        {
            this.createdTime = createdTime;
            this.suiteDestination = suiteDestination;
            this.viewDestination = viewDestination;
            this.data = data;
        }

        protected UtilitiesDataMessage(SerializationInfo info, StreamingContext context)
        {
            this.createdTime = info.GetDateTime(UtilitiesDataMessage.KeyCreatedTime);
            this.suiteDestination = (SuiteRoute)info.GetValue(UtilitiesDataMessage.KeySuiteDestination, typeof(SuiteRoute));
            this.viewDestination = info.GetString(UtilitiesDataMessage.KeyViewDestination);
            this.data = info.GetValue(UtilitiesDataMessage.KeyData, typeof(object));
        }

        public DateTime CreatedTime
        {
            get { return this.createdTime; }
        }

        public object Data
        {
            get { return this.data; }
        }

        public SuiteRoute SuiteDestination
        {
            get { return this.suiteDestination; }
        }

        public string ViewDestination
        {
            get { return this.viewDestination; }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(UtilitiesDataMessage.KeyCreatedTime, this.createdTime);
            info.AddValue(UtilitiesDataMessage.KeySuiteDestination, this.suiteDestination);
            info.AddValue(UtilitiesDataMessage.KeyViewDestination, this.viewDestination);
            info.AddValue(UtilitiesDataMessage.KeyData, this.data);
        }
    }
}