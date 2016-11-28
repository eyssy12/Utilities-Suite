namespace Zagorapps.Utilities.Suite.UI.Services
{
    using System;
    using MaterialDesignThemes.Wpf;

    public class SnackbarNotificationService : ISnackbarNotificationService
    {
        protected readonly Lazy<Snackbar> Snackbar;

        public SnackbarNotificationService(Lazy<Snackbar> snackbar)
        {
            if (snackbar == null)
            {
                throw new ArgumentNullException(nameof(snackbar), ""); //TODO: message
            }

            this.Snackbar = snackbar;
        }

        public SnackbarNotificationService(Snackbar snackbar) : this(new Lazy<Snackbar>(() => snackbar))
        {
        }

        public void Notify(string contents)
        {
            this.Snackbar.Value.MessageQueue.Enqueue(contents);
        }
    }
}