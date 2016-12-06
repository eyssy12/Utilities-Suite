namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using System.Collections.Concurrent;
    using Bluetooth.Library.Client.Models;
    using Bluetooth.Library.Handlers;
    using Bluetooth.Library.Providers;
    using Commands;
    using Controls;
    using Library.Communications;
    using Utilities.Library.Factories;
    using WindowsInput;
    using Zagorapps.Utilities.Suite.Library.Attributes;

    [DefaultNavigatable]
    public partial class BluetoothInteraction : DataFacilitatorViewControlBase
    {
        public const string ViewName = nameof(BluetoothInteraction);

        private const string ServiceID = "1f1aa577-32d6-4c59-b9a2-f262994783e9";

        protected readonly ConcurrentDictionary<string, IBluetoothConnectionHandler> ActiveClients;
        protected readonly IBluetoothServicesProvider Provider;
        protected readonly IInputSimulator InputSimulator;

        private bool serviceEnabled;

        public BluetoothInteraction(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(BluetoothInteraction.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.Provider = this.Factory.Create<IBluetoothServicesProvider>();
            this.InputSimulator = this.Factory.Create<IInputSimulator>();

            this.ActiveClients = new ConcurrentDictionary<string, IBluetoothConnectionHandler>();

            this.serviceEnabled = false;
        }

        public bool ServiceEnabled
        {
            get { return this.serviceEnabled; }
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
            Console.WriteLine("Message received");
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
