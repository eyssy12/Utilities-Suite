namespace Zagorapps.Utilities.Suite.UI.Views.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using Library.Attributes;
    using Library.Communications;
    using Services;
    using ViewModels;
    using Zagorapps.Utilities.Library.Factories;
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
                .GetAllSuitesOrderByDefaultNavigatable()
                .Select((a, index) => new DashboardItemViewModel
                {
                    Identifier = a.Item1.Name,
                    FriendlyName = (index + 1) + " - " + a.Item1.FriendlyName,
                    ChangeSuiteCommand = this.CommandProvider.CreateRelayCommand<string>(param => this.ChangeSuite(param))
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
            Console.WriteLine(ViewName + " - initialised");
        }

        public override void FinaliseView()
        {
            Console.WriteLine(ViewName + " - View finalised");
        }

        private void ChangeSuite(string identifier)
        {
            this.SuiteService.ChangeSuite(identifier);
        }

        public override void ProcessMessage(IUtilitiesDataMessage data)
        {
        }
    }
}