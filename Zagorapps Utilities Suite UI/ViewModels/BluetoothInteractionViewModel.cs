namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using Bluetooth.Library.Handlers;
    using Controls;

    public class BluetoothInteractionViewModel : ViewModelBase
    {
        private readonly ConcurrentDictionary<string, ConnectedClientViewModel> handlers;

        private Visibility progressBarVisibility = Visibility.Hidden,
            startServiceButtonVisibility = Visibility.Visible,
            contentVisibility = Visibility.Hidden;

        private StringBuilder serviceClientLogBuilder = new StringBuilder(),
            serviceServerLogBuilder = new StringBuilder();

        private string serviceStartText;
        private string pin;

        private ICommand serviceStartCommand;
        private bool serviceEnabled, serviceStartButtonEnabled, contentEnabled;

        public BluetoothInteractionViewModel()
        {
            this.handlers = new ConcurrentDictionary<string, ConnectedClientViewModel>();

            this.ServiceStartText = "Start Service";
            this.ContentEnabled = false;
            this.ServiceEnabled = false;
            this.ServiceButtonEnabled = true;
        }

        public void UpdateConnectionClientHeartbeat(string name, int time)
        {
            this.handlers[name].HeartbeatCurrentTime = time;
        }

        public TResult InvokeHandlerNotifyableAction<TResult>(Func<ConcurrentDictionary<string, ConnectedClientViewModel>, TResult> action)
        {
            return this.NotifyableAction(this.handlers, action, nameof(this.ConnectedClients));
        }

        public bool TryRemoveHandler(string client, out IBluetoothConnectionHandler handler)
        {
            ConnectedClientViewModel model;
            if (this.handlers.TryRemove(client, out model))
            {
                handler = model.Handler;
                this.OnPropertyChanged(nameof(this.ConnectedClients));

                return true;
            }

            handler = null;

            return false;
        }

        public void InvokeHandlerNotifyableAction(Action<ConcurrentDictionary<string, ConnectedClientViewModel>> action)
        {
            this.NotifyableAction(this.handlers, action, nameof(this.ConnectedClients));
        }

        public ConcurrentDictionary<string, ConnectedClientViewModel> Handlers
        {
            get { return this.handlers; }
        }

        public IEnumerable<ConnectedClientViewModel> ConnectedClients
        {
            get { return this.Handlers.Select(h => h.Value).ToArray(); }
        }

        public string ServiceClientLogConsole
        {
            get { return this.serviceClientLogBuilder.ToString(); }
            set
            {
                this.serviceClientLogBuilder.AppendLine(value);

                this.OnPropertyChanged(nameof(ServiceClientLogConsole));
            }
        }

        public string ServiceServerLogConsole
        {
            get { return this.serviceServerLogBuilder.ToString(); }
            set
            {
                this.serviceServerLogBuilder.AppendLine(value);

                this.OnPropertyChanged(nameof(ServiceServerLogConsole));
            }
        }

        public string Pin
        {
            get { return this.pin; }
            set { this.SetField(ref pin, value, nameof(this.Pin)); }
        }

        public bool ServiceEnabled
        {
            get { return this.serviceEnabled; }
            set { this.SetField(ref serviceEnabled, value, nameof(this.ServiceEnabled)); }
        }

        public bool ContentEnabled
        {
            get { return this.contentEnabled; }
            set { this.SetField(ref contentEnabled, value, nameof(this.ContentEnabled)); }
        }

        public bool ServiceButtonEnabled
        {
            get { return this.serviceStartButtonEnabled; }
            set { this.SetField(ref serviceStartButtonEnabled, value, nameof(this.ServiceButtonEnabled)); }
        }

        public string ServiceStartText
        {
            get { return this.serviceStartText; }
            set { this.SetField(ref serviceStartText, value, nameof(this.ServiceStartText)); }
        }

        public Visibility ContentVisibility
        {
            get { return this.contentVisibility; }
            set { this.SetField(ref contentVisibility, value, nameof(this.ContentVisibility)); }
        }

        public Visibility ProgressBarVisibility
        {
            get { return this.progressBarVisibility; }
            set { this.SetField(ref progressBarVisibility, value, nameof(this.ProgressBarVisibility)); }
        }

        public Visibility StartServiceButtonVisibility
        {
            get { return this.startServiceButtonVisibility; }
            set { this.SetFieldIfChanged(ref startServiceButtonVisibility, value, nameof(this.StartServiceButtonVisibility)); }
        }

        public ICommand ServiceStartCommand
        {
            get { return this.serviceStartCommand; }
            set { this.SetFieldIfChanged(ref serviceStartCommand, value, nameof(this.ServiceStartCommand)); }
        }
    }
}