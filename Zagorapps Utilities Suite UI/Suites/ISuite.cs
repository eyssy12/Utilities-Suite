namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System;
    using System.ComponentModel;
    using Core.Library.Events;
    using Navigation;

    public interface ISuite : INavigatable, INotifyPropertyChanged
    {
        event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        IViewControl ActiveView { get; }

        IViewControl DefaultView { get; }

        void Navigate(string viewName, object args);
    }
}