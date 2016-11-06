namespace File.Organiser.UI.ViewModels
{
    using EyssyApps.Organiser.Library;

    public class TaskViewModel
    {
        public string ID { get; set; }

        public TaskType TaskType { get; set; }

        public TaskState State { get; set; }

        public string Description { get; set; }
    }
}