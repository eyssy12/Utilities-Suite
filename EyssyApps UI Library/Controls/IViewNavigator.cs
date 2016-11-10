namespace EyssyApps.UI.Library.Controls
{
    using System;
    using EyssyApps.Core.Library.Events;

    public interface IViewNavigator
    {
        event EventHandler<EventArgs<IViewControl>> OnViewChanged;

        void Navigate(string viewName);

        IViewControl ActiveView { get; }
    }
}