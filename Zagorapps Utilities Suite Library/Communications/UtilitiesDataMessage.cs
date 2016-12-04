namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class UtilitiesDataMessage : IUtilitiesDataMessage
    {
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}