namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using System.Windows.Input;
    using Controls;
    using Utilities.Suite.Library;

    public class ConnectedClientViewModel : ViewModelBase
    {
        private readonly ConnectionType connectionType;
        private readonly string name;
        private readonly ICommand openDropFolder, disconnectClient;
        private bool charging;

        private DateTime nextHeartbeatTimestamp;

        public ConnectedClientViewModel(string clientName, ConnectionType connectionType, ICommand openDropFolder, ICommand disconnectClient)
        {
            this.name = clientName;
            this.connectionType = connectionType;
            this.openDropFolder = openDropFolder;
            this.disconnectClient = disconnectClient;
            this.charging = false;
        }

        public ConnectionType ConnectionType
        {
            get { return this.connectionType; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public bool Charging
        {
            get { return this.charging; }
            set { this.SetField(ref this.charging, value, nameof(this.Charging)); }
        }
        public ICommand OpenDropFolder
        {
            get { return this.openDropFolder; }
        }

        public ICommand DisconnectClient
        {
            get { return this.disconnectClient; }
        }

        public DateTime NextHeartbeatTimestamp
        {
            get { return this.nextHeartbeatTimestamp; }
            set { this.SetField(ref this.nextHeartbeatTimestamp, value, nameof(this.NextHeartbeatTimestamp)); }
        }
    }
}