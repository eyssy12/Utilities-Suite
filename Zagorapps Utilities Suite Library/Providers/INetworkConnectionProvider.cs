namespace Zagorapps.Utilities.Suite.Library.Providers
{
    using Core.Library.Communications;
    using Core.Library.Construction;

    public interface INetworkConnectionProvider
    {
        INetworkConnection CreateNetworkConnection(ConnectionType type, IContext context);
    }
}