namespace Zagorapps.Utilities.Suite.Library.Communications.Server
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using Bluetooth.Library.Client;
    using Bluetooth.Library.Networking;
    using Core.Library.Communications;
    using Core.Library.Construction;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Providers;

    public class LocalBluetoothServer : ILocalNetworkServer
    {
        protected readonly ConcurrentDictionary<string, INetworkConnection> Clients;

        protected readonly INetworkConnectionProvider CommsProvider;
        protected readonly ISimpleBluetoothClientReceiver BluetoothClientReceiver;

        public LocalBluetoothServer(ISimpleBluetoothClientReceiver bluetoothClientReceiver, INetworkConnectionProvider commsProvider)
        {
            if (bluetoothClientReceiver == null)
            {
                throw new ArgumentNullException(nameof(bluetoothClientReceiver), "No client receiver provided");
            }

            if (commsProvider == null)
            {
                throw new ArgumentNullException(nameof(commsProvider), "No network connection provider provided.");
            }

            this.BluetoothClientReceiver = bluetoothClientReceiver;
            this.CommsProvider = commsProvider;

            this.Clients = new ConcurrentDictionary<string, INetworkConnection>();
        }

        public event EventHandler<EventArgs<IDataMessage>> MessageReceived;

        public event EventHandler<EventArgs<IDataMessage>> MessageSent;

        public event EventHandler<EventArgs<ConnectionType, string>> ClientConnected;

        public event EventHandler<EventArgs<ConnectionType, string>> ClientDisconnected;

        public bool Start()
        {
            if (this.BluetoothClientReceiver.TryInitialise())
            {
                this.BluetoothClientReceiver.ClientReceived += this.BluetoothClientReceiver_ClientReceived;
                this.BluetoothClientReceiver.Listen();

                return true;
            }

            return false;
        }

        public bool Stop()
        {
            this.BluetoothClientReceiver.Stop();
            this.BluetoothClientReceiver.ClientReceived -= this.BluetoothClientReceiver_ClientReceived;

            this.Clients.ForEach(c => c.Value.Close());

            Console.WriteLine();
            Console.WriteLine(" --> All devices (" + this.Clients.Count + ") disconnected from server.");
            Console.WriteLine();

            this.Clients.Clear();

            return true;
        }
        
        public void Broadcast(IDataMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Data message not provided.");
            }

            this.Clients.ForEach(c => c.Value.Send(message));

            Invoker.Raise(ref this.MessageSent, this, message);
        }

        public bool Send(string to, IDataMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Data message not provided.");
            }

            if (this.Clients.ContainsKey(to))
            {
                INetworkConnection connection = this.Clients[to];

                connection.Send(message);

                Invoker.Raise(ref this.MessageSent, this, message);

                return true;
            }

            return false;
        }

        public void Dispose()
        {
            this.Disposing(true);
        }

        private void Disposing(bool disposing)
        {
            if (disposing)
            {
                this.Clients.ForEach(c => c.Value.TryDispose());
            }
        }

        private void BluetoothClientReceiver_ClientReceived(object sender, EventArgs<IBluetoothClient> e)
        {
            string clientName = e.First.RemoteMachineName;

            if (this.Clients.Any(h => h.Key == clientName))
            {
                Console.WriteLine("Device with an existing machine name '" + clientName + "' attempting to connect -> refusing.");

                e.First.Close();
                e.First.Dispose();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Client accepted: '" + clientName + "'");
                Console.WriteLine("------------------------------------");
                Console.WriteLine();

                Task.Run(() =>
                {
                    ConstructionContext context = new ConstructionContext();
                    context.AddValue("connectionClient", e.First);

                    INetworkConnection connection = this.CommsProvider.CreateNetworkConnection(ConnectionType.Bluetooth, context);

                    try
                    {
                        this.Clients.TryAdd(clientName, connection);

                        connection.MessageReceived += this.Connection_MessageReceived;
                        connection.Start();
                    }
                    catch
                    {
                        connection.Close();
                        connection.MessageReceived -= this.Connection_MessageReceived;

                        this.Clients.TryRemove(clientName, out connection);
                    }

                    Invoker.Raise(ref this.ClientConnected, this, ConnectionType.Bluetooth, clientName);
                });
            }
        }

        private void Connection_MessageReceived(object sender, EventArgs<IDataMessage> e)
        {
            string data = e.First.Data.ToString();

            bool endSession = data.Contains("EndSession");

            if (endSession) 
            {
                INetworkConnection connection;
                if (this.Clients.TryRemove(e.First.From, out connection))
                {
                    connection.MessageReceived -= this.Connection_MessageReceived;
                    connection.Close();

                    Invoker.Raise(ref this.ClientDisconnected, this, ConnectionType.Bluetooth, e.First.From);
                }
            }
            else
            {
                // Now we know that we have some data that other code may have interest in dealing with
                Invoker.Raise(ref this.MessageReceived, this, e); 
            }
        }
    }
}