namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Zagorapps.Core.Library.Events;

    public interface IViewNavigator
    {
        event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        void Navigate(string viewName, object args);

        IViewControl ActiveView { get; }
    }
}