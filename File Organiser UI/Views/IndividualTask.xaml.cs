namespace File.Organiser.UI.Views
{
    using System;
    using System.Windows;
    using Controls;
    using EyssyApps.Organiser.Library.Factories;

    public partial class IndividualTask : ViewControlBase
    {
        public const string ViewName = nameof(IndividualTask);

        public IndividualTask(IOrganiserFactory factory)
            : base(IndividualTask.ViewName, isDefault: false, factory: factory)
        {
            this.InitializeComponent();
        }

        public override void ActivateView()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnViewChange(Home.ViewName);
        }
    }
}