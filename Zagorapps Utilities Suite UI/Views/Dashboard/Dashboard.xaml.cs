namespace Zagorapps.Utilities.Suite.UI.Views.Dashboard
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using Services;
    using Suites;
    using ViewModels;
    using Zagorapps.Organiser.Library;
    using Zagorapps.Organiser.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Commands;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable]
    public partial class Dashboard : ViewControlBase
    {
        public const string ViewName = nameof(Dashboard);

        protected readonly ISuiteService SuiteService;

        private readonly IEnumerable<DashboardItemViewModel> items;

        public Dashboard(IOrganiserFactory factory, ICommandProvider commandProvider) 
            : base(Dashboard.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.SuiteService = this.Factory.Create<ISuiteService>();

            this.DataContext = this;

            this.items = new List<DashboardItemViewModel>
            {
                new DashboardItemViewModel { Identifier = FileOrganiserSuite.Name },
                new DashboardItemViewModel { Identifier = ConnectivitySuite.Name }
            };
        }

        public IEnumerable<DashboardItemViewModel> Items
        {
            get { return this.items; }
        }

        public override void InitialiseView(object arg)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DashboardItemViewModel item = (sender as Button).DataContext as DashboardItemViewModel;

            this.SuiteService.ChangeSuite(item.Identifier);
        }
    }
}