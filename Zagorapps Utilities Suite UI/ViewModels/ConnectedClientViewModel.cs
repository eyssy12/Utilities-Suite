namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using Controls;
    using Core.Library.Communications;

    public class ConnectedClientViewModel : ViewModelBase
    {
        private readonly INetworkConnection handler;

        private string name;
        private DateTime nextHeartbeatTimestamp;

        public ConnectedClientViewModel(string clientName, INetworkConnection handler)
        {
            this.name = clientName;
            this.handler = handler;
        }

        public INetworkConnection Handler
        {
            get { return this.handler; }
        }

        public DateTime NextHeartbeatTimestamp
        {
            get { return this.nextHeartbeatTimestamp; }
            set { this.SetField(ref nextHeartbeatTimestamp, value, nameof(this.NextHeartbeatTimestamp)); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetField(ref name, value, nameof(this.Name)); }
        }
    }
}