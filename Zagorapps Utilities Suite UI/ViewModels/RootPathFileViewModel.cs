namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using Controls;

    public class RootPathFileViewModel : ViewModelBase
    {
        public string Path { get; set; } // TODO: omit this and make use of datagrid multiselect feature

        public string FileName { get; set; }

        public bool Exempt { get; set; }
    }
}