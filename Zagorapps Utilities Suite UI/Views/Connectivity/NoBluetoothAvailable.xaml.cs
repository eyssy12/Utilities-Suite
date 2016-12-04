namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using Commands;
    using Library.Attributes;
    using Library.Communications;
    using Zagorapps.Utilities.Library;
    using Zagorapps.Utilities.Library.Factories;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable]
    public partial class NoBluetoothAvailable : ViewControlBase
    {
        public const string ViewName = nameof(NoBluetoothAvailable);

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
    }
}