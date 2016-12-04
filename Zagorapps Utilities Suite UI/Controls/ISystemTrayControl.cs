namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Windows;
    using Zagorapps.Core.Library.Events;

    public interface ISystemTrayControl : IDisposable
    {
        event EventHandler<EventArgs<TrayState>> StateChanged;

        void SetVisibility(Visibility visiblity);
    }
}