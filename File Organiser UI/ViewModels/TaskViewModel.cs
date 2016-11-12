namespace File.Organiser.UI.ViewModels
{
    using System;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Organiser.Library;
    using EyssyApps.Organiser.Library.Tasks;
    using File.Organiser.UI.Controls;

    public class TaskViewModel : ViewModelBase
    {
        protected readonly ITask Task;

        public TaskViewModel(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "No Task provided");
            }

            this.Task = task;
            this.Task.StateChanged += Task_StateChanged;

            this.ID = task.Id.ToString();
            this.TaskType = task.TaskType;
            this.State = task.State;
            this.Description = task.Description;
        }

        private void Task_StateChanged(object sender, EventArgs<TaskState> e)
        {
            this.State = e.First;
            this.OnPropertyChanged(nameof(this.State));
        }

        public string ID { get; protected set; }

        public TaskType TaskType { get; protected set; }

        public TaskState State { get; protected set; }

        public string Description { get; protected set; }
    }
}