namespace Zagorapps.Utilities.Library.Communications
{
    using Core.Library.Communications;
    using Utilities.Library;

    public interface IUtilitiesDataMessage : IDataMessage
    {
        SuiteRoute SuiteDestination { get; }

        string ViewDestination { get; }
    }
}