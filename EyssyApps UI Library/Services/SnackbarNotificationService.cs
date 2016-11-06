namespace EyssyApps.UI.Library.Services
{
    using System;
    using Core.Library.Messaging;
    using MaterialDesignThemes.Wpf;

    public class SnackbarNotificationService : ISnackbarNotificationService
    {
        protected readonly Snackbar Snackbar;

        public SnackbarNotificationService(Snackbar snackbar)
        {
            if (snackbar == null)
            {
                throw new ArgumentNullException(nameof(snackbar), ""); //TODO: message
            }

            this.Snackbar = snackbar;
        }

        public void Notify(string contents)
        {
            this.Snackbar.MessageQueue.Enqueue(contents);
        }
    }
}