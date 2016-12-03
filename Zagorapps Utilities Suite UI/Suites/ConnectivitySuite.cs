namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Library.Attributes;
    using Navigation;

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