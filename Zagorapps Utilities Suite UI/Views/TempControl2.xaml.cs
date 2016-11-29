using System;
using Zagorapps.Organiser.Library.Factories;
using Zagorapps.Utilities.Suite.UI.Commands;
using Zagorapps.Utilities.Suite.UI.Controls;

namespace Zagorapps.Utilities.Suite.UI.Views
{
    public partial class TempControl2 : ViewControlBase
    {
        public const string ViewName = nameof(TempControl2);

        public TempControl2(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(TempControl2.ViewName, factory, commandProvider)
        {
            InitializeComponent();
        }

        public override void InitialiseView(object arg)
        {
            Console.WriteLine(arg);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OnViewChange(TempControl.ViewName);
        }
    }
}
