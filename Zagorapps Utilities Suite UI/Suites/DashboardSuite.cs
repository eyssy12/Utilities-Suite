namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Zagorapps.Utilities.Suite.UI.Navigation;

    [DefaultNavigatable]
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