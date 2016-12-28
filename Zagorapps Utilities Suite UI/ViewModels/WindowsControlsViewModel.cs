namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Core.Library.Extensions;
    using Zagorapps.Utilities.Suite.UI.Controls;

    public class WindowsControlsViewModel : ViewModelBase
    {
        private List<ProcessViewModel> processes;

        private List<string> prohibits;

        private int totalProcesses;
        private string filter;

        private bool controlsEnabled;

        public WindowsControlsViewModel()
        {
            this.processes = new List<ProcessViewModel>();
            this.prohibits = new List<string>();
            this.totalProcesses = this.processes.Count();
            this.filter = string.Empty;
            this.controlsEnabled = true; //TODO: might change if i allow save/load feature
        }

        public ICommand InvokeSystemOperation { get; set; }

        public ICommand AddProhibitCommand { get; set; }

        public bool ControlsEnabled
        {
            get { return this.controlsEnabled; }
            set { this.SetFieldIfChanged(ref this.controlsEnabled, value, nameof(this.ControlsEnabled)); }
        }

        public void AddProcesses(IEnumerable<ProcessViewModel> processes)
        {
            this.Processes.AddRange(processes);

            this.OnPropertyChanged(nameof(this.FilteredProcesses));
            this.AdjustTotalProcessesCount();
        }

        public void AddProcess(ProcessViewModel process)
        {
            this.Processes.Add(process);

            this.OnPropertyChanged(nameof(this.FilteredProcesses));
            this.AdjustTotalProcessesCount();
        }

        public List<ProcessViewModel> Processes
        {
            get { return this.processes; }
            set
            {
                this.SetFieldIfChanged(ref this.processes, value, nameof(this.Processes));

                this.AdjustTotalProcessesCount();
            }
        }

        public IEnumerable<ProcessViewModel> FilteredProcesses
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Filter))
                {
                    return this.processes;
                }
                else
                {
                    return this.processes.Where(p => p.ProcessName.Contains(this.Filter, StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        public IEnumerable<string> Prohibits
        {
            get { return this.prohibits.ToArray(); }
        }

        public void AddProhibit(string prohibit)
        {
            if (!this.prohibits.Contains(prohibit))
            {
                this.prohibits.Add(prohibit);
                this.OnPropertyChanged(nameof(this.Prohibits));
            }

            this.VerifyControlsAvailability();
        }

        public void RemoveProhibit(string prohibit)
        {
            if (this.prohibits.Contains(prohibit))
            {
                this.prohibits.Remove(prohibit);
                this.OnPropertyChanged(nameof(this.Prohibits));
            }

            this.VerifyControlsAvailability();
        }

        public int TotalProcesses
        {
            get { return this.totalProcesses; }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.SetField(ref this.filter, value, nameof(this.Filter));

                this.OnPropertyChanged(nameof(this.FilteredProcesses));
            }
        }

        public void VerifyControlsAvailability()
        {
            if (this.processes.Any(p => this.prohibits.Contains(p.ProcessName)))
            {
                this.controlsEnabled = false;
            }
            else
            {
                this.controlsEnabled = true;
            }

            this.OnPropertyChanged(nameof(this.ControlsEnabled));
        }

        private void AdjustTotalProcessesCount()
        {
            this.totalProcesses = this.processes.Count();
            this.OnPropertyChanged(nameof(this.TotalProcesses));
        }
    }
}