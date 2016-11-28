namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using Zagorapps.Core.Library.Events;

    public interface IViewControl : INotifyPropertyChanged
    {
        string ViewControlName { get; }

        bool IsActive { get; set; }

        event EventHandler<EventArgs<string, object>> OnChangeView;

        void InitialiseView(object arg);
    }
}