namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Windows;
    using Commands;
    using Controls;
    using Library.Attributes;
    using Utilities.Library.Factories;

    [Navigatable(UdpConnection.ViewName)]
    public partial class UdpConnection : ViewControlBase
    {
        private const string ViewName = nameof(UdpConnection);

        public UdpConnection(IOrganiserFactory factory, ICommandProvider commandProvider) 
            : base(UdpConnection.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();
        }

        public override void FinaliseView()
        {
        }

        public override void InitialiseView(object arg)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Any, 30301);
            UdpClient server = new UdpClient(serverEndpoint);

            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);

            byte[] data = new byte[1024];
            while (true)
            {
                data = server.Receive(ref client);

                Console.WriteLine("Message received from {0}:", client.ToString());
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                Console.WriteLine();
            }
        }
    }
}