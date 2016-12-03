namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using Controls;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Utilities.Library;
    using Zagorapps.Utilities.Library.Tasks;

    public class TaskViewModel : ViewModelBase
    {
        private readonly ITask task;

        public TaskViewModel(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "No Task provided");
            }

            this.task = task;
            this.task.StateChanged += Task_StateChanged;
        }

        private void Task_StateChanged(object sender, EventArgs<TaskState> e)
        {
            this.OnPropertyChanged(nameof(this.State));
        }

        public string Identity
        {
            get { return this.Reference.Identity.ToString(); }
        }

        public string Name
        {
            get { return this.Reference.Name; }
        }

        public TaskType TaskType
        {
            get { return this.Reference.TaskType; }
        }

        public TaskState State
        {
            get { return this.Reference.State; }
        }

        public string Description
        {
            get { return this.Reference.Description; }
        }

        public ITask Reference
        {
            get { return this.task; }
        }
    }
}