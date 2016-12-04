namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Navigation;

    [Suite(SystemSuite.Name, "System Control")]
    public class SystemSuite : DataFacilitatorSuiteBase
    {
        public const string Name = nameof(SystemSuite);

        public SystemSuite(IEnumerable<IViewControl> views, IEnumerable<IReceiveSuiteData> receivers, IEnumerable<ISendSuiteData> senders) 
            : base(SystemSuite.Name, SuiteRoute.System, views, receivers, senders)
        {
        }
    }
}