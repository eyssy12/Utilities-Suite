namespace Zagorapps.Utilities.Suite.Library.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Library.Communications;
    using Core.Library.Events;

    public class TcpNetworkConnection : INetworkConnection
    {
        public event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Send(IDataMessage message)
        {
            throw new NotImplementedException();
        }
    }
}