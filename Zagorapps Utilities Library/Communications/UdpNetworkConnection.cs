namespace Zagorapps.Utilities.Library.Communications
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Library.Communications;
    using Core.Library.Events;

    public class UdpNetworkConnection : INetworkConnection
    {
        protected readonly int EndpointPort, PacketSize;
        protected readonly IPEndPoint Endpoint;

        private UdpClient server;
        private bool connected;

        public UdpNetworkConnection(int endpointPort, int packetSize = 1024)
        {
            this.EndpointPort = endpointPort;
            this.PacketSize = packetSize;
            this.Endpoint = new IPEndPoint(IPAddress.Any, this.EndpointPort);

            this.connected = false;
        }

        public event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        public void Close()
        {
            if (this.connected)
            {
                this.server.Close();
                this.server.Dispose();

                this.connected = false;
            }
        }

        public void Start()
        {
            if (!this.connected)
            {
                this.server = new UdpClient(this.Endpoint);

                this.connected = true;

                Task.Run(() =>
                {
                    try
                    {
                        IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
                        byte[] data = new byte[this.PacketSize];

                        while (this.connected)
                        {
                            data = server.Receive(ref client);

                            Invoker.Raise(ref this.MessageReceived, this, new BasicDataMessage(client.Address.ToString(), Encoding.UTF8.GetString(data, 0, data.Length).TrimEnd()));
                        }
                    }
                    catch
                    {
                        // stream closed
                    }
                });
            }
        }

        public void Send(IDataMessage message)
        {
            if (this.connected)
            {
                try
                {
                    this.server.SendAsync(Encoding.UTF8.GetBytes(message.Data.ToString()), this.PacketSize);
                }
                catch
                {

                }   
            }
        }
    }
}