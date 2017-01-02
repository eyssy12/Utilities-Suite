namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using Controls;
    using Utilities.Suite.Library;

    public class ConnectedClientViewModel : ViewModelBase
    {
        private readonly ConnectionType connectionType;
        private readonly string name;

        private DateTime nextHeartbeatTimestamp;

        public ConnectedClientViewModel(string clientName, ConnectionType connectionType)
        {
            this.name = clientName;
            this.connectionType = connectionType;
        }

        public ConnectionType ConnectionType
        {
            get { return this.connectionType; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public DateTime NextHeartbeatTimestamp
        {
            get { return this.nextHeartbeatTimestamp; }
            set { this.SetField(ref this.nextHeartbeatTimestamp, value, nameof(this.NextHeartbeatTimestamp)); }
        }
    }
}