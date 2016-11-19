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
        }

        private void Task_StateChanged(object sender, EventArgs<TaskState> e)
        {
            this.OnPropertyChanged(nameof(this.State));
        }

        public string Identity
        {
            get { return this.Task.Identity.ToString(); }
        }

        public string Name
        {
            get { return this.Task.Name; }
        }

        public TaskType TaskType
        {
            get { return this.Task.TaskType; }
        }

        public TaskState State
        {
            get { return this.Task.State; }
        }

        public string Description
        {
            get { return this.Task.Description; }
        }
    }
}