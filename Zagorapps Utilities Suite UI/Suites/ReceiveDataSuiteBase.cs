namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Navigation;

    public abstract class ReceiveDataSuiteBase : SuiteBase
    {
        protected readonly IEnumerable<object> InboundChannels;
        protected readonly IEnumerable<object> OutboundChannels;

        protected ReceiveDataSuiteBase(string suiteName, IEnumerable<IViewControl> views, IEnumerable<object> inboundChannels, IEnumerable<object> outboundChannels) 
            : base(suiteName, views)
        {
        }
    }
}