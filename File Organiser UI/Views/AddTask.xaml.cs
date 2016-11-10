namespace File.Organiser.UI.Views
{
    using System.Windows;
    using Controls;

    public partial class AddTask : ViewControlBase
    {
        public const string ViewName = "AddTaskView";

        public AddTask()
            : base(AddTask.ViewName, isDefault: false)
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }
    }
}