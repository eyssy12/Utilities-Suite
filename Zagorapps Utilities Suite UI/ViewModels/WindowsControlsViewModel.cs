namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Zagorapps.Utilities.Suite.UI.Controls;

    public class WindowsControlsViewModel : ViewModelBase
    {
        private IEnumerable<ProcessViewModel> processes;

        private int totalProcesses;

        public WindowsControlsViewModel()
        {
            this.processes = new ProcessViewModel[0];
            this.totalProcesses = this.processes.Count();
        }

        public ICommand InvokeSystemOperation { get; set; }

        public IEnumerable<ProcessViewModel> Processes
        {
            get { return this.processes; }
            set
            {
                this.SetFieldIfChanged(ref this.processes, value, nameof(this.Processes));

                this.totalProcesses = this.processes.Count();
                this.OnPropertyChanged(nameof(this.TotalProcesses));
            }
        }

        public int TotalProcesses
        {
            get { return this.totalProcesses; }
        }
    }
}