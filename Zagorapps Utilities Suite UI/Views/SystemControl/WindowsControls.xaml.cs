namespace Zagorapps.Utilities.Suite.UI.Views.SystemControl
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Commands;
    using Connectivity;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Core.Library.Timing;
    using Core.Library.Windows;
    using Events;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using Utilities.Library.Factories;
    using ViewModels;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable(WindowsControls.ViewName)]
    public partial class WindowsControls : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(WindowsControls); // TODO: maybe have enums for this instead

        protected readonly IWinSystemService WinService;
        protected readonly ITimer Timer;

        protected readonly WindowsControlsViewModel Model;

        public WindowsControls(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(WindowsControls.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.WinService = this.Factory.Create<IWinSystemService>();
            this.Timer = this.Factory.Create<ITimer>();

            this.Model = new WindowsControlsViewModel();
            this.Model.AddProhibitCommand = this.CommandProvider.CreateRelayCommand<string>(param => this.Model.AddProhibit(param));

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
            this.Timer.TimeElapsed += Timer_TimeElapsed;
            this.Timer.Start(1000, 1000);
        }

        public override void FinaliseView()
        {
            this.Timer.Stop();
            this.Timer.TimeElapsed -= Timer_TimeElapsed;
        }

        public override void ProcessMessage(IUtilitiesDataMessage data)
        {
            if (data.Data.ToString().Contains(":")) // TODO:custom object
            {
                string[] split = data.Data.ToString().Split(':');

                if (split[1].Contains("lock"))
                {
                    if (!this.HandleOperation("Lock"))
                    {
                        this.OnDataSendRequest(
                            this,
                            ViewBag.GetViewName<WindowsControls>(), 
                            SuiteRoute.Connectivity,
                            ViewBag.GetViewName<ConnectionInteraction>(),
                            split[0] + ":Permitted Process Running");
                    }
                }
            }
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            this.OnDataSendRequest(this, WindowsControls.ViewName, SuiteRoute.Connectivity, ViewBag.GetViewName<ConnectionInteraction>(), "EndSession");
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                this.OnDataSendRequest(this, WindowsControls.ViewName, SuiteRoute.Connectivity, ViewBag.GetViewName<ConnectionInteraction>(), "machine_locked");
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                this.OnDataSendRequest(this, WindowsControls.ViewName, SuiteRoute.Connectivity, ViewBag.GetViewName<ConnectionInteraction>(), "machine_unlocked");
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
            var processes = Process
                .GetProcesses()
                .Select(p => new ProcessViewModel
                {
                    ProcessName = p.ProcessName,
                    IsRunning = p.IsProcessRunning(),
                    TimeRunning = "-1" //this.GetTotalProcessorTime(p)
                })
                .ToArray();

            var disctint = processes
                .Where(p => p.IsRunning)
                .Except(this.Model.Processes, new ProcessComparer());

            this.Model.AddProcesses(disctint);
            this.Model.VerifyControlsAvailability();
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

        private sealed class ProcessComparer : IEqualityComparer<ProcessViewModel>
        {
            public bool Equals(ProcessViewModel x, ProcessViewModel y)
            {
                return x.ProcessName == y.ProcessName;
            }

            public int GetHashCode(ProcessViewModel obj)
            {
                return 0;
            }
        }
    }
}