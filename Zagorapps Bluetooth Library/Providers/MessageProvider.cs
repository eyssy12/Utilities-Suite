namespace Zagorapps.Bluetooth.Library.Providers
{
    using Messaging;

    public class MessageProvider : IMessageProvider
    {
        public MessageProvider()
        {
        }

        public IBasicStringMessage CreateBasicStringMessage(string contents)
        {
            return new BasicStringMessage(contents);
        }
    }
}