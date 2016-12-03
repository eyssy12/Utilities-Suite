namespace Zagorapps.Utilities.Library.Communications
{
    using Core.Library.Communications;

    public interface ISendData
    {
        void Send(IDataMessage data);
    }
}