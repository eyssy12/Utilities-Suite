namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using Zagorapps.Core.Library.Events;

    public interface IViewControl : INotifyPropertyChanged
    {
        event EventHandler<EventArgs<string, object>> OnChangeView;

        string ViewControlName { get; }

        bool IsActive { get; set; }

        void InitialiseView(object arg);
    }
}