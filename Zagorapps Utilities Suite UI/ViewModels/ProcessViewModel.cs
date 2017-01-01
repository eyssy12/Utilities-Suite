namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using Controls;

    public class ProcessViewModel : ViewModelBase
    {
        public int ProcessId { get; set; }

        public string ProcessName { get; set; }

        public string TimeRunning { get; set; }

        public bool IsRunning { get; set; }
    }
}