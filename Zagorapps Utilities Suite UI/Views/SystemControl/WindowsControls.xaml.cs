﻿namespace Zagorapps.Utilities.Suite.UI.Views.SystemControl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Audio.Library.Events;
    using Audio.Library.Managers;
    using Bluetooth.Library;
    using Commands;
    using Comparators;
    using Connectivity;
    using Controls;
    using Core.Library.Events;
    using Core.Library.Events.Windows;
    using Core.Library.Timing;
    using Core.Library.Windows;
    using Events;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Library.Factories;
    using Library.Interoperability;
    using Library.Messaging.Client;
    using Library.Messaging.Suite;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using ViewModels;
    using WindowsInput;
    using WindowsInput.Native;

    [DefaultNavigatable(WindowsControls.ViewName)]
    public partial class WindowsControls : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(WindowsControls); // TODO: maybe have enums for this instead

        protected readonly IWinSystemService WinService;
        protected readonly IWmiManagementService WmiService;
        protected readonly IAudioManager AudioManager;
        protected readonly IInteropHandle InteropHandle;
        protected readonly IInputSimulator InputSimulator;
        protected readonly ITimer TaskRetrieveTimer, WinServiceTimer;

        protected readonly WindowsControlsViewModel Model;
        protected readonly ProcessViewModelComparator ProcessComparer;

        private bool isConnectivitySuiteLive;

        public WindowsControls(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(WindowsControls.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.WinService = this.Factory.Create<IWinSystemService>();
            this.AudioManager = this.Factory.Create<IAudioManager>();
            this.InteropHandle = this.Factory.Create<IInteropHandle>();
            this.WmiService = this.Factory.Create<IWmiManagementService>();
            this.InputSimulator = this.Factory.Create<IInputSimulator>();
            this.WinServiceTimer = this.Factory.Create<ITimer>();
            this.TaskRetrieveTimer = this.Factory.Create<ITimer>();

            this.ProcessComparer = new ProcessViewModelComparator();

            this.Model = new WindowsControlsViewModel();
            this.Model.AddProhibitCommand = this.CommandProvider.CreateRelayCommand<string>(this.Model.AddProhibit);
            this.Model.MuteAudioCommand = this.CommandProvider.CreateRelayCommand(this.MuteAudio);
            this.Model.MuteButtonText = this.AudioManager.IsMuted ? UiResources.Label_Unmute : UiResources.Label_Mute;
            this.Model.Volume = this.AudioManager.Volume;

            this.WinServiceTimer.TimeElapsed += this.WinServiceTimer_TimeElapsed;
            this.AudioManager.OnVolumeChanged += this.AudioManager_OnVolumeChanged;
            this.WmiService.EventReceived += this.WmiService_EventReceived;

            if (!this.WmiService.Start())
            {
                // Not supported - may not be a laptop.

                this.WmiService.Stop();
                this.WmiService.EventReceived -= this.WmiService_EventReceived;
                this.WmiService.Dispose();

                this.Model.WmiSupported = false;
            }
            else
            {
                // TODO: add a dropdown to allow to specify monitors
                this.Model.Brightness = this.WmiService.GetBrightnesses().First().Brightness;
            }

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
            this.TaskRetrieveTimer.TimeElapsed += this.TaskRetrieveTimer_TimeElapsed;
            this.TaskRetrieveTimer.Start(1000, 1000);
        }

        public override void FinaliseView()
        {
            this.TaskRetrieveTimer.Stop();
            this.TaskRetrieveTimer.TimeElapsed -= this.TaskRetrieveTimer_TimeElapsed;
        }

        protected override void HandleSuiteMessageAsync(IUtilitiesDataMessage data)
        {
            if (data.Data is SyncMessage)
            {
                SyncMessage message = data.Data as SyncMessage;

                if (message.State == SyncState.Request)
                {
                    string syncData = this.AudioManager.IsMuted + "_" + this.AudioManager.Volume + "_" + (this.WmiService.IsWmiSupported ? this.WmiService.GetBrightnesses().First().Brightness : default(int));

                    this.OnDataSendRequest(
                        this,
                        ViewBag.GetViewName<WindowsControls>(),
                        SuiteRoute.Connectivity,
                        ViewBag.GetViewName<ConnectionInteraction>(),
                        message.From + ":" + SyncState.Response + ":" + syncData);
                }
            }
            else if (data.Data is ConnectionInteractionMessage)
            {
                this.isConnectivitySuiteLive = (data.Data as ConnectionInteractionMessage).ServiceLive;
            }
            else if (data.Data is VoiceMessage)
            {
                VoiceMessage message = data.Data as VoiceMessage;

                if (message.Value == "lock_machine")
                {
                    if (!this.HandleOperation("Lock"))
                    {
                        this.PerformConnectivityRoutingAction(message.From + ":Permitted Process Running");
                    }
                }
            }
            else if (data.Data is KeyboardMessage)
            {
                char character = (data.Data as KeyboardMessage).Character;

                this.InputSimulator.Keyboard.TextEntry(Convert.ToChar(character));
            }
            else if (data.Data is MotionMessage)
            {
                MotionMessage message = data.Data as MotionMessage;

                this.InputSimulator.Mouse.MoveMouseBy(message.X, message.Y);
            }
            else if (data.Data is ScreenMessage)
            {
                ScreenMessage message = data.Data as ScreenMessage;

                this.Slider_Window_Brightness.ValueChanged -= this.Slider_Window_Brightness_ValueChanged;
                this.WmiService.SetBrightness(message.Value);
                this.Slider_Window_Brightness.ValueChanged += this.Slider_Window_Brightness_ValueChanged;
            }
            else if (data.Data is VolumeMessage)
            {
                VolumeMessage message = data.Data as VolumeMessage;

                this.Slider_Volume.ValueChanged -= this.Slider_Volume_ValueChanged;
                this.AudioManager.OnVolumeChanged -= this.AudioManager_OnVolumeChanged;

                this.AudioManager.IsMuted = message.Enabled;

                if (message.Value.HasValue)
                {
                    this.AudioManager.Volume = message.Value.Value;
                }

                this.AudioManager.OnVolumeChanged += this.AudioManager_OnVolumeChanged;
                this.Slider_Volume.ValueChanged += this.Slider_Volume_ValueChanged;
            }
            else if (data.Data is CommandMessage)
            {
                ClientCommand command = (data.Data as CommandMessage).Command;

                if (command == ClientCommand.LeftClick)
                {
                    this.InputSimulator.Mouse.LeftButtonClick();
                }
                else if (command == ClientCommand.MiddleClick)
                {
                }
                else if (command == ClientCommand.RightClick)
                {
                    this.InputSimulator.Mouse.RightButtonClick();
                }
                else if (command == ClientCommand.DoubleTap)
                {
                    this.InputSimulator.Mouse.LeftButtonDoubleClick();
                }
                else if (command == ClientCommand.Backspace)
                {
                    this.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
                }
            }
        }

        private void PerformConnectivityRoutingAction(object data)
        {
            if (this.isConnectivitySuiteLive)
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

            this.PerformConnectivityRoutingAction(new BroadcastMessage { Id = "vol", Value = e.IsMuted });
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

            this.PerformConnectivityRoutingAction(new BroadcastMessage { Id = "vol", Value = this.AudioManager.IsMuted });
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            this.PerformConnectivityRoutingAction("EndSession");
        }
        
        private void WmiService_EventReceived(object sender, WmiEventArgs e)
        {
            this.Model.Brightness = e.Brightness;

            if (e.IsActive)
            {
                this.PerformConnectivityRoutingAction(new BroadcastMessage { Id = "brightnes", Value = true});
            }
            else
            {
                this.PerformConnectivityRoutingAction(new BroadcastMessage { Id = "brightnes", Value = false + "_value:" + e.Brightness });
            }
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

        private void TaskRetrieveTimer_TimeElapsed(object sender, EventArgs<int> e)
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

        private void WinServiceTimer_TimeElapsed(object sender, EventArgs<int> e)
        {

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

            this.PerformConnectivityRoutingAction(new BroadcastMessage { Id = "vol", Value = this.AudioManager.Volume });
        }

        private void Slider_Window_Brightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.WmiService.SetBrightness(this.Model.Brightness);

            this.PerformConnectivityRoutingAction(new BroadcastMessage { Id = "screen", Value = this.Model.Brightness });
        }
    }
}