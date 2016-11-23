namespace File.Organiser.UI.Controls
{
    using System;
    using System.Windows;
    using EyssyApps.Core.Library.Events;
    using static Enumerations;

    public interface ISystemTrayControl : IDisposable
    {
        event EventHandler<EventArgs<TrayState>> StateChanged;

        void SetVisibility(Visibility visiblity);
    }
}