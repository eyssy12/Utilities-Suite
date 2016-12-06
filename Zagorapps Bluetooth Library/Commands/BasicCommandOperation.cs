namespace Zagorapps.Bluetooth.Library.Commands
{
    using System;
    using Core.Library.Events;
    using Messaging;
    using Networking;

    public class BasicCommandOperation : IBasicCommandOperation
    {
        protected readonly INetworkWriter Writer;

        public BasicCommandOperation(INetworkWriter writer)
        {
            if (writer == null)
            {
                // TODO: resources
                throw new ArgumentNullException(nameof(writer), "No network writer proviuded");
            }

            this.Writer = writer;
        }

        public event EventHandler<EventArgs<DateTime>> OperationStarted;

        public bool Invoke(IMessage argument)
        {
            try
            {
                Invoker.Raise(ref this.OperationStarted, this, DateTime.UtcNow);

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