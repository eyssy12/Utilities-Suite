namespace Zagorapps.Bluetooth.Library.Providers
{
    using System;
    using Commands;
    using Data;
    using Networking;

    public class CommandOperationsProvider : ICommandOperationsProvider
    {
        public IBasicCommandOperation CreateBasicOperation(INetworkWriter writer)
        {
            return new BasicCommandOperation(writer);
        }

        public ICommandOperation CreateOperation(Action action)
        {
            throw new NotImplementedException();
        }
    }
}