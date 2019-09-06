namespace Zagorapps.Utilities.Suite.UI.Views.Organiser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Commands;
    using Controls;
    using Library.Attributes;
    using MaterialDesignThemes.Wpf;
    using Services;
    using ViewModels;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Core.Library.Managers;
    using Zagorapps.Core.Library.Timing;
    using Zagorapps.Utilities.Suite.Library;
    using Zagorapps.Utilities.Suite.Library.Factories;
    using Zagorapps.Utilities.Suite.Library.Managers;
    using Zagorapps.Utilities.Suite.Library.Models.Settings;
    using Zagorapps.Utilities.Suite.Library.Providers;
    using Zagorapps.Utilities.Suite.Library.Tasks;

    [Navigatable(AddTask.ViewName)]
    public partial class AddTask : ViewControlBase
    {
        private const string ViewName = nameof(AddTask);

        protected readonly AddTaskViewModel Model;

        protected readonly ITaskManager Manager;
        protected readonly ISnackbarNotificationService Notifier;
        protected readonly IOrganiserSettingsProvider SettingsProvider;
        protected readonly IFileExtensionProvider ExtensionProvider;
        protected readonly IList<ValidationError> Errors;

        public AddTask(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(AddTask.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.Manager = this.Factory.Create<ITaskManager>();
            this.SettingsProvider = this.Factory.Create<IOrganiserSettingsProvider>();
            this.ExtensionProvider = this.Factory.Create<IFileExtensionProvider>();
            this.Notifier = this.Factory.Create<ISnackbarNotificationService>();

            this.Model = new AddTaskViewModel();
            this.Model.SelectRootPathCommand = this.CommandProvider.CreateRelayCommand(() => this.Model.RootPath = this.Factory.Create<IFormsService>().SelectFolderPathDialog());
            this.Model.LoadRootPathFilesCommand = this.CommandProvider.CreateRelayCommand(() =>
            {
                if (!string.IsNullOrWhiteSpace(this.Model.RootPath))
                {
                    IEnumerable<string> files = this.Factory
                        .Create<IDirectoryManager>()
                        .GetFiles(this.Model.RootPath, searchOption: SearchOption.TopDirectoryOnly);

                    this.Model.FileRootPathFiles = new List<RootPathFileViewModel>(files
                        .Select(file => new RootPathFileViewModel
                        {
                            Path = file,
                            FileName = Path.GetFileName(file),
                            Exempt = false
                        })
                        .ToArray());
                }
            });
            this.Model.LoadDirectoryRootPathFilesCommand = this.CommandProvider.CreateRelayCommand(() =>
            {
                if (!string.IsNullOrWhiteSpace(this.Model.RootPath))
                {
                    IEnumerable<string> files = this.Factory
                        .Create<IDirectoryManager>()
                        .GetDirectores(this.Model.RootPath, searchOption: SearchOption.TopDirectoryOnly);

                    this.Model.DirectoryRootPathFiles = new List<RootPathFileViewModel>(files
                        .Select(file => new RootPathFileViewModel
                        {
                            Path = file,
                            FileName = Path.GetFileName(file),
                            Exempt = false
                        })
                        .ToArray());
                }
            });

            this.Errors = new List<ValidationError>();

            this.DataContext = this;

            this.Model.FileExtensions = this.ExtensionProvider.GetAllExtensions().Select(e => new FileExtensionViewModel { Value = e.Value }).ToList();
            this.Model.Categories = this.ExtensionProvider
                .GetAllCategories()
                .Select(c => new CategoriesViewModel
                {
                    Category = c.Value,
                    Extensions = c.Extensions.Select(e => new FileExtensionViewModel { Value = e.Value })
                })
                .ToList();
        }

        public AddTaskViewModel TaskViewModel
        {
            get { return this.Model; }
        }

        public override void InitialiseView(object arg)
        {
        }

        public override void FinaliseView()
        {
            this.Model.Reset();
        }

        protected void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            string param = eventArgs.Parameter.ToString();

            if (param == "Command_FileExtensions_Saved")
            {
                IList<FileExtensionViewModel> temp = new List<FileExtensionViewModel>();

                foreach (var item in this.ListBox_FileExtensions.SelectedItems)
                {
                    temp.Add(item as FileExtensionViewModel);
                }

                this.Model.ExemptedFileExtensions = temp;
                this.ListBox_ExemptedFileExtensions.ItemsSource = this.Model.ExemptedFileExtensions;
            }
        }

        protected void ViewControlBase_Error(object sender, ValidationErrorEventArgs e)
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

        protected void ToolbarButton_Click(object sender, RoutedEventArgs e)
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

                        this.OnViewChange(ViewBag.GetViewName<Home>(), args);
                    }
                }
                else if (button.Name == "Name_ButtonDiscard")
                {
                    this.Notifier.Notify("Item discarded.");

                    this.OnViewChange(ViewBag.GetViewName<Home>());
                }
            }
        }

        protected void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(ViewBag.GetViewName<Home>());
        }

        protected ITask CreateTask()
        {
            TaskType taskType = this.Model.TaskType;
            OrganiseType organiseType = this.Model.OrganiseType;

            IOrganiserTask task = this.CreateTask(Guid.Parse(this.Model.Identity), organiseType);

            if (taskType == TaskType.Organiser)
            {
                return task;
            }

            return this.CreateScheduledTask(this.Model.Name, task, 5000, 7000);
        }

        protected IOrganiserTask CreateTask(Guid identity, OrganiseType type)
        {
            if (type == OrganiseType.File)
            {
                return this.CreateFileOrganiserTask(identity, this.Model);
            }

            return this.CreateDirectoryOrganiserTask(identity, this.Model);
        }

        protected IOrganiserTask CreateFileOrganiserTask(Guid identity, AddTaskViewModel model)
        {
            this.SettingsProvider.Save(new FileOrganiserSettings
            {
                Reference = identity,
                FileExemptions = model.FileRootPathFiles.Where(r => r.Exempt).Select(s => s.Path).ToArray(),
                RootPath = model.RootPath,
                ExtensionExemptions = model.ExemptedFileExtensions.Select(e => e.Value).ToArray()
            });

            return new FileOrganiserTask(
                model.Name,
                model.Description,
                this.Factory.Create<IOrganiserSettingsProvider>(), 
                this.Factory.Create<IFileExtensionProvider>(), 
                this.Factory.Create<IFileManager>(),
                this.Factory.Create<IDirectoryManager>(),
                identity: identity);
        }

        protected IOrganiserTask CreateDirectoryOrganiserTask(Guid identity, AddTaskViewModel model)
        {
            this.SettingsProvider.Save(new DirectoryOrganiserSettings
            {
                Reference = identity,
                RootPath = model.RootPath
            });

            return new DirectoryOrganiserTask(
                model.Name,
                model.Description,
                this.Factory.Create<IOrganiserSettingsProvider>(),
                this.Factory.Create<IDirectoryManager>(),
                identity: identity);
        }

        protected ITask CreateScheduledTask(string name, ITask executable, int initialWaitTime, int timerPeriod)
        {
            return new ScheduledTask(
                name,
                string.Format(ScheduledTask.DescriptionFormat, executable.Identity, executable.Description),
                this.Factory.Create<ITimer>(),
                executable,
                initialWaitTime: initialWaitTime,
                timerPeriod: timerPeriod);
        }

        protected void ComboBox_OrganiseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        protected void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.AdjustScrollviewer((ScrollViewer)sender, e);
        }

        protected void MainScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.AdjustScrollviewer((ScrollViewer)sender, e);
        }

        protected void AdjustScrollviewer(ScrollViewer scroller, MouseWheelEventArgs args)
        {
            scroller.ScrollToVerticalOffset(scroller.VerticalOffset - args.Delta);

            args.Handled = true;
        }
    }
}