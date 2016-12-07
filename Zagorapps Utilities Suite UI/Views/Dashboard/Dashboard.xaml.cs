namespace Zagorapps.Utilities.Suite.UI.Views.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Library.Attributes;
    using Library.Communications;
    using Services;
    using ViewModels;
    using Zagorapps.Utilities.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Commands;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable]
    public partial class Dashboard : DataFacilitatorViewControlBase
    {
        public const string ViewName = nameof(Dashboard);

        protected readonly ISuiteService SuiteService;

        private readonly StringBuilder builder = new StringBuilder();

        private readonly IEnumerable<DashboardItemViewModel> items;

        public Dashboard(IOrganiserFactory factory, ICommandProvider commandProvider) 
            : base(Dashboard.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.SuiteService = this.Factory.Create<ISuiteService>();

            this.items = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsDefined(typeof(SuiteAttribute), false))
                    .Select(t => t.GetCustomAttribute<SuiteAttribute>())
                    .Select(a => new DashboardItemViewModel
                    {
                        Identifier = a.Name,
                        FriendlyName = a.FriendlyName,
                        ChangeSuiteCommand = this.CommandProvider.CreateRelayCommand<string>(param => this.ChangeSuite(param))
                    })
                    .ToArray();

            this.DataContext = this;
        }

        public string Text
        {
            get { return this.builder.ToString(); }
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
            Console.WriteLine("View finalised");
        }

        private void ChangeSuite(string identifier)
        {
            this.SuiteService.ChangeSuite(identifier);
        }

        public override void ProcessMessage(IUtilitiesDataMessage data)
        {
            builder.Append(data.Data + " ");

            this.OnPropertyChanged(nameof(this.Text));
        }
    }
}