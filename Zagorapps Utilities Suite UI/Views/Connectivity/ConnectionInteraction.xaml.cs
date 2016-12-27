namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Bluetooth.Library;
    using Bluetooth.Library.Client;
    using Bluetooth.Library.Client.Models;
    using Bluetooth.Library.Extensions;
    using Bluetooth.Library.Networking;
    using Bluetooth.Library.Providers;
    using Commands;
    using Controls;
    using Core.Library.Communications;
    using Core.Library.Construction;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Library;
    using Library.Communications;
    using Microsoft.Win32;
    using Services;
    using SystemControl;
    using Utilities.Library;
    using Utilities.Library.Factories;
    using Utilities.Library.Providers;
    using ViewModels;
    using WindowsInput;
    using WindowsInput.Native;
    using Zagorapps.Utilities.Suite.Library.Attributes;
    using VisibilityEnum = System.Windows.Visibility;

    [DefaultNavigatable]
    public partial class ConnectionInteraction : DataFacilitatorViewControlBase
    {
        public const string ViewName = nameof(ConnectionInteraction);

        private const string ServiceID = "1f1aa577-32d6-4c59-b9a2-f262994783e9",
            Pin = "12345";

        protected readonly IBluetoothServicesProvider Provider;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IInputSimulator InputSimulator;
        protected readonly INetworkConnectionProvider CommsProvider;

        protected readonly BluetoothInteractionViewModel Model;

        private INetworkConnection localServer;

        private ISimpleBluetoothClientReceiver receiver; // TODO: don't think this should be here - i may need an interface for ILocalServer

        public ConnectionInteraction(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(ConnectionInteraction.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.Provider = this.Factory.Create<IBluetoothServicesProvider>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
            this.InputSimulator = this.Factory.Create<IInputSimulator>();
            this.CommsProvider = this.Factory.Create<INetworkConnectionProvider>();

            this.Model = new BluetoothInteractionViewModel();
            this.Model.Pin = ConnectionInteraction.Pin;
            this.Model.ServiceStartCommand = this.CommandProvider.CreateRelayCommand(() => this.InvokeService());

            this.DataContext = this;

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
        }

        // If i use commands to lock my machine, etc, i may want to display an overlay to force the user to log into the machine and then remove the overlay
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                //I left my desk
                this.ViewModel.Handlers.ForEach(h => h.Value.Handler.Send(new BasicDataMessage(h.Key, "machine_locked")));
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                //I returned to my desk
                this.ViewModel.Handlers.ForEach(h => h.Value.Handler.Send(new BasicDataMessage(h.Key, "machine_unlocked")));
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
                    this.Model.ServiceStartText = "Start Service";
                    this.Model.ContentVisibility = VisibilityEnum.Hidden;

                    this.Model.ServiceEnabled = false;
                }
                else
                {
                    this.Model.ServiceEnabled = true;

                    this.StartService();
                    this.Model.ServiceStartText = "End Service";
                    this.Model.ContentVisibility = VisibilityEnum.Visible;
                }

                this.Model.ContentEnabled = true;
                this.Model.ServiceButtonEnabled = true;

                this.Model.ProgressBarVisibility = VisibilityEnum.Hidden;
                this.Model.StartServiceButtonVisibility = VisibilityEnum.Visible;
            });

            if (this.Model.ServiceEnabled)
            {
                this.Notifier.Notify("Bluetooth Service Started");
            }
            else
            {
                this.Notifier.Notify("Bluetooth Service Stopped");
            }
        }

        public BluetoothInteractionViewModel ViewModel
        {
            get { return this.Model; }
        }

        public override void InitialiseView(object arg)
        {
            if (!this.Provider.IsBluetoothAvailable)
            {
                this.OnViewChange(NoBluetoothAvailable.ViewName);
            }

            Console.WriteLine(ViewName + " - initialised");
        }

        public override void FinaliseView()
        {
            Console.WriteLine(ViewName + " - finalised");
        }

        public override void ProcessMessage(IUtilitiesDataMessage data)
        {
            
        }

        private void Receiver_ClientReceived(object sender, EventArgs<IBluetoothClient> e)
        {
            string clientName = e.First.RemoteMachineName;

            if (this.Model.Handlers.Any(h => h.Key == clientName))
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
                        if (this.Model.InvokeHandlerNotifyableAction(m => m.TryAdd(clientName, new ConnectedClientViewModel(clientName, connection))))
                        {
                            connection.MessageReceived += NetworkConnection_MessageReceived;
                            connection.Start();
                        }
                        else
                        {
                            // something happened, make sure to at least clean up the exisiting connection 
                            connection.Close();
                        }
                    }
                    catch
                    {
                        connection.Close();
                    }
                });
            }
        }

        private void HandleInteraction(IDataMessage message)
        {
            // TODO: implement the below code into some sort of a decision tree

            string data = message.Data.ToString();

            ClientCommand command;
            if (Enum.TryParse(data, out command))
            {
                if (command == ClientCommand.EndSession)
                {
                    INetworkConnection handler;
                    if (this.Model.TryRemoveHandler(message.From, out handler))
                    {
                        handler.MessageReceived -= NetworkConnection_MessageReceived;
                        handler.Close();

                        Console.WriteLine();
                        Console.WriteLine(" -->  '" + message.From + "' disconnected from server.");
                        Console.WriteLine();
                    }
                }
                else if (command == ClientCommand.LeftClick)
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
                string[] newPositions = data.Split(':');

                float xMovingUnits = float.Parse(newPositions[0]);
                float yMovingUnits = float.Parse(newPositions[1]);

                this.InputSimulator.Mouse.MoveMouseBy((int)xMovingUnits, (int)yMovingUnits);
            }
            else if (data == "lock machine")
            {
                this.OnDataSendRequest(this, ConnectionInteraction.ViewName, SuiteRoute.SystemControl, WindowsControls.ViewName, data);
            }
            else if (data == ServerCommand.Backspace.ToString())
            {
                this.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
            }
            else
            {
                // KB

                // this.InputSimulator.Keyboard.TextEntry(Convert.ToChar(data));
            }
        }

        private void StartService()
        {
            Thread.Sleep(1000);

            if (this.ViewModel.ConnectionType == ConnectionType.Bluetooth)
            {
                ConnectionSettings settings = new ConnectionSettings { ServiceID = Encoding.UTF8.GetBytes(this.Model.Pin).CreateJavaUUIDBasedGuid(), Pin = this.Model.Pin };

                this.receiver = this.Provider.CreateReceiver(settings, this.Factory.Create<IBluetoothServicesProvider>());

                if (this.receiver.TryInitialise())
                {
                    this.receiver.ClientReceived += this.Receiver_ClientReceived;
                    this.receiver.Listen();
                }
            }
            else if (this.ViewModel.ConnectionType == ConnectionType.Udp)
            {
                ConstructionContext context = new ConstructionContext(); // TODO: this approach is bullshit, i should just have a method for the corresponding connection type
                context.AddValue("endpointPort", 30301);

                this.localServer = this.CommsProvider.CreateNetworkConnection(ConnectionType.Udp, context);
                this.localServer.MessageReceived += NetworkConnection_MessageReceived;
                this.localServer.Start();
            }
        }

        private void NetworkConnection_MessageReceived(object sender, EventArgs<IDataMessage> e)
        {
            this.Model.ServiceClientLogConsole = e.First.From + ": " + e.First.Data;

            this.HandleInteraction(e.First);
        }

        private void StopService()
        {
            Thread.Sleep(1000);

            if (this.ViewModel.ConnectionType == ConnectionType.Bluetooth)
            {
                this.CloseClients();

                this.receiver.ClientReceived -= this.Receiver_ClientReceived;
                this.receiver.Stop();
                this.receiver = null;
            }
            else
            {
                this.localServer.Close();
                this.localServer = null;
            }
        }

        private void CloseClients()
        {
            this.Model.Handlers.ForEach(a =>
            {
                a.Value.Handler.Close();
            });

            Console.WriteLine();
            Console.WriteLine(" --> All devices (" + this.Model.Handlers.Count + ") disconnected from server.");
            Console.WriteLine();

            this.Model.InvokeHandlerNotifyableAction(m => m.Clear());
        }
    }
}