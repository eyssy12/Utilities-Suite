namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Bluetooth.Library;
    using Bluetooth.Library.Client.Models;
    using Bluetooth.Library.Extensions;
    using Bluetooth.Library.Providers;
    using Commands;
    using Controls;
    using Core.Library.Communications;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Library;
    using Library.Communications;
    using Services;
    using SystemControl;
    using Utilities.Library;
    using Utilities.Library.Communications.Server;
    using Utilities.Library.Factories;
    using Utilities.Library.Providers;
    using ViewModels;
    using WindowsInput;
    using WindowsInput.Native;
    using Zagorapps.Utilities.Suite.Library.Attributes;
    using VisibilityEnum = System.Windows.Visibility;

    [DefaultNavigatable(ConnectionInteraction.ViewName)]
    public partial class ConnectionInteraction : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(ConnectionInteraction),
            ServiceID = "1f1aa577-32d6-4c59-b9a2-f262994783e9",
            Pin = "12345";

        protected readonly IBluetoothServicesProvider Provider;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IInputSimulator InputSimulator;

        protected readonly BluetoothInteractionViewModel Model;

        private ILocalNetworkServer localServer;

        public ConnectionInteraction(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(ConnectionInteraction.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.Provider = this.Factory.Create<IBluetoothServicesProvider>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
            this.InputSimulator = this.Factory.Create<IInputSimulator>();

            this.Model = new BluetoothInteractionViewModel();
            this.Model.Pin = ConnectionInteraction.Pin;
            this.Model.ServiceStartCommand = this.CommandProvider.CreateRelayCommand(() => this.InvokeService());

            this.DataContext = this;
        }

        public BluetoothInteractionViewModel ViewModel
        {
            get { return this.Model; }
        }

        public override void InitialiseView(object arg)
        {
            Console.WriteLine(ViewName + " - initialised");
        }

        public override void FinaliseView()
        {
            Console.WriteLine(ViewName + " - finalised");
        }

        // TODO: internal messaging protocol is a mess - need to come up with a protocol (custom objects extending from BasicDataMessage) and refactor all "Process Message" implementations
        // i.e. ClientBroadcastMessage
        // i.e. TargetClientMessage
        protected override void HandleProcessMessage(IUtilitiesDataMessage data)
        {
            string received = data.Data.ToString();

            if (received.Contains(':')) // TODO: create a custom object instead of string split
            {
                string[] split = received.Split(':');

                // this.localServer.Send(message.From, new BasicDataMessage(ConnectionInteraction.ViewName, "SyncResponse:sync_data"));

                if (split.Length == 2)
                {
                    this.localServer.Send(split[0], new BasicDataMessage(ConnectionInteraction.ViewName, split[1]));
                }
                else if (split.Length == 3) // index 0 = "br" = broadcast
                {
                    if (split[0] == "br")
                    {
                        this.localServer.Broadcast(new BasicDataMessage(ConnectionInteraction.ViewName, split[1] + ":" + split[2]));
                    }
                    else
                    {
                        this.localServer.Send(split[0], new BasicDataMessage(ConnectionInteraction.ViewName, split[1] + ":" + split[2]));
                    }
                }
            }
            else if (received == "machine_locked" || received == "machine_unlocked")
            {
                this.localServer.Broadcast(new BasicDataMessage(ConnectionInteraction.ViewName, received));
            }
            else if (received == "EndSession")
            {
                if (this.Model.ServiceEnabled)
                {
                    this.InvokeService();
                }
            }
        }

        private void HandleInteraction(IDataMessage message)
        {
            Task.Run(() =>
            {
                // TODO: implement the below code into some sort of a decision tree

                string data = message.Data.ToString();

                if (string.IsNullOrWhiteSpace(data))
                {
                    // raise error
                }
                else
                {
                    ClientCommand command;
                    if (Enum.TryParse(data, out command))
                    {
                        if (command == ClientCommand.LeftClick)
                        {
                            this.InputSimulator.Mouse.LeftButtonClick();
                        }
                        else if (command == ClientCommand.MiddleClick)
                        {
                        }
                        else if (command == ClientCommand.RightClick)
                        {
                            this.InputSimulator.Mouse.RightButtonClick();
                        }
                        else if (command == ClientCommand.DoubleTap)
                        {
                            this.InputSimulator.Mouse.LeftButtonDoubleClick();
                        }
                    }
                    else if (data.Contains(":"))
                    {
                        string[] dataSplit = data.Split(':');

                        if (dataSplit[0] == "vol")
                        {
                            this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), "vol:" + dataSplit[1]);
                        }
                        else
                        {

                            float xMovingUnits = float.Parse(dataSplit[0]);
                            float yMovingUnits = float.Parse(dataSplit[1]);

                            this.InputSimulator.Mouse.MoveMouseBy((int)xMovingUnits, (int)yMovingUnits);
                        }
                    }
                    else if (data.Contains("lock"))
                    {
                        this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), message.From + ":" + data);
                    }
                    else if (data == "SyncRequest")
                    {
                        this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), message.From + ":SyncClient");
                    }
                    else if (data == "SyncResponseAck")
                    {
                        this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), data);
                    }
                    else if (data == ServerCommand.Backspace.ToString())
                    {
                        this.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
                    }
                    else if (data.Length > 1)
                    {
                        // KB
                        // typing or whatever
                    }
                    else
                    {
                        // KB char input
                        this.InputSimulator.Keyboard.TextEntry(Convert.ToChar(data));
                    }
                }
            });
        }

        private void StartService()
        {
            Thread.Sleep(1000);

            if (this.ViewModel.ConnectionType == ConnectionType.Bluetooth)
            {
                ConnectionSettings settings = new ConnectionSettings { ServiceID = Encoding.UTF8.GetBytes(this.Model.Pin).CreateJavaUUIDBasedGuid(), Pin = this.Model.Pin };

                this.localServer = new LocalBluetoothServer(this.Provider.CreateReceiver(settings, this.Factory.Create<IBluetoothServicesProvider>()), this.Factory.Create<INetworkConnectionProvider>());
            }
            else if (this.ViewModel.ConnectionType == ConnectionType.Udp)
            {
                this.localServer = new LocalUdpServer(30301);

                //ConstructionContext context = new ConstructionContext(); // TODO: this approach is bullshit, i should just have a method for the corresponding connection type
                //context.AddValue("endpointPort", 30301);
            }

            this.localServer.ClientConnected += this.Server_ClientConnected;
            this.localServer.ClientDisconnected += this.Server_ClientDisconnected;
            this.localServer.MessageReceived += this.Server_MessageReceived;
            this.localServer.Start();
        }

        private void Server_ClientDisconnected(object sender, EventArgs<ConnectionType, string> e)
        {
            ConnectedClientViewModel temp;
            this.Model.InvokeConnectedClientNotifyableAction(c => c.TryRemove(e.Second, out temp));

            Console.WriteLine();
            Console.WriteLine(" -->  '" + e.Second + "' disconnected from server.");
            Console.WriteLine();
        }

        private void Server_ClientConnected(object sender, EventArgs<ConnectionType, string> e)
        {
            this.Model.InvokeConnectedClientNotifyableAction(c => c.TryAdd(e.Second, new ConnectedClientViewModel(e.Second, e.First)));
        }

        private void Server_MessageReceived(object sender, EventArgs<IDataMessage> e)
        {
            this.Model.ServiceClientLogConsole = e.First.From + ": " + e.First.Data;

            this.HandleInteraction(e.First);
        }

        private void ComboBox_ConnectionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((ConnectionType)e.AddedItems[0] == ConnectionType.Bluetooth)
            {
                if (!this.Provider.IsBluetoothAvailable)
                {
                    this.Model.ServiceButtonEnabled = false;
                    this.Model.ServiceButtonText = "No Bluetooth Available";
                }
                else
                {
                    this.Model.ServiceButtonEnabled = true;
                    this.Model.ServiceButtonText = "Start Service";
                }
            }
            else
            {
                this.Model.ServiceButtonEnabled = true;
                this.Model.ServiceButtonText = "Start Service";
            }
        }

        private async void InvokeService()
        {
            await Task.Run(() =>
            {
                this.Model.ProgressBarVisibility = VisibilityEnum.Visible;
                this.Model.StartServiceButtonVisibility = VisibilityEnum.Hidden;

                this.Model.ContentEnabled = false;
                this.Model.ServiceButtonEnabled = false;

                if (this.Model.ServiceEnabled)
                {
                    this.StopService();
                    this.Model.ServiceButtonText = "Start Service";
                    this.Model.ContentVisibility = VisibilityEnum.Hidden;

                    this.Model.ServiceEnabled = false;
                }
                else
                {
                    this.Model.ServiceEnabled = true;

                    this.StartService();
                    this.Model.ServiceButtonText = "End Service";
                    this.Model.ContentVisibility = VisibilityEnum.Visible;
                }

                this.Model.ContentEnabled = true;
                this.Model.ServiceButtonEnabled = true;

                this.Model.ProgressBarVisibility = VisibilityEnum.Hidden;
                this.Model.StartServiceButtonVisibility = VisibilityEnum.Visible;
            });

            if (this.Model.ServiceEnabled)
            {
                this.Notifier.Notify(this.Model.ConnectionType + " Service Started");
            }
            else
            {
                this.Notifier.Notify(this.Model.ConnectionType + " Service Stopped");
            }
        }

        private void StopService()
        {
            this.localServer.Stop();
            this.localServer.MessageReceived -= this.Server_MessageReceived;
            this.localServer.ClientConnected -= this.Server_ClientConnected;
            this.localServer.ClientDisconnected -= this.Server_ClientDisconnected;
            this.localServer.Dispose();

            Thread.Sleep(1000);
        }
    }
}