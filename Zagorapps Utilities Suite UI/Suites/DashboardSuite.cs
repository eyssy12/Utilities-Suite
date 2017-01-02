namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Library.Attributes;
    using Library.Communications.Suite;
    using Utilities.Suite.Library;
    using Zagorapps.Utilities.Suite.UI.Navigation;

    [DefaultNavigatable(DashboardSuite.Name)] // TODO: shouldn't need to have the id passed in to two different things - try to reuse
    [Suite(DashboardSuite.Name, "Dashboard")]
    public class DashboardSuite : DataFacilitatorSuiteBase
    {
        public const string Name = nameof(DashboardSuite);

        public DashboardSuite(IEnumerable<IViewControl> views, IEnumerable<IReceiveSuiteData> receivers, IEnumerable<ISendSuiteData> senders) 
            : base(DashboardSuite.Name, SuiteRoute.Dashboard, views, receivers, senders)
        {
        }
    }
}