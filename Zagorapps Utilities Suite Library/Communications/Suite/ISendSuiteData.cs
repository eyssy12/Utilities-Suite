namespace Zagorapps.Utilities.Suite.Library.Communications.Suite
{
    public interface ISendSuiteData : ISendData
    {
        SuiteRoute Route { get; }
    }
}