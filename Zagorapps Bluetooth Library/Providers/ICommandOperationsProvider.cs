namespace Zagorapps.Bluetooth.Library.Providers
{
    using Commands;
    using Networking;

    public interface ICommandOperationsProvider
    {
        IBasicCommandOperation CreateBasicOperation(INetworkWriter writer);
    }
}