namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using System.Collections.Concurrent;
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
    using Core.Library.Windows;
    using Graphics.Library.Extensions;
    using Graphics.Library.ZXing;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Library.Messaging.Client;
    using Library.Messaging.Suite;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Services;
    using Utilities.Suite.Library.Communications.Server;
    using Utilities.Suite.Library.Factories;
    using Utilities.Suite.Library.Providers;
    using Utilities.Suite.UI.Views.SystemControl;
    using ViewModels;
    using VisibilityEnum = System.Windows.Visibility;

    [DefaultNavigatable(ConnectionInteraction.ViewName)]
    public partial class ConnectionInteraction : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(ConnectionInteraction),
            DefaultPin = "12345";

        // Has to be concurrent because this is used in a Task.
        protected readonly ConcurrentDictionary<string, Action<string, IDictionary<string, object>>> MessageActions;

        protected readonly IBluetoothServicesProvider BTServiceProvider;
        protected readonly IQRCodeServiceProvider QRCodeServiceProvider;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IWinSystemService WinSystem;
        protected readonly IConnectivityStore Store;

        protected readonly BluetoothInteractionViewModel Model;

        private ILocalNetworkServer localServer;

        public ConnectionInteraction(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(ConnectionInteraction.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.BTServiceProvider = this.Factory.Create<IBluetoothServicesProvider>();
            this.QRCodeServiceProvider = this.Factory.Create<IQRCodeServiceProvider>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
            this.Store = this.Factory.Create<IConnectivityStore>();
            this.WinSystem = this.Factory.Create<IWinSystemService>();

            this.Model = new BluetoothInteractionViewModel();
            this.Model.Pin = ConnectionInteraction.DefaultPin; // TODO: allow save/load via ini configurator
            this.Model.ServiceStartCommand = this.CommandProvider.CreateRelayCommand(() => this.InvokeConnectivityService());

            // TODO: add additional method to IViewControl and ISuite interfaces to initiate shutdown on controls - this will be called whenever the user terminates the application

            this.MessageActions = this.CreateActionDictionary();

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
        protected override void HandleSuiteMessageAsync(IUtilitiesDataMessage data)
        {
            string received = data.Data.ToString();

            // TODO: create a custom object instead of string split
            if (data.Data is BroadcastMessage)
            {
                BroadcastMessage message = data.Data as BroadcastMessage;

                this.localServer.Broadcast(new BasicDataMessage(ConnectionInteraction.ViewName, message.Id + ":" + message.Value));
            }
            else if (received.Contains(':'))
            {
                string[] split = received.Split(':');

                // this.localServer.Send(message.From, new BasicDataMessage(ConnectionInteraction.ViewName, "SyncResponse:sync_data"));

                if (split.Length == 2)
                {
                    this.localServer.Send(split[0], new BasicDataMessage(ConnectionInteraction.ViewName, split[1]));
                }
                else if (split.Length == 3)
                {
                    this.localServer.Send(split[0], new BasicDataMessage(ConnectionInteraction.ViewName, split[1] + ":" + split[2]));
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
                    this.InvokeConnectivityService();
                }
            }
        }
        
        private void HandleClientMessage(IDataMessage message)
        {
            // TODO:all incoming data is JSON

            Task.Run(() =>
            {
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

                    Action<string, IDictionary<string, object>> action;
                    if (this.MessageActions.TryGetValue(dictionary["id"].ToString(), out action))
                    {
                        action(message.From, dictionary);
                    }
                }
                catch
                {
                    dictionary = null;
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
            this.localServer.MessageSent += this.Server_MessageSent;
            this.localServer.Start();
        }

        private void Server_MessageSent(object sender, EventArgs<IDataMessage> e)
        {
            this.Model.ServiceServerLogConsole = DateTime.UtcNow + " -) " + e.First.Data;
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
            this.Model.InvokeConnectedClientNotifyableAction(c => c.TryAdd(e.Second, new ConnectedClientViewModel(e.Second, e.First, this.CommandProvider.CreateRelayCommand(() => this.WinSystem.OpenFolder(this.Store.GetClientStorePath(e.Second))))));
        }

        private void Server_MessageReceived(object sender, EventArgs<IDataMessage> e)
        {
            // this.Model.ServiceClientLogConsole = DateTime.UtcNow + " -) " + e.First.From + ": " + e.First.Data;

            this.HandleClientMessage(e.First);
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

        private async void InvokeConnectivityService()
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

                this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), new ConnectionInteractionMessage { ServiceLive = this.Model.ServiceEnabled });
            }
        }

        private void StopService()
        {
            this.localServer.Stop();
            this.localServer.MessageReceived -= this.Server_MessageReceived;
            this.localServer.MessageSent -= this.Server_MessageSent;
            this.localServer.ClientConnected -= this.Server_ClientConnected;
            this.localServer.ClientDisconnected -= this.Server_ClientDisconnected;
            this.localServer.Dispose();

            Thread.Sleep(1000);
        }

        private ConcurrentDictionary<string, Action<string, IDictionary<string, object>>> CreateActionDictionary()
        {
            // TODO: implement the below code into some sort of a decision tree
            // TODO: add the strings to resources or consts at some stage.
            // it would be better if individual actions were encapsulated into their own classes
            // however, the classes relying on inter Suite messaging, i don't want to expose the OnDataSendRequest
            // Maybe create a service that the action in the tree may raise and that the facilitators will listen to...

            Action<string, IDictionary<string, object>> commandAction = (from, json) =>
            {
                ClientCommand command;
                string value = (string)json["value"];
                if (Enum.TryParse(value, out command))
                {
                    this.SendToWindowsControls(new CommandMessage { Command = command });
                }
                else
                {
                    this.SendToWindowsControls(new KeyboardMessage { Character = Convert.ToChar(value) });
                }
            };

            Action<string, IDictionary<string, object>> motionAction = (from, json) =>
            {
                int xMovingUnits = Convert.ToInt32(json["x"]);
                int yMovingUnits = Convert.ToInt32(json["y"]);
                
                this.SendToWindowsControls(new MotionMessage { X = xMovingUnits, Y = yMovingUnits });
            };

            Action<string, IDictionary<string, object>> voiceAction = (from, json) =>
            {
                string value = (string)json["value"];

                if (value.Contains("lock"))
                {
                    this.SendToWindowsControls(new VoiceMessage { From = from, Value = "lock_machine" });
                }
            };

            Action<string, IDictionary<string, object>> volumeAction = (from, json) =>
            {
                bool volumeEnabled = (bool)json["volOn"];
                
                object value;
                if (json.TryGetValue("value", out value))
                {
                    int volume = Convert.ToInt32(value);
                    
                    this.SendToWindowsControls(new VolumeMessage { Enabled = volumeEnabled, Value = volume });
                }
                else
                {
                    this.SendToWindowsControls(new VolumeMessage { Enabled = volumeEnabled });
                }
            };

            Action<string, IDictionary<string, object>> batteryAction = (from, json) =>
            {
                if (json.ContainsKey("batCharge"))
                {
                    bool value = (bool)json["batCharge"];
                }

                if (json.ContainsKey("chargeType"))
                {
                    string type = (string)json["chargeType"];
                }

                if (json.ContainsKey("batState"))
                {
                    string state = (string)json["batState"];
                }
            };

            Action<string, IDictionary<string, object>> syncStateAction = (from, json) =>
            {
                SyncState state = (SyncState)Enum.Parse(typeof(SyncState), (string)json["value"]);

                if (state == SyncState.Request)
                {
                    this.SendToWindowsControls(new SyncMessage { From = from, State = state });
                }
                else if (state == SyncState.ResponseAck)
                {
                }
            };

            Action<string, IDictionary<string, object>> screenAction = (from, json) =>
            {
                long value = (long)json["value"];

                this.SendToWindowsControls(new ScreenMessage { Value = Convert.ToInt32(value) });
            };

            Action<string, IDictionary<string, object>> fileAction = (from, json) =>
            {
                string action = (string)json["action"];

                if (action == "beginFileTransfer")
                {
                    long checksum = (long)json["checksum"];
                    string name = (string)json["name"];

                    this.localServer.Send(from, new BasicDataMessage(ConnectionInteraction.ViewName, "file:" + checksum + "_readyForSending"));
                }
                else if (action == "fileSending")
                {
                    long checksum = (long)json["checksum"];
                    string name = (string)json["name"];
                    string contents = (string)json["value"];
                    long remaining = (long)json["remaining"];

                    byte[] bytes = Convert.FromBase64String(contents);

                    this.Store.SaveFile(bytes, name, from, append: true);

                    if (remaining > 0)
                    {
                        this.localServer.Send(from, new BasicDataMessage(ConnectionInteraction.ViewName, "file:" + checksum + "_chunkAccepted"));
                    }
                    else
                    {
                        this.localServer.Send(from, new BasicDataMessage(ConnectionInteraction.ViewName, "file:" + checksum + "_finished"));
                    }
                }
            };

            ConcurrentDictionary<string, Action<string, IDictionary<string, object>>> messageActions = new ConcurrentDictionary<string, Action<string, IDictionary<string, object>>>();
            messageActions.TryAdd("cmd", commandAction);
            messageActions.TryAdd("motion", motionAction);
            messageActions.TryAdd("voice", voiceAction);
            messageActions.TryAdd("vol", volumeAction);
            messageActions.TryAdd("battery", batteryAction);
            messageActions.TryAdd("syncState", syncStateAction);
            messageActions.TryAdd("screen", screenAction);
            messageActions.TryAdd("file", fileAction);

            return messageActions;
        }

        private void SendToWindowsControls(object data)
        {
            this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, ViewBag.GetViewName<WindowsControls>(), data);
        }
    }
}