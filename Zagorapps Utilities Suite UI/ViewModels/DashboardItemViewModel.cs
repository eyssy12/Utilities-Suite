namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System.Windows.Input;
    using Controls;

    public class DashboardItemViewModel : ViewModelBase
    {
        public string Identifier { get; set; }

        public ICommand ChangeSuiteCommand { get; set; }
    }
}