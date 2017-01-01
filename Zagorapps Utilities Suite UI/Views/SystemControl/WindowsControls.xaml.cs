namespace Zagorapps.Utilities.Suite.UI.Views.SystemControl
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Audio.Library.Events;
    using Audio.Library.Managers;
    using Commands;
    using Comparators;
    using Connectivity;
    using Controls;
    using Core.Library.Events;
    using Core.Library.Timing;
    using Core.Library.Windows;
    using Events;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Library.Factories;
    using Library.Interoperability;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using ViewModels;

    [DefaultNavigatable(WindowsControls.ViewName)]
    public partial class WindowsControls : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(WindowsControls); // TODO: maybe have enums for this instead

        protected readonly IWinSystemService WinService;
        protected readonly IAudioManager AudioManager;
        protected readonly IInteropHandle InteropHandle;
        protected readonly ITimer Timer;

        protected readonly WindowsControlsViewModel Model;
        protected readonly ProcessViewModelComparator ProcessComparer;

        private bool initialSyncWithClientPerformed;

        public WindowsControls(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(WindowsControls.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.WinService = this.Factory.Create<IWinSystemService>();
            this.AudioManager = this.Factory.Create<IAudioManager>();
            this.InteropHandle = this.Factory.Create<IInteropHandle>();
            this.Timer = this.Factory.Create<ITimer>();

            this.AudioManager.OnVolumeChanged += this.AudioManager_OnVolumeChanged;

            this.ProcessComparer = new ProcessViewModelComparator();

            this.Model = new WindowsControlsViewModel();
            this.Model.AddProhibitCommand = this.CommandProvider.CreateRelayCommand<string>(this.Model.AddProhibit);
            this.Model.MuteAudioCommand = this.CommandProvider.CreateRelayCommand(this.MuteAudio);
            this.Model.MuteButtonText = this.AudioManager.IsMuted ? "Unmute" : "Mute";
            this.Model.Volume = this.AudioManager.Volume;

            SystemEvents.SessionSwitch += this.SystemEvents_SessionSwitch;
            SystemEvents.SessionEnding += this.SystemEvents_SessionEnding;

            this.DataContext = this;
        }

        public WindowsControlsViewModel ViewModel
        {
            get { return this.Model; }
        }

        public override void InitialiseView(object arg)
        {
            this.Timer.TimeElapsed += this.Timer_TimeElapsed;
            this.Timer.Start(1000, 1000);
        }

        public override void FinaliseView()
        {
            this.Timer.Stop();
            this.Timer.TimeElapsed -= this.Timer_TimeElapsed;
        }

        protected override void HandleProcessMessage(IUtilitiesDataMessage data)
        {
            string messageData = data.Data.ToString();

             // TODO:custom object
            if (messageData.Contains(":"))
            {
                string[] split = messageData.Split(':');

                if (split[0] == "vol")
                {
                    bool muted;
                    if (bool.TryParse(split[1], out muted))
                    {
                        this.AudioManager.IsMuted = muted;
                    }
                    else
                    {
                        int volume = int.Parse(split[1]);

                        this.AudioManager.Volume = volume;
                    }
                }
                else
                {
                    if (split[1].Contains("lock"))
                    {
                        if (!this.HandleOperation("Lock"))
                        {
                            this.PerformConnectivityRoutingAction(split[0] + ":Permitted Process Running");
                        }
                    }
                    else if (split[1] == "SyncClient")
                    {
                        string syncData = this.AudioManager.IsMuted + "_" + this.AudioManager.Volume;

                        this.OnDataSendRequest(
                            this,
                            ViewBag.GetViewName<WindowsControls>(),
                            SuiteRoute.Connectivity,
                            ViewBag.GetViewName<ConnectionInteraction>(),
                            split[0] + ":SyncResponse:" + syncData);
                    }
                }
            }
            else if (messageData == "SyncResponseAck")
            {
                this.initialSyncWithClientPerformed = true;
            }
        }

        private void PerformConnectivityRoutingAction(string data)
        {
            if (this.initialSyncWithClientPerformed)
            {
                this.OnDataSendRequest(
                    this,
                    ViewBag.GetViewName<WindowsControls>(),
                    SuiteRoute.Connectivity,
                    ViewBag.GetViewName<ConnectionInteraction>(),
                    data);
            }
        }

        private void AudioManager_OnVolumeChanged(object sender, VolumeChangeEvent e)
        {
            this.Model.MuteButtonText = e.IsMuted ? "Unmute" : "Mute";
            this.Model.Volume = e.Volume;

            this.PerformConnectivityRoutingAction("br:vol:" + e.IsMuted);
        }

        private void MuteAudio()
        {
            if (this.AudioManager.IsMuted)
            {
                this.Model.MuteButtonText = "Mute";
                this.AudioManager.IsMuted = false;
            }
            else
            {
                this.Model.MuteButtonText = "Unmute";
                this.AudioManager.IsMuted = true;
            }

            this.PerformConnectivityRoutingAction("br:vol:" + this.AudioManager.IsMuted);
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            this.PerformConnectivityRoutingAction("EndSession");
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                this.PerformConnectivityRoutingAction("machine_locked");
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                this.PerformConnectivityRoutingAction("machine_unlocked");
            }
        }

        private bool HandleOperation(string param)
        {
            if (this.Model.ControlsEnabled)
            {
                switch (param)
                {
                    case "Lock":
                        this.WinService.LockMachine();
                        return true;
                    case "LogOff":
                        this.WinService.LogOff();
                        return true;
                }
            }

            return false;
        }

        private void ConfirmDialog_OnConfirm(object sender, ConfirmDialogEventArgs e)
        {
            this.HandleOperation(e.First);
        }

        private void Timer_TimeElapsed(object sender, EventArgs<int> e)
        {
            Task.Run(() =>
            {
                IEnumerable<ProcessViewModel> disctint = Process
                    .GetProcesses()
                    .Select(p => new ProcessViewModel
                    {
                        ProcessId = p.Id,
                        ProcessName = p.ProcessName,
                        TimeRunning = "-1" //this.GetTotalProcessorTime(p)
                                })
                    .Except(this.Model.Processes, this.ProcessComparer);

                // TODO: investigate - sometimes there is item inconsistency with the model and the ItemsSource in the view which throws InvalidOperationException
                this.Model.RemoveStaleProcesses();
                this.Model.AddProcesses(disctint);
                this.Model.VerifyControlsAvailability();
            });
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            this.Model.Filter = box.Text;
        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            Chip chip = (Chip)sender;

            this.Model.RemoveProhibit(chip.Content.ToString());
        }

        private void Slider_Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.AudioManager.Volume = this.Model.Volume;
            this.AudioManager.IsMuted = false;

            this.PerformConnectivityRoutingAction("br:vol:" + this.AudioManager.Volume);
        }
    }
}