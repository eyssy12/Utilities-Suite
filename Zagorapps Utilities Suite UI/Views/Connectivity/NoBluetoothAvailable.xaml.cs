namespace Zagorapps.Utilities.Suite.UI.Views.Connectivity
{
    using System;
    using Commands;
    using Zagorapps.Organiser.Library;
    using Zagorapps.Organiser.Library.Factories;
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
            Console.WriteLine(arg);
        }
    }
}