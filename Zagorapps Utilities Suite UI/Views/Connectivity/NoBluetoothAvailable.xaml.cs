namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using Commands;
    using Library.Attributes;
    using Zagorapps.Utilities.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [Navigatable(NoBluetoothAvailable.ViewName)]
    public partial class NoBluetoothAvailable : ViewControlBase
    {
        private const string ViewName = nameof(NoBluetoothAvailable);

        public NoBluetoothAvailable(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(NoBluetoothAvailable.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();
        }

        public override void InitialiseView(object arg)
        {
            Console.WriteLine(ViewName + " - initialised");
        }

        public override void FinaliseView()
        {
            Console.WriteLine(ViewName + " - finalised");
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OnViewChange(ViewBag.GetViewName<UdpConnection>());
        }
    }
}