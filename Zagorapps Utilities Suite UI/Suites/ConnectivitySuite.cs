namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Navigation;

    [Suite(ConnectivitySuite.Name, "Connectivity")]
    public class ConnectivitySuite : DataFacilitatorSuiteBase
    {
        public const string Name = nameof(ConnectivitySuite);

        public ConnectivitySuite(IEnumerable<IViewControl> views, IEnumerable<IReceiveSuiteData> receivers, IEnumerable<ISendSuiteData> senders) 
            : base(ConnectivitySuite.Name, SuiteRoute.Connectivity, views, receivers, senders)
        {
        }
    }
}