namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using Controls;
    using Core.Library.Extensions;
    using Utilities.Suite.Library;

    public class BluetoothInteractionViewModel : ViewModelBase
    {
        private readonly ConcurrentDictionary<string, ConnectedClientViewModel> connectedClients;

        private Visibility progressBarVisibility,
            startServiceButtonVisibility,
            contentVisibility,
            pinFieldVisiblity,
            qrCodeVisibiliity;

        private LinkedList<string> serviceClientLogger = new LinkedList<string>();

        // TOOD: apply linked list logic to this too
        private StringBuilder serviceServerLogBuilder = new StringBuilder();

        private string serviceStartText;
        private string pin;

        private int clientLogMaxLines = 10;
        
        private bool serviceEnabled, serviceStartButtonEnabled, contentEnabled;

        private ConnectionType conenctionType;

        private BitmapSource qrCodeSource;

        public BluetoothInteractionViewModel()
        {
            this.connectedClients = new ConcurrentDictionary<string, ConnectedClientViewModel>();

            this.ServiceButtonText = "Start Service";
            this.ConnectionType = ConnectionType.Bluetooth;
            this.ProgressBarVisibility = Visibility.Hidden;
            this.StartServiceButtonVisibility = Visibility.Visible;
            this.ContentVisibility = Visibility.Hidden;
            this.PinFieldVisibility = Visibility.Visible;
            this.QRCodeButtonVisiblity = Visibility.Hidden;
            this.ContentEnabled = false;
            this.ServiceEnabled = false;
            this.ServiceButtonEnabled = true;
        }

        public ConcurrentDictionary<string, ConnectedClientViewModel> ClientModels
        {
            get { return this.connectedClients; }
        }

        public IEnumerable<ConnectedClientViewModel> ConnectedClients
        {
            get { return this.ClientModels.Select(h => h.Value).ToArray(); }
        }

        public string ServiceClientLogConsole
        {
            get
            {
                return this.serviceClientLogger.Any() ? this.serviceClientLogger.Aggregate((a, b) => a + "\n" + b) : string.Empty;
            }

            set
            {
                if (this.serviceClientLogger.Count == this.clientLogMaxLines)
                {
                    this.serviceClientLogger.RemoveFirst();
                }

                this.serviceClientLogger.AddLast(value);

                this.OnPropertyChanged(nameof(this.ServiceClientLogConsole));
            }
        }

        public string ServiceServerLogConsole
        {
            get
            {
                return this.serviceServerLogBuilder.ToString();
            }

            set
            {
                this.serviceServerLogBuilder.AppendLine(value);

                this.OnPropertyChanged(nameof(this.ServiceServerLogConsole));
            }
        }

        public string Pin
        {
            get { return this.pin; }
            set { this.SetField(ref this.pin, value, nameof(this.Pin)); }
        }

        public bool ServiceEnabled
        {
            get { return this.serviceEnabled; }
            set { this.SetField(ref this.serviceEnabled, value, nameof(this.ServiceEnabled)); }
        }

        public bool ContentEnabled
        {
            get { return this.contentEnabled; }
            set { this.SetField(ref this.contentEnabled, value, nameof(this.ContentEnabled)); }
        }

        public bool ServiceButtonEnabled
        {
            get { return this.serviceStartButtonEnabled; }
            set { this.SetField(ref this.serviceStartButtonEnabled, value, nameof(this.ServiceButtonEnabled)); }
        }

        public string ServiceButtonText
        {
            get { return this.serviceStartText; }
            set { this.SetField(ref this.serviceStartText, value, nameof(this.ServiceButtonText)); }
        }

        public Visibility ContentVisibility
        {
            get { return this.contentVisibility; }
            set { this.SetField(ref this.contentVisibility, value, nameof(this.ContentVisibility)); }
        }

        public Visibility PinFieldVisibility
        {
            get { return this.pinFieldVisiblity; }
            set { this.SetField(ref this.pinFieldVisiblity, value, nameof(this.PinFieldVisibility)); }
        }

        public Visibility ProgressBarVisibility
        {
            get { return this.progressBarVisibility; }
            set { this.SetField(ref this.progressBarVisibility, value, nameof(this.ProgressBarVisibility)); }
        }

        public Visibility StartServiceButtonVisibility
        {
            get { return this.startServiceButtonVisibility; }
            set { this.SetFieldIfChanged(ref this.startServiceButtonVisibility, value, nameof(this.StartServiceButtonVisibility)); }
        }

        public Visibility QRCodeButtonVisiblity
        {
            get { return this.qrCodeVisibiliity; }
            set { this.SetFieldIfChanged(ref this.qrCodeVisibiliity, value, nameof(this.QRCodeButtonVisiblity)); }
        }

        public BitmapSource QRCodeSource
        {
            get { return this.qrCodeSource; }
            set { this.SetFieldIfChanged(ref this.qrCodeSource, value, nameof(this.QRCodeSource)); }
        }

        public ICommand ServiceStartCommand { get; set; }

        public ConnectionType ConnectionType
        {
            get { return this.conenctionType; }
            set { this.SetFieldIfChanged(ref this.conenctionType, value, nameof(this.ConnectionType)); }
        }

        public IEnumerable<ConnectionType> ConnectionTypes
        {
            get { return this.GetValues<ConnectionType>(); }
        }

        public void UpdateConnectionClientHeartbeat(string name, DateTime time)
        {
            this.connectedClients[name].NextHeartbeatTimestamp = time;
        }

        public TResult InvokeConnectedClientNotifyableAction<TResult>(Func<ConcurrentDictionary<string, ConnectedClientViewModel>, TResult> action)
        {
            return this.NotifyableAction(this.connectedClients, action, nameof(this.ConnectedClients));
        }

        public void ClearClients()
        {
            this.connectedClients.Clear();
            this.OnPropertyChanged(nameof(this.ConnectedClients));
        }

        public bool TryRemoveClient(string clientName)
        {
            ConnectedClientViewModel model;
            if (this.connectedClients.TryRemove(clientName, out model))
            {
                this.OnPropertyChanged(nameof(this.ConnectedClients));

                return true;
            }

            return false;
        }

        public void InvokeConnectedClientNotifyableAction(Action<ConcurrentDictionary<string, ConnectedClientViewModel>> action)
        {
            this.NotifyableAction(this.connectedClients, action, nameof(this.ConnectedClients));
        }
    }
}