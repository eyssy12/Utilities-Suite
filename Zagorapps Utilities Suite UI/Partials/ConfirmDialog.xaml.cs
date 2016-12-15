namespace Zagorapps.Utilities.Suite.UI.Partials
{
    using System;
    using System.Windows.Controls;
    using Core.Library.Events;
    using Events;
    using MaterialDesignThemes.Wpf;

    public partial class ConfirmDialog : UserControl
    {
        public ConfirmDialog()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        public string ConfirmationText { get; set; }

        public string ConfirmParameter { get; set; }

        public event EventHandler<ConfirmDialogEventArgs> OnConfirm;

        private void Button_Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void Button_Confirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Invoker.Raise(ref this.OnConfirm, this, new ConfirmDialogEventArgs(this.ConfirmParameter));
        }
    }
}