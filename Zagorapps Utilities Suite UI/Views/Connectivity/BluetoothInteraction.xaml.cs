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
    using Bluetooth.Library.Events;
    using Bluetooth.Library.Extensions;
    using Bluetooth.Library.Handlers;
    using Bluetooth.Library.Messaging;
    using Bluetooth.Library.Networking;
    using Bluetooth.Library.Providers;
    using Commands;
    using Controls;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Core.Library.Timing;
    using Library.Communications;
    using Services;
    using Utilities.Library.Factories;
    using ViewModels;
    using WindowsInput;
    using WindowsInput.Native;
    using Zagorapps.Utilities.Suite.Library.Attributes;
    using VisibilityEnum = System.Windows.Visibility;

    [DefaultNavigatable]
    public partial class BluetoothInteraction : DataFacilitatorViewControlBase
    {
        public const string ViewName = nameof(BluetoothInteraction);

        private const string ServiceID = "1f1aa577-32d6-4c59-b9a2-f262994783e9",
            Pin = "12345";

        protected readonly IBluetoothServicesProvider Provider;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IInputSimulator InputSimulator;

        protected readonly BluetoothInteractionViewModel Model;

        private ISimpleBluetoothClientReceiver receiver;

        public BluetoothInteraction(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(BluetoothInteraction.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.Provider = this.Factory.Create<IBluetoothServicesProvider>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();
            this.InputSimulator = this.Factory.Create<IInputSimulator>();

            this.Model = new BluetoothInteractionViewModel();
            this.Model.Pin = BluetoothInteraction.Pin;
            this.Model.ServiceStartCommand = this.CommandProvider.CreateRelayCommand(() => this.InvokeService());

            this.DataContext = this;
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
                    IBluetoothConnectionHandler handler = this.PrepareConnectionHandler(e.First);

                    try
                    {
                        if (this.Model.InvokeHandlerNotifyableAction(m => m.TryAdd(clientName, new ConnectedClientViewModel(clientName, handler))))
                        {
                            handler.DataReceived += Handler_DataReceived;
                            handler.HeartbeatInitiated += Handler_HeartbeatInitiated;
                            handler.Begin();

                            this.Model.UpdateConnectionClientHeartbeat(clientName, DateTime.UtcNow);
                        }
                        else
                        {
                            // something happened, make sure to at least clean up the exisiting connection 
                            handler.Finish();
                        }
                    }
                    catch
                    {
                        handler.Finish();
                    }
                });
            }
        }

        private void Handler_HeartbeatInitiated(object sender, EventArgs<string, DateTime> e)
        {
            this.Model.UpdateConnectionClientHeartbeat(e.First, e.Second.AddMilliseconds(BluetoothConnectionHandlerBase.DefaultHeartbeatInterval));

            this.Model.Handlers[e.First].Handler.SendMessage(new BasicStringMessage(e.Second.ToString()));
        }

        private void Handler_DataReceived(object sender, BluetoothConnectionEventArgs e)
        {
            // TODO: implement the below code into some sort of a decision tree

            string data = BluetoothConnectionHandlerBase.DefaultEncoding.GetString(e.Arg);

            ClientCommand command;
            if (Enum.TryParse(data, out command))
            {
                if (command == ClientCommand.EndSession)
                {
                    IBluetoothConnectionHandler handler;
                    if (this.Model.TryRemoveHandler(e.Raiser, out handler))
                    {
                        handler.DataReceived -= Handler_DataReceived;
                        handler.HeartbeatInitiated -= Handler_HeartbeatInitiated;
                        handler.Finish();

                        Console.WriteLine();
                        Console.WriteLine(" -->  '" + e.Raiser + "' disconnected from server.");
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
            else if (data == ServerCommand.Backspace.ToString())
            {
                this.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
            }
            else
            {
                // KB

                this.InputSimulator.Keyboard.TextEntry(Convert.ToChar(data));
            }

            this.Model.ServiceClientLogConsole = e.Raiser + ": " + data;
        }

        private void StartService()
        {
            Thread.Sleep(1000);

            ConnectionSettings settings = this.PrepareConnectionSettings(Encoding.UTF8.GetBytes(this.Model.Pin).CreateJavaUUIDBasedGuid(), "12345");

            this.receiver = this.Provider.CreateReceiver(settings, this.Factory.Create<IBluetoothServicesProvider>());

            if (this.receiver.TryInitialise())
            {
                this.receiver.ClientReceived += this.Receiver_ClientReceived;
                this.receiver.Listen();
            }
        }

        private void StopService()
        {
            Thread.Sleep(1000);

            this.CloseClients();

            this.receiver.ClientReceived -= this.Receiver_ClientReceived;
            this.receiver.Stop();
            this.receiver = null;
        }

        private void CloseClients()
        {
            this.Model.Handlers.ForEach(a =>
            {
                a.Value.Handler.Finish();
            });

            Console.WriteLine();
            Console.WriteLine(" --> All devices (" + this.Model.Handlers.Count + ") disconnected from server.");
            Console.WriteLine();

            this.Model.InvokeHandlerNotifyableAction(m => m.Clear());
        }

        private IBluetoothConnectionHandler PrepareConnectionHandler(IBluetoothClient client)
        {
            IMessageHandler<IMessage> messageHandler = this.Factory.Create<IMessageHandler<IMessage>>();
            IMessageProvider messageProvider = this.Factory.Create<IMessageProvider>();
            IStreamProvider streamProvider = this.Factory.Create<IStreamProvider>();
            ICommandOperationsProvider operationProvider = this.Factory.Create<ICommandOperationsProvider>();
            ITimer timer = this.Factory.Create<ITimer>();
            IBluetoothConnectionHandler handler = this.Provider.CreateConnectionHandler(client, streamProvider, messageHandler, operationProvider, messageProvider, timer);

            return handler;
        }

        private ConnectionSettings PrepareConnectionSettings(Guid serviceID, string pin = null)
        {
            // TODO: add to appSettings
            return new ConnectionSettings
            {
                Pin = pin ?? string.Empty,
                ServiceID = serviceID
            };
        }
    }
}
