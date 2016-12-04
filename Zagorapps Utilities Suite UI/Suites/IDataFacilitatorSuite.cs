namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using Library;
    using Library.Communications;
    using Utilities.Library.Communications;

    public interface IDataFacilitatorSuite : ISuite, IReceiveSuiteData, ISendData
    {
        SuiteRoute Route { get; }
    }
}