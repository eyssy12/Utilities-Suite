namespace Zagorapps.Utilities.Suite.UI.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Extensions;
    using Zagorapps.Utilities.Suite.UI.Suites;

    public class DataFacilitatorSuiteManager : SuiteManager, IDataFacilitatorSuiteManager
    {
        protected readonly IEnumerable<IDataFacilitatorSuite> DataSuites;

        public DataFacilitatorSuiteManager(IEnumerable<ISuite> suites)
            : base(suites)
        {
            this.DataSuites = this.Navigatables.OfType<IDataFacilitatorSuite>();
        }

        public bool Start()
        {
            this.DataSuites.ForEach(d => d.Start());

            return true;
        }

        public bool Stop()
        {
            this.DataSuites.ForEach(d => d.Stop());

            return true;
        }
    }
}