namespace File.Organiser.UI.ViewModels
{
    using EyssyApps.Organiser.Library;

    public class TaskViewModel
    {
        public string ID { get; set; }

        public TaskType Type { get; set; }

        public string Description { get; set; }
    }
}