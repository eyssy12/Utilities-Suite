namespace Zagorapps.Utilities.Suite.UI.ViewModels
{
    using System;
    using Controls;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Utilities.Suite.Library;
    using Zagorapps.Utilities.Suite.Library.Tasks;

    public class TaskViewModel : ViewModelBase
    {
        private readonly ITask task;

        private Lazy<bool> isScheduled;

        public TaskViewModel(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "No Task provided");
            }

            this.task = task;
            this.task.StateChanged += this.Task_StateChanged;

            this.isScheduled = new Lazy<bool>(() => this.task.GetType() == typeof(IScheduledTask));
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

        public string LastRan
        {
            get { return this.Reference.LastRan.HasValue ? this.Reference.LastRan.Value.ToString() :  "-"; }
        }

        public string NextScheduled
        {
            get
            {
                if (this.isScheduled.Value)
                {
                    IScheduledTask scheduled = task as IScheduledTask;

                    return scheduled.NextScheduled.HasValue ? scheduled.NextScheduled.Value.ToString() : "-";
                }

                return "N/A";
            }
        }

        public ITask Reference
        {
            get { return this.task; }
        }

        private void Task_StateChanged(object sender, EventArgs<TaskState> e)
        {
            this.OnPropertyChanged(nameof(this.State));
            this.OnPropertyChanged(nameof(this.LastRan));
            this.OnPropertyChanged(nameof(this.NextScheduled));
        }
    }
}