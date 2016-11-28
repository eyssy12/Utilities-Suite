namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using Core.Library.Events;

    public interface ISuite : INotifyPropertyChanged
    {
        event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        string SuiteName { get; }

        void Navigate(string viewName, object args);

        bool IsActive { get; set; }

        IViewControl ActiveView { get; }

        IViewControl DefaultView { get; }
    }
}