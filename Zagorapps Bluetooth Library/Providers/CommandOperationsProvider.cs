namespace Zagorapps.Bluetooth.Library.Providers
{
    using Commands;
    using Networking;

    public class CommandOperationsProvider : ICommandOperationsProvider
    {
        public IBasicCommandOperation CreateBasicOperation(INetworkWriter writer)
        {
            return new BasicCommandOperation(writer);
        }
    }
}