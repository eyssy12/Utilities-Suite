namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Library.Attributes;
    using Library.Communications.Suite;
    using Navigation;
    using Utilities.Library;

    [Suite(SystemSuite.Name, "System Control")]
    public class SystemSuite : DataFacilitatorSuiteBase
    {
        public const string Name = nameof(SystemSuite);

        public SystemSuite(IEnumerable<IViewControl> views, IEnumerable<IReceiveSuiteData> receivers, IEnumerable<ISendSuiteData> senders) 
            : base(SystemSuite.Name, SuiteRoute.SystemControl, views, receivers, senders)
        {
        }
    }
}