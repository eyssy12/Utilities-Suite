namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    using System;
    using System.ComponentModel;
    using Core.Library.Events;

    public interface ISuite : INavigatable, INotifyPropertyChanged
    {
        event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        IViewControl ActiveView { get; }

        IViewControl DefaultView { get; }

        void Navigate(string viewName, object args);
    }
}