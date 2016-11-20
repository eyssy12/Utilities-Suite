namespace File.Organiser.UI.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Commands;
    using Controls;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Core.Library.Managers;
    using EyssyApps.Core.Library.Timing;
    using EyssyApps.Organiser.Library;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.Organiser.Library.Managers;
    using EyssyApps.Organiser.Library.Models.Settings;
    using EyssyApps.Organiser.Library.Providers;
    using EyssyApps.Organiser.Library.Tasks;
    using EyssyApps.UI.Library.Services;
    using MaterialDesignThemes.Wpf;
    using ViewModels;

    public partial class AddTask : ViewControlBase
    {
        public const string ViewName = nameof(AddTask);

        protected readonly AddTaskViewModel Model;

        protected readonly ITaskManager Manager;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IFileExtensionProvider Provider;
        protected readonly IList<ValidationError> Errors;

        public AddTask(IOrganiserFactory factory)
            : base(AddTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();

            this.Manager = this.Factory.Create<ITaskManager>();
            this.Provider = this.Factory.Create<IFileExtensionProvider>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();

            this.Model = new AddTaskViewModel(); // TODO: this doesn't seem the right way to go about it, should think about a more cleaner way...
            this.Model.SelectRootPathCommand = new RelayCommand<object>(value => this.Model.RootPath = this.Factory.Create<IFormsService>().SelectFolderPathDialog());
            this.Model.LoadRootPathFilesCommand = new RelayCommand<object>(value =>
            {
                if (!string.IsNullOrWhiteSpace(this.Model.RootPath))
                {
                    IEnumerable<string> files = this.Factory
                        .Create<IDirectoryManager>()
                        .GetFiles(this.Model.RootPath, searchOption: SearchOption.TopDirectoryOnly);

                    this.Model.RootPathFiles = new List<RootPathFileViewModel>(files
                        .Select(file => new RootPathFileViewModel
                        {
                            File = file,
                            Exempt = false
                        }));
                }
            });

            this.Errors = new List<ValidationError>();

            this.DataContext = this;

            Task.Run(() =>
            {
                this.Model.FileExtensions = this.Provider.GetAllExtensions().Select(e => new FileExtensionViewModel { Value = e.Value }).ToList();
            });
        }

        public AddTaskViewModel TaskViewModel
        {
            get { return this.Model; }
        }

        public override void InitialiseView(object arg)
        {
            this.Model.Reset();
        }

        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            string param = eventArgs.Parameter.ToString();

            if (param == "Command_FileExtensions_Saved")
            {
                // TODO: What to do with the selected exemptions:
                // Create chips beside the button ?
                // or just list them

                this.OnPropertyChanged(nameof(this.Model.ExemptedFileExtensions));
            }
        }

        private void ViewControlBase_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                this.Errors.Add(e.Error);
            }
            else
            {
                this.Errors.Remove(e.Error);
            }
        }

        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = sender as Button;

                if (button.Name == "Name_ButtonSave")
                {
                    if (this.Errors.Any())
                    {
                        this.Notifier.Notify("Cannot create new task - There are " + this.Errors.Count + " validation errors.");
                    }
                    else
                    {
                        bool immediateStart = false;

                        ITask task = this.CreateTask();
                        EventArgs<ITask, bool> args = new EventArgs<ITask, bool>(task, immediateStart);

                        this.OnViewChange(Home.ViewName, args);
                    }
                }
                else if (button.Name == "Name_ButtonDiscard")
                {
                    this.Notifier.Notify("Item discarded.");

                    this.OnViewChange(Home.ViewName);
                }
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }

        private ITask CreateTask()
        {
            TaskType taskType = this.Model.TaskType;
            OrganiseType organiseType = this.Model.OrganiseType;

            IOrganiseTask task = this.CreateTask(Guid.Parse(this.Model.Identity), organiseType);

            if (taskType == TaskType.Organiser)
            {
                return task;
            }

            return this.CreateScheduledTask("scheduled", task, 5000, 7000);
        }

        private IOrganiseTask CreateTask(Guid identity, OrganiseType type)
        {
            if (type == OrganiseType.File)
            {
                return this.CreateFileOrganiserTask(identity, this.Model.Name, this.Model.Description, new FileOrganiserSettings
                {
                    FileExemptions = this.Model.RootPathFiles.Where(r => r.Exempt).Select(s => s.File).ToArray(),
                    RootPath = this.Model.RootPath,
                    ExtensionExemptions = new string[0]
                });
            }

            return this.CreateDirectoryOrganiserTask(identity, this.Model.Name, this.Model.Description, new DirectoryOrganiserSettings
            {
                RootPath = this.Model.RootPath
            });
        }

        private IOrganiseTask CreateFileOrganiserTask(Guid identity, string name, string description, FileOrganiserSettings settings)
        {
            return new FileOrganiserTask(
                name,
                description,
                settings, 
                this.Factory.Create<IFileExtensionProvider>(), 
                this.Factory.Create<IFileManager>(),
                this.Factory.Create<IDirectoryManager>(),
                identity: identity);
        }

        private IOrganiseTask CreateDirectoryOrganiserTask(Guid identity, string name, string description, DirectoryOrganiserSettings settings)
        {
            return new DirectoryOrganiserTask(
                name,
                description, 
                settings, 
                this.Factory.Create<IDirectoryManager>(),
                identity: identity);
        }

        private ITask CreateScheduledTask(string name, ITask executable, int initialWaitTime, int timerPeriod)
        {
            return new ScheduledTask(
                name,
                string.Format(ScheduledTask.DescriptionFormat, executable.Identity, executable.Description),
                this.Factory.Create<ITimer>(),
                executable,
                initialWaitTime: initialWaitTime,
                timerPeriod: timerPeriod);
        }

        private void Panel_FileExemptions_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine(this.Grid_Exemptions.IsEnabled);
        }

        private void Panel_DirectoryExemptions_Drop(object sender, DragEventArgs e)
        {

        }

        private void ComboBox_OrganiseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox element = sender as ComboBox;

            if (element.SelectedValue != null)
            {
                OrganiseType type = (OrganiseType)element.SelectedValue;

                switch (type)
                {
                    case OrganiseType.File:
                        this.Panel_DirectoryExemptions.Visibility = Visibility.Collapsed;
                        this.Panel_ExtensionExemptions.Visibility = Visibility.Visible;
                        this.Panel_FileExemptions.Visibility = Visibility.Visible;
                        break;
                    case OrganiseType.Directory:
                        this.Panel_DirectoryExemptions.Visibility = Visibility.Visible;
                        this.Panel_ExtensionExemptions.Visibility = Visibility.Collapsed;
                        this.Panel_FileExemptions.Visibility = Visibility.Collapsed;
                        break;
                    case OrganiseType.All:
                        this.Panel_DirectoryExemptions.Visibility = Visibility.Visible;
                        this.Panel_ExtensionExemptions.Visibility = Visibility.Visible;
                        this.Panel_FileExemptions.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            // TODO: adjust ExemptedFileExtensions
            
        }
    }
}