namespace Zagorapps.Utilities.Suite.UI.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Extensions;
    using Zagorapps.Utilities.Suite.UI.Suites;

    public class DataFacilitatorSuiteManager : SuiteManager, IDataFacilitatorSuiteManager
    {
        protected readonly IEnumerable<IDataFacilitatorSuite> dataSuites;

        public DataFacilitatorSuiteManager(IEnumerable<ISuite> suites)
            : base(suites)
        {
            this.dataSuites = this.Navigatables.OfType<IDataFacilitatorSuite>();
        }

        public bool Start()
        {
            this.dataSuites.ForEach(d => d.Start());

            return true;
        }

        public bool Stop()
        {
            this.dataSuites.ForEach(d => d.Stop());

            return true;
        }
    }
}