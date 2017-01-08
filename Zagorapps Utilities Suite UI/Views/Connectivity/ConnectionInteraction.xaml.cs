namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
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
    using Graphics.Library.Extensions;
    using Graphics.Library.ZXing;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Services;
    using Suites;
    using Utilities.Suite.Library.Communications.Server;
    using Utilities.Suite.Library.Factories;
    using Utilities.Suite.Library.Providers;
    using Utilities.Suite.UI.Views.SystemControl;
    using ViewModels;
    using WindowsInput;
    using WindowsInput.Native;
    using VisibilityEnum = System.Windows.Visibility;

    [DefaultNavigatable(ConnectionInteraction.ViewName)]
    public partial class ConnectionInteraction : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(ConnectionInteraction),
            ServiceID = "1f1aa577-32d6-4c59-b9a2-f262994783e9",
            Pin = "12345";

        protected readonly IBluetoothServicesProvider BTServiceProvider;
        protected readonly IQRCodeServiceProvider QRCodeServiceProvider;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IInputSimulator InputSimulator;

        protected readonly BluetoothInteractionViewModel Model;

        private ILocalNetworkServer localServer;

        public ConnectionInteraction(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(ConnectionInteraction.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.BTServiceProvider = this.Factory.Create<IBluetoothServicesProvider>();
            this.QRCodeServiceProvider = this.Factory.Create<IQRCodeServiceProvider>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
            this.InputSimulator = this.Factory.Create<IInputSimulator>();

            this.Model = new BluetoothInteractionViewModel();
            this.Model.Pin = ConnectionInteraction.Pin;
            this.Model.ServiceStartCommand = this.CommandProvider.CreateRelayCommand(() => this.InvokeService());

            // TODO: add additional method to IViewControl and ISuite interfaces to initiate shutdown on controls - this will be called whenever the user terminates the application

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

            // TODO: create a custom object instead of string split
            if (received.Contains(':'))
            {
                string[] split = received.Split(':');

                // this.localServer.Send(message.From, new BasicDataMessage(ConnectionInteraction.ViewName, "SyncResponse:sync_data"));

                if (split.Length == 2)
                {
                    this.localServer.Send(split[0], new BasicDataMessage(ConnectionInteraction.ViewName, split[1]));
                }
                else if (split.Length == 3)
                {
                    // index 0 = "br" = broadcast

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
            // TODO:all incoming data is JSON

            Task.Run(() =>
            {
                // TODO: implement the below code into some sort of a decision tree

                string data = message.Data.ToString();
                string from = message.From;

                if (string.IsNullOrWhiteSpace(data))
                {
                    // raise error
                }

                IDictionary<string, object> dictionary;
                try
                {
                    dictionary = JsonConvert.DeserializeObject<ExpandoObject>(data, new ExpandoObjectConverter());
                }
                catch
                {
                    dictionary = null;
                }

                if (dictionary != null)
                {
                    // get the battery state
                    if (dictionary.ContainsKey("batCharge"))
                    {
                        bool value = (bool)dictionary["batCharge"];
                    }

                    if (dictionary.ContainsKey("chargeType"))
                    {
                        string type = (string)dictionary["chargeType"];
                    }
                    
                    if (dictionary.ContainsKey("batState"))
                    {
                        string state = (string)dictionary["batState"];
                    }

                    if (dictionary.ContainsKey("cmd"))
                    {
                        ClientCommand command;
                        if (Enum.TryParse((string)dictionary["cmd"], out command))
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
                            else if (command == ClientCommand.Backspace)
                            {
                                this.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
                            }
                        }
                        else
                        {
                            string character = (string)dictionary["cmd"];

                            this.InputSimulator.Keyboard.TextEntry(Convert.ToChar(character));
                        }
                    }
                    else if (dictionary.ContainsKey("motion"))
                    {
                        double xMovingUnits = (double)dictionary["x"];
                        double yMovingUnits = (double)dictionary["y"];

                        this.InputSimulator.Mouse.MoveMouseBy((int)xMovingUnits, (int)yMovingUnits);
                    }
                    else if (dictionary.ContainsKey("voice"))
                    {
                        if (((string)dictionary["voice"]).Contains("lock"))
                        {
                            this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), message.From + ":lock");
                        }
                    }
                    else if (dictionary.ContainsKey("vol"))
                    {
                        bool volumeEnabled = (bool)dictionary["vol"];

                        long value;

                        try
                        {
                            value = (long)dictionary["value"];
                        }
                        catch
                        {
                            value = -1;
                        }

                        if (value == -1)
                        {
                            this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), "vol:" + volumeEnabled);
                        }
                        else
                        {
                            this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), "vol:" + value);
                        }
                    }
                    else if (dictionary.ContainsKey("syncState"))
                    {
                        string value = (string)dictionary["syncState"];

                        if (value == "syncRequest")
                        {
                            this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), message.From + ":syncClient");
                        }
                        else if (value == "syncResponseAck")
                        {
                            // we're done - should only enter here once per client
                            Console.WriteLine(from + " - " + value);
                        }
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

                this.localServer = new LocalBluetoothServer(this.BTServiceProvider.CreateReceiver(settings, this.Factory.Create<IBluetoothServicesProvider>()), this.Factory.Create<INetworkConnectionProvider>());
            }
            else if (this.ViewModel.ConnectionType == ConnectionType.Udp)
            {
                this.localServer = new LocalUdpServer(30301);
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
            this.Model.ServiceClientLogConsole = DateTime.UtcNow + " -) " + e.First.From + ": " + e.First.Data;

            this.HandleInteraction(e.First);
        }

        private void ComboBox_ConnectionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionType type = (ConnectionType)e.AddedItems[0];

            if (type == ConnectionType.Bluetooth && !this.BTServiceProvider.IsBluetoothAvailable)
            {
                this.Model.ServiceButtonEnabled = false;
                this.Model.ServiceButtonText = "No Bluetooth Available";
            }
            else
            {
                if (type == ConnectionType.Bluetooth)
                {
                    this.Model.PinFieldVisibility = VisibilityEnum.Visible;
                    this.Model.QRCodeSource = this.QRCodeServiceProvider.GenerateImage(this.BTServiceProvider.Name + "-" + this.BTServiceProvider.LocalAddress.ToString("C") + "-" + this.Model.Pin, 500, 500).ToSource();
                }
                else
                {
                    this.Model.PinFieldVisibility = VisibilityEnum.Hidden;
                    this.Model.QRCodeSource = this.QRCodeServiceProvider.GenerateImage("localaddress and port", 500, 500).ToSource();
                }

                this.Model.ServiceButtonEnabled = true;
                this.Model.ServiceButtonText = "Start Service";
            }
        }

        private async void InvokeService()
        {
            if (this.Model.ConnectionType != ConnectionType.Tcp)
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

                        this.Model.ServiceEnabled = false;

                        this.Model.ClearClients();
                        this.Model.ServiceButtonText = "Start Service";
                        this.Model.ContentVisibility = VisibilityEnum.Hidden;
                        this.Model.QRCodeButtonVisiblity = VisibilityEnum.Hidden;
                    }
                    else
                    {
                        this.StartService();

                        this.Model.ServiceEnabled = true;

                        this.Model.ServiceButtonText = "End Service";
                        this.Model.ContentVisibility = VisibilityEnum.Visible;
                        this.Model.QRCodeButtonVisiblity = VisibilityEnum.Visible;
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

                this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), ConnectivitySuite.Name + ":" + this.Model.ServiceEnabled);
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