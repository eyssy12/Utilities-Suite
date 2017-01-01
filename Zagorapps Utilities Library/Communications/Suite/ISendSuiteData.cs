namespace Zagorapps.Utilities.Library.Communications.Suite
{
    using Utilities.Library;
    using Utilities.Library.Communications;

    public interface ISendSuiteData : ISendData
    {
        SuiteRoute Route { get; }
    }
}