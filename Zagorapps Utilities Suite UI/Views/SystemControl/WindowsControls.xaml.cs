namespace Zagorapps.Utilities.Suite.UI.Views.SystemControl
{
    using Commands;
    using Core.Library.Windows;
    using Events;
    using Library.Attributes;
    using Library.Communications;
    using Utilities.Library.Factories;
    using ViewModels;
    using Zagorapps.Utilities.Suite.UI.Controls;

    [DefaultNavigatable]
    public partial class WindowsControls : DataFacilitatorViewControlBase
    {
        public const string ViewName = nameof(WindowsControls);

        protected readonly IWinSystemService WinService;

        protected readonly WindowsControlsViewModel Model;

        public WindowsControls(IOrganiserFactory factory, ICommandProvider commandProvider)
            : base(WindowsControls.ViewName, factory, commandProvider)
        {
            this.InitializeComponent();

            this.WinService = this.Factory.Create<IWinSystemService>();

            this.Model = new WindowsControlsViewModel();

            this.DataContext = this;
        }

        public WindowsControlsViewModel ViewModel
        {
            get { return this.Model; }
        }

        public override void InitialiseView(object arg)
        {

        }

        public override void FinaliseView()
        {
        }

        public override void ProcessMessage(IUtilitiesDataMessage data)
        {
            if (data.Data.ToString().Contains("lock"))
            {
                this.HandleOperation("Lock");
            }
        }

        protected void HandleOperation(string param)
        {
            switch (param)
            {
                case "Lock":
                    this.WinService.LockMachine();
                    break;
                case "LogOff":
                    this.WinService.LogOff();
                    break;
            }
        }

        protected void ConfirmDialog_OnConfirm(object sender, ConfirmDialogEventArgs e)
        {
            this.HandleOperation(e.First);
        }
    }
}
