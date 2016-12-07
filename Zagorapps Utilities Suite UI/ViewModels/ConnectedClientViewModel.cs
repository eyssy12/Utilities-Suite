namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using Bluetooth.Library.Handlers;
    using Controls;

    public class ConnectedClientViewModel : ViewModelBase
    {
        private readonly IBluetoothConnectionHandler handler;

        private string name;
        private int heartbeatCountdown;

        public ConnectedClientViewModel(string clientName, IBluetoothConnectionHandler handler)
        {
            this.name = clientName;
            this.handler = handler;
        }

        public IBluetoothConnectionHandler Handler
        {
            get { return this.handler; }
        }

        public int HeartbeatCurrentTime
        {
            get { return this.heartbeatCountdown; }
            set { this.SetField(ref heartbeatCountdown, value, nameof(this.HeartbeatCurrentTime)); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetField(ref name, value, nameof(this.Name)); }
        }
    }
}