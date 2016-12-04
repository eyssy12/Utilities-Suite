namespace Zagorapps.Utilities.Suite.UI.Views.Dashboard
{
    using System;
    using System.Collections.Generic;
    using Library.Attributes;
    using Services;
    using Suites;
    using ViewModels;
    using Zagorapps.Utilities.Library.Factories;
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

            this.items = new List<DashboardItemViewModel>
            {
                new DashboardItemViewModel { Identifier = FileOrganiserSuite.Name, ChangeSuiteCommand = this.CommandProvider.CreateRelayCommand<string>(param => this.ChangeSuite(param)) },
                new DashboardItemViewModel { Identifier = ConnectivitySuite.Name, ChangeSuiteCommand = this.CommandProvider.CreateRelayCommand<string>(param => this.ChangeSuite(param)) }
            };

            this.DataContext = this;
        }

        public IEnumerable<DashboardItemViewModel> Items
        {
            get { return this.items; }
        }

        public override void InitialiseView(object arg)
        {
        }

        public override void FinaliseView()
        {
            Console.WriteLine("View finalised");
        }

        private void ChangeSuite(string identifier)
        {
            this.SuiteService.ChangeSuite(identifier);
        }
    }
}