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

        private static readonly DependencyProperty ConfirmationTextProperty = DependencyProperty.Register(nameof(ConfirmationText), typeof(string), typeof(ConfirmDialog)),
            ConfirmParamaterProperty = DependencyProperty.Register(nameof(ConfirmParameter), typeof(string), typeof(ConfirmDialog)),
            CancelParamaterProperty = DependencyProperty.Register(nameof(CancelParameter), typeof(string), typeof(ConfirmDialog));

        public ConfirmDialog()
        {
            this.InitializeComponent();

            this.ConfirmationText = ConfirmDialog.DefaultConfirmationTitle;

            this.DataContext = this;
        }

        public string ConfirmationText
        {
            get { return (string)this.GetValue(ConfirmDialog.ConfirmationTextProperty); }
            set { this.SetValue(ConfirmDialog.ConfirmationTextProperty, value); }
        }

        public string ConfirmParameter
        {
            get { return (string)this.GetValue(ConfirmDialog.ConfirmParamaterProperty); }
            set { this.SetValue(ConfirmDialog.ConfirmParamaterProperty, value); }
        }

        public string CancelParameter
        {
            get { return (string)this.GetValue(ConfirmDialog.CancelParamaterProperty); }
            set { this.SetValue(ConfirmDialog.CancelParamaterProperty, value); }
        }

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