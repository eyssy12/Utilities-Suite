namespace File.Organiser.UI.Views
{
    using System.Windows;
    using Controls;
    using EyssyApps.Organiser.Library.Factories;

    public partial class AddTask : ViewControlBase
    {
        public const string ViewName = nameof(AddTask);

        public AddTask(IOrganiserFactory factory)
            : base(AddTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }
    }
}