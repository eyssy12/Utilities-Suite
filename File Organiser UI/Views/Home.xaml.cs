namespace File.Organiser.UI.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using EyssyApps.Core.Library.Managers;
    using EyssyApps.Core.Library.Native;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.Organiser.Library.Managers;
    using EyssyApps.Organiser.Library.Models.Settings;
    using EyssyApps.Organiser.Library.Providers;
    using EyssyApps.Organiser.Library.Tasks;
    using IoC;
    using MaterialDesignThemes.Wpf;
    using ViewModels;

    public partial class Home : UserControl
    {
        protected readonly IOrganiserFactory Factory;

        protected readonly ITaskManager Manager;

        private readonly Lazy<Snackbar> Snackbar;

        public Home()
        {
            InitializeComponent();

            this.Snackbar = new Lazy<Snackbar>(() => (Snackbar)Application.Current.MainWindow.FindName("MainSnackbar"));

            this.Factory = DependencyProvider.Get<IOrganiserFactory>();
            this.Manager = this.Factory.Create<ITaskManager>();

            IFileManager fileManager = this.Factory.Create<IFileManager>();
            IDirectoryManager directoryManager = this.Factory.Create<IDirectoryManager>();
            IFileExtensionProvider provider = this.Factory.Create<IFileExtensionProvider>();

            // TODO: dont allow to create tasks of the same type for the same root path, i.e. Two seperate tasks for directory organiser with the same root path
            FileOrganiserSettings settings = new FileOrganiserSettings
            {
                RootPath = KnownFolders.GetPath(KnownFolder.Downloads),
                DirectoryExemptions = new List<string> { },
                ExtensionExemptions = new List<string> { },
                FileExemptions = new List<string>() { }
            };

            // TODO: save/laod feature
            FileOrganiserTask fileTask = new FileOrganiserTask(Guid.NewGuid(), "Sorts the files in the Downloads folder", settings, provider, fileManager, directoryManager);
            DirectoryOrganiserTask directoryTask = new DirectoryOrganiserTask(Guid.NewGuid(), "Sorts the individual directories in the Downloads folder", settings, directoryManager);

            this.Manager.Add(fileTask);
            this.Manager.Add(directoryTask);

            this.DataContext = this;
        }

        public IEnumerable<TaskViewModel> Tasks
        {
            // TODO: this should be properly wired up through eventhandlers so that the ui knows when the viewmodels property has changed
        
            get
            {
                return this.Manager
                    .GetAll()
                    .Select(t =>
                    {
                        return new TaskViewModel
                        {
                            ID = t.Id.ToString(),
                            TaskType = t.TaskType,
                            State = t.State,
                            Description = t.Description
                        };
                    })
                    .ToArray();
            }
        }

        private void RunTask(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button runTask = sender as Button;

                string id = (runTask.DataContext as TaskViewModel).ID;

                ITask task = this.Manager.FindById(Guid.Parse(id));

                this.Snackbar.Value.MessageQueue.Enqueue("Task '" + id + "' invoked");

                task.Execute();
            }
        }

        private void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Snackbar.Value.MessageQueue.Enqueue(((ButtonBase)sender).Content.ToString());
        }

        private void Button_AddTask_Click(object sender, RoutedEventArgs e)
        {
            this.Manager.Add(new FileOrganiserTask(Guid.NewGuid(), "Sorts the files in the Downloads folder", null, null, null, null));
            this.TasksGrid.ItemsSource = null;
            this.TasksGrid.ItemsSource = this.Tasks;

            this.Snackbar.Value.MessageQueue.Enqueue("New task created");
        }

        private void TerminateApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
