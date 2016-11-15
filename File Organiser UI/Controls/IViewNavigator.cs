namespace File.Organiser.UI.Controls
{
    using System;
    using EyssyApps.Core.Library.Events;

    public interface IViewNavigator
    {
        event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        void Navigate(string viewName, object args);

        IViewControl ActiveView { get; }
    }
}