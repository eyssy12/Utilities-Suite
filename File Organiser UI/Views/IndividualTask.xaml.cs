namespace File.Organiser.UI.Views
{
    using System.Windows;
    using Controls;

    public partial class IndividualTask : ViewControlBase
    {
        public const string ViewName = "IndividualTaskView";

        public IndividualTask()
            : base(IndividualTask.ViewName, isDefault: false)
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }
    }
}