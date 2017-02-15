namespace Zagorapps.Utilities.Suite.Library.Communications.Server
{
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Library.Communications;
    using Core.Library.Events;
    using Core.Library.Extensions;

    public class LocalUdpServer : ILocalNetworkServer
    {
        protected readonly ConcurrentDictionary<string, IPEndPoint> KnownClients;

        protected readonly int EndpointPort, PacketSize;
        protected readonly IPEndPoint Endpoint;

        private UdpClient server; // abstraction ?
        private bool isConnected;

        public LocalUdpServer(int endpointPort, int packetSize = 1024)
        {
            this.EndpointPort = endpointPort;
            this.PacketSize = packetSize;
            this.Endpoint = new IPEndPoint(IPAddress.Any, this.EndpointPort);

            this.KnownClients = new ConcurrentDictionary<string, IPEndPoint>();

            this.isConnected = false;
        }

        public event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        public event EventHandler<EventArgs<IDataMessage>> MessageSent;

        public event EventHandler<EventArgs<ConnectionType, string>> ClientConnected;

        public event EventHandler<EventArgs<ConnectionType, string>> ClientDisconnected; // will not be called, UDP originally has no concept of connected/disconnected clients

        public bool Start()
        {
            if (!this.isConnected)
            {
                this.server = new UdpClient(this.Endpoint);

                this.isConnected = true;

                Task.Run(async () =>
                {
                    try
                    {
                        while (this.isConnected)
                        {
                            UdpReceiveResult result = await server.ReceiveAsync();

                            string clientAddress = result.RemoteEndPoint.Address.ToString();
                            if (!this.KnownClients.ContainsKey(clientAddress))
                            {
                                this.KnownClients.TryAdd(clientAddress, new IPEndPoint(result.RemoteEndPoint.Address, result.RemoteEndPoint.Port));
                                Invoker.Raise(ref this.ClientConnected, this, ConnectionType.Udp, clientAddress);
                            }

                            Invoker.Raise(ref this.MessageReceived, this, new BasicDataMessage(clientAddress, Encoding.UTF8.GetString(result.Buffer, 0, result.Buffer.Length).TrimEnd()));
                        }
                    }
                    catch
                    {
                        // stream closed
                    }
                });

                return true;
            }

            return false;
        }

        public bool Stop()
        {
            if (this.isConnected)
            {
                this.server.Close();
                this.Dispose();

                this.isConnected = false;

                return true;
            }

            return false;
        }

        public void Broadcast(IDataMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "No message provided"); // TODO: add a base class
            }

            byte[] datagram = Encoding.UTF8.GetBytes(message.Data.ToString());
            this.KnownClients.ForEach(c => this.server.SendAsync(datagram, this.PacketSize, c.Value));
        }

        public bool Send(string to, IDataMessage message)
        {
            return true; // TODO: implement
        }

        public bool DisconnectClient(string who)
        {
            return true; // TODO: implement
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.server != null)
                {
                    this.server.Dispose();
                }
            }
        }
    }
}