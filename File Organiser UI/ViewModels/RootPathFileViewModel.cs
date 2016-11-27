namespace File.Organiser.UI.ViewModels
{
    using Controls;

    public class RootPathFileViewModel : ViewModelBase
    {
        public string File { get; set; } // TODO: omit this and make use of datagrid multiselect feature

        public bool Exempt { get; set; }
    }
}