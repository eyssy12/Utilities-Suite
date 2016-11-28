namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Attributes;
    using Controls;

    [Suite(TempSuite.Name)]
    public class TempSuite : SuiteBase
    {
        public const string Name = nameof(TempSuite);

        public TempSuite(IEnumerable<IViewControl> views) 
            : base(TempSuite.Name, views)
        {
        }
    }
}