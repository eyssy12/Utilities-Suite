namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using Utilities.Library.Communications;

    public interface ISendSuiteData : ISendData
    {
        SuiteRoute Route { get; }
    }
}