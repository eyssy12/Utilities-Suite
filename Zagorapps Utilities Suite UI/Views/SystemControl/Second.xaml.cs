namespace Zagorapps.Utilities.Suite.UI.Views.SystemControl
{
    using System;
    using System.Windows;
    using Commands;
    using Library;
    using Library.Communications;
    using Utilities.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Controls;

    public partial class Second : DataFacilitatorViewControlBase
    {
        public Second(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base("Second", factory, commandProvider)
        {
            this.InitializeComponent();
        }

        public override void InitialiseView(object arg)
        {
            throw new NotImplementedException();
        }

        public override void FinaliseView()
        {
        }

        public override void ProcessMessage(IUtilitiesDataMessage data)
        {
            MessageBox.Show("data received in second view of Test Suite: " + data.Data);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OnDataSendRequest(this, SuiteRoute.System, "Second", "data");
        }
    }
}