namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Windows;
    using Zagorapps.Core.Library.Events;
    using static Enumerations;

    public interface ISystemTrayControl : IDisposable
    {
        event EventHandler<EventArgs<TrayState>> StateChanged;

        void SetVisibility(Visibility visiblity);
    }
}