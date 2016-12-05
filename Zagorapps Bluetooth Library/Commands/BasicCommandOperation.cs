namespace Zagorapps.Bluetooth.Library.Commands
{
    using System;
    using Messaging;
    using Networking;
    using Core.Library.Events;

    public class BasicCommandOperation : IBasicCommandOperation
    {
        protected readonly INetworkWriter Writer;

        public BasicCommandOperation(INetworkWriter writer)
        {
            // TODO: guard conditions

            this.Writer = writer;
        }

        public event EventHandler<EventArgs<DateTime>> OperationStarted;

        public bool Invoke(IMessage argument)
        {
            try
            {
                this.Writer.Write(argument.Prepare());
                this.Writer.Flush();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}