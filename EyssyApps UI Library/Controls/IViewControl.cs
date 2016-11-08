namespace EyssyApps.UI.Library.Controls
{
    using System;
    using EyssyApps.Core.Library.Events;

    public interface IViewControl
    {
        event EventHandler<EventArgs<string>> ChangeView;
    }
}