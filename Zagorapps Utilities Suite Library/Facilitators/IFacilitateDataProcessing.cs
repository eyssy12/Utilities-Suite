namespace Zagorapps.Utilities.Suite.Library.Facilitators
{
    using System.Collections.Generic;
    using Utilities.Library.Communications;

    public interface IFacilitateDataProcessing
    {
        IEnumerable<IReceiveData> Receivers { get; }

        IEnumerable<ISendData> Senders { get; }
    }
}