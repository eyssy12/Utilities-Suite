namespace Zagorapps.Utilities.Library.Communications
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using Core.Library.Communications;
    using Utilities.Library;

    [Serializable]
    public class UtilitiesDataMessage : BasicDataMessage, IUtilitiesDataMessage
    {
        protected const string KeySuiteDestination = "suitedestination",
            KeyViewDestination = "viewdestination";

        private readonly SuiteRoute suiteDestination;
        private readonly string viewDestination;

        public UtilitiesDataMessage(string from, SuiteRoute suiteDestination, string viewDestination, object data)
            : base(from, data)
        {
            this.suiteDestination = suiteDestination;
            this.viewDestination = viewDestination;
        }

        protected UtilitiesDataMessage(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.suiteDestination = (SuiteRoute)info.GetValue(UtilitiesDataMessage.KeySuiteDestination, typeof(SuiteRoute));
            this.viewDestination = info.GetString(UtilitiesDataMessage.KeyViewDestination);
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
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(UtilitiesDataMessage.KeySuiteDestination, this.suiteDestination);
            info.AddValue(UtilitiesDataMessage.KeyViewDestination, this.viewDestination);
        }
    }
}