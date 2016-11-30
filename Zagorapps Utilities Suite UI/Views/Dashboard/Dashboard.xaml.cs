namespace Zagorapps.Utilities.Suite.UI.Views.Dashboard
{
    using System.Collections.Generic;
    using ViewModels;
    using Zagorapps.Organiser.Library;
    using Zagorapps.Organiser.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Commands;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable]
    public partial class Dashboard : ViewControlBase
    {
        public const string ViewName = nameof(Dashboard);

        private readonly IEnumerable<DashboardItemViewModel> items;

        public Dashboard(IOrganiserFactory factory, ICommandProvider commandProvider) 
            : base(Dashboard.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.DataContext = this;

            this.items = new List<DashboardItemViewModel>
            {
                new DashboardItemViewModel { Identifier = "test" },
                new DashboardItemViewModel { Identifier = "test2" }
            };
        }

        public IEnumerable<DashboardItemViewModel> Items
        {
            get { return this.items; }
        }

        public override void InitialiseView(object arg)
        {
        }
    }
}