namespace Zagorapps.Utilities.Suite.UI.Events
{
    using Core.Library.Events;

    public class ConfirmDialogEventArgs : EventArgs<string>
    {
        public ConfirmDialogEventArgs(string confirmParameter)
            : base(confirmParameter)
        {
        }
    }
}