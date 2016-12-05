namespace Zagorapps.Bluetooth.Library.Providers
{
    using System;
    using Commands;
    using Data;
    using Networking;

    public interface ICommandOperationsProvider
    {
        ICommandOperation CreateOperation(Action action);

        IBasicCommandOperation CreateBasicOperation(INetworkWriter writer);
    }
}