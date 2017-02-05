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
        private readonly ICommand openDropFolder;

        private DateTime nextHeartbeatTimestamp;

        public ConnectedClientViewModel(string clientName, ConnectionType connectionType, ICommand openDropFolder)
        {
            this.name = clientName;
            this.connectionType = connectionType;
            this.openDropFolder = openDropFolder;
        }

        public ConnectionType ConnectionType
        {
            get { return this.connectionType; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public ICommand OpenDropFolder
        {
            get { return this.openDropFolder; }
        }

        public DateTime NextHeartbeatTimestamp
        {
            get { return this.nextHeartbeatTimestamp; }
            set { this.SetField(ref this.nextHeartbeatTimestamp, value, nameof(this.NextHeartbeatTimestamp)); }
        }
    }
}