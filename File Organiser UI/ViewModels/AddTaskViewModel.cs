namespace File.Organiser.UI.ViewModels
{
    using Controls;

    public class AddTaskViewModel : ViewModelBase
    {
        private string description;

        public AddTaskViewModel()
        {
            this.description = string.Empty;
        }

        public string Description
        {
            get { return this.description; }
            set { this.SetFieldIfChanged(ref description, value, nameof(this.Description)); }
        }
    }
}