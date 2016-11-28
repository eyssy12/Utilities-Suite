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
            : base(IndividualTask.ViewName, factory, commandProvider)
        {
            InitializeComponent();
        }

        public override void InitialiseView(object arg)
        {
            Console.WriteLine(arg);
        }
    }
}
