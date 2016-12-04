namespace Zagorapps.Utilities.Suite.UI.Views.SystemControl
{
    using System;
    using System.Windows;
    using Commands;
    using Library;
    using Library.Attributes;
    using Library.Communications;
    using Utilities.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable]
    public partial class First : DataFacilitatorViewControlBase
    {
        public First(IOrganiserFactory factory, ICommandProvider commandProvider) 
            : base("First", factory, commandProvider)
        {
            this.InitializeComponent();
        }

        public override void InitialiseView(object arg)
        {
            
        }

        public override void FinaliseView()
        {
        }

        public override void ProcessMessage(IUtilitiesDataMessage data)
        {
            Console.WriteLine("First - Data Received: " + data.Data);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnDataSendRequest(this, SuiteRoute.System, "Second", "data");
        }
    }
}