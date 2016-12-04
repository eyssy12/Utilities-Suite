namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using Core.Library.Communications;

    public interface IUtilitiesDataMessage : IDataMessage
    {
        SuiteRoute SuiteDestination { get; }

        string ViewDestination { get; }

        object Data { get; }
    }
}