namespace Zagorapps.Utilities.Suite.UI.Partials
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Core.Library.Events;
    using Events;

    public partial class ConfirmDialog : UserControl
    {
        protected const string DefaultConfirmationTitle = "Are you sure you want to invoke this operation?";

        public ConfirmDialog()
        {
            this.InitializeComponent();

            this.ConfirmationText = ConfirmDialog.DefaultConfirmationTitle;

            this.DataContext = this;
        }

        public string ConfirmationText { get; set; }

        public string ConfirmParameter { get; set; }

        public string CancelParameter { get; set; }

        public event EventHandler<ConfirmDialogEventArgs> OnConfirm;

        public event EventHandler<ConfirmDialogEventArgs> OnCancel;

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Invoker.Raise(ref this.OnCancel, this, new ConfirmDialogEventArgs(this.CancelParameter));
        }

        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            Invoker.Raise(ref this.OnConfirm, this, new ConfirmDialogEventArgs(this.ConfirmParameter));
        }
    }
}