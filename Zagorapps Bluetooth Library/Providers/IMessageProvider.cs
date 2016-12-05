namespace Zagorapps.Bluetooth.Library.Providers
{
    using Messaging;

    public interface IMessageProvider
    {
        IBasicStringMessage CreateBasicStringMessage(string contents);
    }
}