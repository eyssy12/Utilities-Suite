using System;
using Zagorapps.Organiser.Library;
using Zagorapps.Organiser.Library.Factories;
using Zagorapps.Utilities.Suite.UI.Commands;
using Zagorapps.Utilities.Suite.UI.Controls;

namespace Zagorapps.Utilities.Suite.UI.Views
{
    [DefaultEntity]
    public partial class TempControl : ViewControlBase
    {
        public const string ViewName = nameof(TempControl);

        public TempControl(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(TempControl.ViewName, factory, commandProvider)
        {
            InitializeComponent();
        }

        public override void InitialiseView(object arg)
        {
            Console.WriteLine(arg);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OnViewChange(TempControl2.ViewName);
        }
    }
}
