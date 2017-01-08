namespace Zagorapps.Utilities.Suite.UI.Views.Dashboard
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using Library.Attributes;
    using Library.Communications;
    using Services;
    using Suites;
    using ViewModels;
    using Zagorapps.Utilities.Suite.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Commands;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable(Dashboard.ViewName)]
    public partial class Dashboard : DataFacilitatorViewControlBase
    {
        private const string ViewName = nameof(Dashboard);

        protected readonly ISuiteService SuiteService;

        private readonly IEnumerable<DashboardItemViewModel> items;

        public Dashboard(IOrganiserFactory factory, ICommandProvider commandProvider) 
            : base(Dashboard.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.SuiteService = this.Factory.Create<ISuiteService>();

            this.items = Assembly
                .GetExecutingAssembly()
                .GetAllSuitesOrderByDefaultNavigatable(exclusions: DashboardSuite.Name)
                .Select((a, index) => new DashboardItemViewModel
                {
                    Identifier = a.Item1.Name,
                    FriendlyName = a.Item1.FriendlyName,
                    FriendlyNameWithIndex = (index + 1) + " - " + a.Item1.FriendlyName,
                    ChangeSuiteCommand = this.CommandProvider.CreateRelayCommand<string>(this.SuiteService.ChangeSuite)
                })
                .ToArray();

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
        }

        protected override void HandleSuiteMessageAsync(IUtilitiesDataMessage data)
        {
            // TODO: perhaps implement logic for displaying the amount of new events that have occured in the suite since the users last visit to it
            // what classifies as an event in a suite and view? 
        }
    }
}