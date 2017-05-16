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
        private int volume;
        private int brightness;
        private string filter, muteButtonText;

        private bool controlsEnabled, wmiSupported;

        public WindowsControlsViewModel()
        {
            this.processes = new List<ProcessViewModel>();
            this.prohibits = new List<string>();
            this.totalProcesses = this.processes.Count();
            this.filter = string.Empty;
            this.controlsEnabled = true; //TODO: will change if i allow save/load feature
        }

        public ICommand InvokeSystemOperation { get; set; }

        public ICommand AddProhibitCommand { get; set; }

        public ICommand MuteAudioCommand { get; set; }

        public bool ControlsEnabled
        {
            get { return this.controlsEnabled; }
            set { this.SetFieldIfChanged(ref this.controlsEnabled, value, nameof(this.ControlsEnabled)); }
        }

        public List<ProcessViewModel> Processes
        {
            get
            {
                return this.processes;
            }

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
                    return this.Processes;
                }
                else
                {
                    return this.Processes.Where(p => p.ProcessName.Contains(this.Filter, StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        public string WindowsTasksText
        {
            get { return "Windows Tasks (" + this.TotalProcesses + ")"; }
        }

        public IEnumerable<string> Prohibits
        {
            get { return this.prohibits.ToArray(); }
        }

        public int TotalProcesses
        {
            get { return this.totalProcesses; }
            set { this.SetFieldIfChanged(ref this.totalProcesses, value, nameof(this.WindowsTasksText)); }
        }

        public bool WmiSupported
        {
            get { return this.wmiSupported; }
            set { this.SetFieldIfChanged(ref this.wmiSupported, value, nameof(this.WmiSupported)); }
        }

        public int Brightness
        {
            get
            {
                return this.brightness;
            }
            set
            {
                this.SetFieldIfChanged(ref this.brightness, value, nameof(this.brightness));
                this.OnPropertyChanged(nameof(this.BrightnessSliderToolTipText));
            }
        }
        
        public string Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                this.SetField(ref this.filter, value, nameof(this.Filter));

                this.OnPropertyChanged(nameof(this.FilteredProcesses));
            }
        }

        public int Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                this.SetFieldIfChanged(ref this.volume, value, nameof(this.Volume));
                this.OnPropertyChanged(nameof(this.VolumeSliderToolTipText));
            }
        }

        public string MuteButtonText
        {
            get { return this.muteButtonText; }
            set { this.SetFieldIfChanged(ref this.muteButtonText, value, nameof(this.MuteButtonText)); }
        }

        public string BrightnessSliderToolTipText
        {
            get { return string.Format("Brightness ({0})", this.Brightness); }
        }

        public string VolumeSliderToolTipText
        {
            get { return string.Format("Volume ({0})", this.Volume); }
        }

        public void VerifyControlsAvailability()
        {
            if (this.Processes.Any(p => this.prohibits.Contains(p.ProcessName)))
            {
                this.ControlsEnabled = false;
            }
            else
            {
                this.ControlsEnabled = true;
            }
        }

        public void RemoveStaleProcesses()
        {
            this.Processes.RemoveAll(p => !p.ProcessId.IsProcessRunning());
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

        private void AdjustTotalProcessesCount()
        {
            this.TotalProcesses = this.Processes.Count();
        }
    }
}