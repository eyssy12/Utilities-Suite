namespace Zagorapps.Utilities.Suite.Library.Facilitators
{
    using System.Collections.Generic;
    using System.Linq;
    using Utilities.Library.Communications;

    public class DataProcesingFacilitator : IFacilitateDataProcessing
    {
        private readonly IEnumerable<IReceiveData> receivers;
        private readonly IEnumerable<ISendData> senders;

        public DataProcesingFacilitator(IEnumerable<IReceiveData> receivers = null, IEnumerable<ISendData> senders = null)
        {
            this.receivers = receivers ?? Enumerable.Empty<IReceiveData>();
            this.senders = senders ?? Enumerable.Empty<ISendData>();
        }

        public IEnumerable<IReceiveData> Receivers
        {
            get { return this.receivers; }
        }

        public IEnumerable<ISendData> Senders
        {
            get { return this.senders; }
        }
    }
}