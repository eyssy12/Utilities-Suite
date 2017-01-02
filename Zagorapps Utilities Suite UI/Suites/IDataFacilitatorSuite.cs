namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using Library.Communications;
    using Library.Communications.Suite;
    using Utilities.Suite.Library;

    public interface IDataFacilitatorSuite : ISuite, IReceiveSuiteData, ISendData
    {
        SuiteRoute Route { get; }
    }
}