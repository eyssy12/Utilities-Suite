namespace Zagorapps.Utilities.Suite.UI.Views.SystemControl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Commands;
    using Core.Library.Events;
    using Core.Library.Timing;
    using Core.Library.Windows;
    using Events;
    using Library.Attributes;
    using Library.Communications;
    using Utilities.Library.Factories;
    using ViewModels;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable(WindowsControls.ViewName)]
    public partial class WindowsControls : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(WindowsControls);

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
            if (data.Data.ToString().Contains("lock"))
            {
                this.HandleOperation("Lock");
            }
        }

        protected void HandleOperation(string param)
        {
            switch (param)
            {
                case "Lock":
                    this.WinService.LockMachine();
                    break;
                case "LogOff":
                    this.WinService.LogOff();
                    break;
            }
        }

        protected void ConfirmDialog_OnConfirm(object sender, ConfirmDialogEventArgs e)
        {
            this.HandleOperation(e.First);
        }

        private void Timer_TimeElapsed(object sender, EventArgs<int> e)
        {
            var distinct = Process
                .GetProcesses()
                .Select(p => new ProcessViewModel
                {
                    ProcessName = p.ProcessName,
                    TimeRunning = "test" //p.TotalProcessorTime.TotalSeconds.ToString()
                })
                .Union(this.Model.Processes)
                .Distinct();

            this.Model.Processes = distinct;
        }

        private class ProcessComparer : IEqualityComparer<ProcessViewModel>
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