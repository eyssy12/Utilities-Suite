namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System.Windows;
    using System.Windows.Input;
    using Controls;

    public class BluetoothInteractionViewModel : ViewModelBase
    {
        private Visibility progressBarVisibility = Visibility.Hidden,
            startServiceButtonVisibility = Visibility.Visible;

        private ICommand serviceStartCommand;
        private bool serviceEnabled;

        public bool ServiceEnabled
        {
            get { return this.serviceEnabled; }
            set { this.SetField(ref serviceEnabled, value, nameof(this.ServiceEnabled)); }
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