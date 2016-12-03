﻿namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Organiser.Library;
    using Zagorapps.Utilities.Suite.UI.Attributes;
    using Zagorapps.Utilities.Suite.UI.Navigation;

    [DefaultNavigatable]
    [Suite(DashboardSuite.Name, "Dashboard")]
    public class DashboardSuite : SuiteBase
    {
        public const string Name = nameof(DashboardSuite);

        public DashboardSuite(IEnumerable<IViewControl> views) 
            : base(DashboardSuite.Name, views)
        {
        }
    }
}