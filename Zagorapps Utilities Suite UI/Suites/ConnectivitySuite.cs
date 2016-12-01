namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Navigation;
    using Zagorapps.Utilities.Suite.UI.Attributes;

    [Suite(ConnectivitySuite.Name, "Connectivity")]
    public class ConnectivitySuite : SuiteBase
    {
        public const string Name = nameof(ConnectivitySuite);

        public ConnectivitySuite(IEnumerable<IViewControl> views) 
            : base(ConnectivitySuite.Name, views)
        {
        }
    }
}